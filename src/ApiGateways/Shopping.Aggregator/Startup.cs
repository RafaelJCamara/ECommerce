using System;
using System.Net.Http;
using System.Security.Cryptography;
using Common.Logging;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Polly;
using Polly.Extensions.Http;
using Serilog;
using Shopping.Aggregator.HttpHandlers;
using Shopping.Aggregator.Services;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System;

namespace Shopping.Aggregator
{
    /*
        -> Startup class is responsible for the application's configuration.
     */
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<LoggingDelegatingHandler>();

            services.AddHttpClient<ICatalogService, CatalogService>(c =>
                    c.BaseAddress = new Uri(Configuration["ApiSettings:CatalogUrl"]))
                .AddHttpMessageHandler<LoggingDelegatingHandler>()
                // specifies the retry policy
                .AddPolicyHandler(GetRetryPolicy())
                // specifies the circuit break policy
                .AddPolicyHandler(GetCircuitBreakerPolicy());

            services.AddHttpClient<IBasketService, BasketService>(c =>
                    c.BaseAddress = new Uri(Configuration["ApiSettings:BasketUrl"]))
                .AddHttpMessageHandler<LoggingDelegatingHandler>()
                .AddHttpMessageHandler<AuthenticationDelegatingHandler>()
                // specifies the retry policy
                .AddPolicyHandler(GetRetryPolicy())
                // specifies the circuit break policy
                .AddPolicyHandler(GetCircuitBreakerPolicy());

            services.AddHttpClient<IOrderService, OrderService>(c =>
                    c.BaseAddress = new Uri(Configuration["ApiSettings:OrderingUrl"]))
                .AddHttpMessageHandler<LoggingDelegatingHandler>()
                .AddHttpMessageHandler<AuthenticationDelegatingHandler>()
                // specifies the retry policy
                .AddPolicyHandler(GetRetryPolicy())
                // specifies the circuit break policy
                .AddPolicyHandler(GetCircuitBreakerPolicy());

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Shopping.Aggregator", Version = "v1" });
            });

            services
                .AddHealthChecks()
                // since our aggregator needs to communicate with our microservices, we must add such health check
                // since we are communicating with microservices, we must add as a AddUrlGroup
                .AddUrlGroup(new Uri($"{Configuration["ApiSettings:CatalogUrl"]}/swagger/index.html"), "Catalog.API", HealthStatus.Degraded)
                .AddUrlGroup(new Uri($"{Configuration["ApiSettings:BasketUrl"]}/swagger/index.html"), "Basket.API", HealthStatus.Degraded)
                .AddUrlGroup(new Uri($"{Configuration["ApiSettings:OrderingUrl"]}/swagger/index.html"), "Ordering.API", HealthStatus.Degraded);

            services
                .AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = Configuration.GetValue<string>("IdentityServerConfiguration:Uri");
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false
                    };
                });
            services.AddHttpContextAccessor();
            services.AddTransient<AuthenticationDelegatingHandler>();
            services.AddAuthorization();
            services.AddOpenTelemetryTracing(builder =>
            {
                builder
                        .SetResourceBuilder(ResourceBuilder
                                                            .CreateDefault()
                                                            .AddService("Shopping.Aggregator")
                                            )
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .SetSampler(new AlwaysOnSampler())
                        .AddZipkinExporter(o =>
                        {
                            o.Endpoint = new Uri(Configuration["ZipkinExporterConfig:Uri"]);
                        });
            });
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(
                    retryCount: 5,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    onRetry: (exception, retryCount, context) =>
                    {
                        Log.Error($"Retry {retryCount} of {context.PolicyKey} at {context.OperationKey}, due to: {exception}.");
                    });
        }

        private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(
                    handledEventsAllowedBeforeBreaking: 5,
                    durationOfBreak: TimeSpan.FromSeconds(30)
                );
        }
        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Shopping.Aggregator v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { 
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/hc", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            });
        }
    }
}
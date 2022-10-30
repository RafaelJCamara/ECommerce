using Basket.API.GrpcServices;
using Basket.API.Repositories;
using Discount.Grpc.Protos;
using HealthChecks.UI.Client;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System;

namespace Basket.API
{
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
            services.AddAutoMapper(typeof(Startup));
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration.GetValue<string>("CacheSettings:ConnectionString");
            });
            services.AddControllers();
            services.AddScoped<IBasketRepository, BasketRepository>();
            services
                .AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>
                (
                    options => options.Address = new Uri(Configuration.GetValue<string>("GrpcSettings:DiscountUrl"))
                );
            services.AddScoped<DiscountGrpcService>();
            services.AddMassTransit(config =>
            {
                config.UsingRabbitMq((ctx, cfg) => { 
                    cfg.Host(Configuration["EventBusSettings:HostAddress"]);
                    cfg.UseHealthCheck(ctx);
                });
            });
            services.AddMassTransitHostedService();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Basket.API", Version = "v1" });
            });

            services
                //registers the possibility of performing health checks        
                .AddHealthChecks()
                //registers the health checks of the services our main service depends on (in this case Redis)
                .AddRedis(
                        Configuration["CacheSettings:ConnectionString"],
                        "Basket Redis Health Check",
                        HealthStatus.Degraded
                );

            services.AddOpenTelemetryTracing(builder =>
            {
                builder
                        .SetResourceBuilder(ResourceBuilder
                                                            .CreateDefault()
                                                            .AddService("Basket.API")
                                            )
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .SetSampler(new AlwaysOnSampler())
                        .AddZipkinExporter(o =>
                        {
                            o.Endpoint = new Uri("http://localhost:9411/api/v2/spans");
                        });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Basket.API v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(
                endpoints => { 
                    endpoints.MapControllers();
                    //creates a route to perform te health check
                    // the addition of HealthCheckOptions is so that the result of /hc route is a json and not a simple text
                    endpoints.MapHealthChecks("/hc", new HealthCheckOptions()
                    {
                        Predicate = _ => true,
                        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                    });
                }
            );
        }
    }
}
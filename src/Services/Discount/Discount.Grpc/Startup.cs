using Discount.Grpc.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System;

namespace Discount.Grpc
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDiscountRepository, DiscountRepository>();
            services.AddAutoMapper(typeof(Startup));
            services.AddGrpc();
            services.AddOpenTelemetryTracing(builder =>
            {
                builder
                        .SetResourceBuilder(ResourceBuilder
                                                            .CreateDefault()
                                                            .AddService("Discount.GRPC")
                                            )
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .SetSampler(new AlwaysOnSampler())
                        .AddGrpcClientInstrumentation(opt => opt.SuppressDownstreamInstrumentation = true)
                        .AddNpgsql()
                        .AddZipkinExporter(o =>
                        {
                            o.Endpoint = new Uri(Configuration["ZipkinExporterConfig:Uri"]);
                        });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<DiscountService>();
            });
        }
    }
}
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using RockLib.DistributedTracing.AspNetCore.Config;

namespace RockLib.DistributedTracing.AspNetCore
{
    /// <summary>
    /// Middleware to add OpenTelemetry to an application
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Bind OpenTelemetry to the application, derived from appsettings
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddOpenTelemetry(this IServiceCollection services, IConfiguration configuration)
        {
            var config = configuration.GetSection("RockLib.DistributedTracing").Get<TracerConfig>();

            switch (config.Exporter?.Trim().ToLower())
            {
                case "jaeger":
                    services.AddOpenTelemetryTracing((builder) => builder
                        .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(config.ServiceName))
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddJaegerExporter(jaegerOptions =>
                        {
                            var hostPort = config.ServiceEndpoint.Split(':');
                            string host = hostPort[0];
                            string port = hostPort.Length > 1 ? hostPort[1] : "6831"; // default from https://www.jaegertracing.io/docs/1.21/deployment/
                            jaegerOptions.AgentHost = host;
                            jaegerOptions.AgentPort = Convert.ToInt32(port);
                            jaegerOptions.MaxPayloadSizeInBytes = 65000; // need to set this for Jaeger v1.21 to workaround a known issue
                            jaegerOptions.ProcessTags = new List<KeyValuePair<string, object>>()
                            {
                                new KeyValuePair<string, object>("ApplicationId", config.ApplicationId),
                                new KeyValuePair<string, object>("ServiceName", config.ServiceName),
                                new KeyValuePair<string, object>("Environment", config.Environment),
                            };
                        }));
                    break;
                case "zipkin":
                //    services.AddOpenTelemetryTracing((builder) => builder
                //        .AddAspNetCoreInstrumentation()
                //        .AddHttpClientInstrumentation()
                //        .AddZipkinExporter(zipkinOptions =>
                //        {
                //            zipkinOptions.ServiceName = config.ServiceName;
                //            zipkinOptions.Endpoint = new Uri(config.ServiceEndpoint);
                //        }));
                //    break;

                case "otlp":
                //    services.AddOpenTelemetryTracing((builder) => builder
                //        .AddAspNetCoreInstrumentation()
                //        .AddHttpClientInstrumentation()
                //        .AddOtlpExporter(otlpOptions =>
                //        {
                //            otlpOptions.ServiceName = config.ServiceName;
                //            otlpOptions.Endpoint = config.ServiceEndpoint;
                //        }));
                //    break;
                default:
                    services.AddOpenTelemetryTracing((builder) => builder
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddConsoleExporter());
                    break;
            }
        }

        //private static TracerConfig GetTracerConfigFromEnvironmentVariables()
        //{
        //    var serviceEndpoint = Environment.GetEnvironmentVariable("ROCKLIB_TRACER_ENDPOINT");
        //    var applicationName = Environment.GetEnvironmentVariable("ROCKLIB_TRACER_SERVICE_NAME");
        //    var applicationId = Environment.GetEnvironmentVariable("ROCKLIB_TRACER_APPLICATION_ID");

        //    return new TracerConfig
        //    {
        //        ApplicationId = applicationId,
        //        ServiceName = applicationName,
        //        ServiceEndpoint = serviceEndpoint
        //    };
        //}
    }
}
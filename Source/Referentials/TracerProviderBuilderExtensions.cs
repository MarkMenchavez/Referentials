namespace Referentials;

using System;
using System.Diagnostics.CodeAnalysis;
using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Referentials.Constants;

[ExcludeFromCodeCoverage]
public static class TracerProviderBuilderExtensions
{
    public static TracerProviderBuilder AddCustomTracing(
        this TracerProviderBuilder builder,
        IWebHostEnvironment webHostEnvironment)
    {
        ArgumentNullException.ThrowIfNull(webHostEnvironment);

        return builder
            .SetResourceBuilder(GetResourceBuilder(webHostEnvironment))
            .AddAspNetCoreInstrumentation(
                options =>
                {
                    options.EnrichWithHttpRequest = (activity, request) =>
                    {
                        activity.AddTag(OpenTelemetryAttributeName.Http.ClientIP, request.HttpContext.Connection.RemoteIpAddress);
                        activity.AddTag(OpenTelemetryAttributeName.Http.RequestContentLength, request.ContentLength);
                        activity.AddTag(OpenTelemetryAttributeName.Http.RequestContentType, request.ContentType);

                        var user = request.HttpContext.User;
                        if (user.Identity?.Name is not null)
                        {
                            activity.AddTag(OpenTelemetryAttributeName.EndUser.Id, user.Identity.Name);
                            activity.AddTag(OpenTelemetryAttributeName.EndUser.Scope, string.Join(',', user.Claims.Select(x => x.Value)));
                        }
                    };

                    options.EnrichWithHttpResponse = (activity, response) =>
                    {
                        activity.AddTag(OpenTelemetryAttributeName.Http.ResponseContentLength, response.ContentLength);
                        activity.AddTag(OpenTelemetryAttributeName.Http.ResponseContentType, response.ContentType);
                    };

                    options.RecordException = true;
                })
            .AddConsoleExporter(
                options => options.Targets = ConsoleExporterOutputTargets.Console | ConsoleExporterOutputTargets.Debug);
    }

    public static TracerProviderBuilder AddIf(
        this TracerProviderBuilder builder,
        bool condition,
        Func<TracerProviderBuilder, TracerProviderBuilder> action)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(action);

        if (condition)
        {
            builder = action(builder);
        }

        return builder;
    }

    private static ResourceBuilder GetResourceBuilder(IWebHostEnvironment webHostEnvironment) =>
        ResourceBuilder
            .CreateEmpty()
            .AddService(
                webHostEnvironment.ApplicationName,
                serviceVersion: AssemblyInformation.Current.Version)
            .AddAttributes(
                new KeyValuePair<string, object>[]
                {
                    new(OpenTelemetryAttributeName.Deployment.Environment, webHostEnvironment.EnvironmentName),
                    new(OpenTelemetryAttributeName.Host.Name, Environment.MachineName),
                })
            .AddEnvironmentVariableDetector();
}

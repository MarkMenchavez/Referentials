namespace Referentials.ConfigureOptions;

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Options;
using Referentials.Constants;

/// <summary>
/// Configures cross-origin resource sharing (CORS) policies.
/// See https://docs.asp.net/en/latest/security/cors.html.
/// </summary>
[ExcludeFromCodeCoverage]
public class ConfigureCorsOptions : IConfigureOptions<CorsOptions>
{
    public void Configure(CorsOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        // Create named CORS policies here which you can consume using application.UseCors("PolicyName")
        // or a [EnableCors("PolicyName")] attribute on your controller or action.
        options.AddPolicy(
            CorsPolicyName.AllowAny,
            x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
    }
}

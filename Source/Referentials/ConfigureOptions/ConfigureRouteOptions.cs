namespace Referentials.ConfigureOptions;

using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Options;

/// <summary>
/// Configures custom routing settings which determines how URL's are generated.
/// </summary>
[ExcludeFromCodeCoverage]
public class ConfigureRouteOptions : IConfigureOptions<RouteOptions>
{
    public void Configure(RouteOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        options.LowercaseUrls = true;
    }
}

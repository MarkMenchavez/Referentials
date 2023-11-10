namespace Referentials.ConfigureOptions;

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Options;

[ExcludeFromCodeCoverage]
public class ConfigureApiVersioningOptions :
    IConfigureOptions<ApiVersioningOptions>,
    IConfigureOptions<ApiExplorerOptions>
{
    public void Configure(ApiVersioningOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
    }

    public void Configure(ApiExplorerOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        // Version format: 'v'major[.minor][-status]
        options.GroupNameFormat = "'v'VVV";
    }
}

namespace Referentials.ConfigureOptions;

using System.Diagnostics.CodeAnalysis;
using Boxed.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Options;
using Referentials.Constants;
using Referentials.Options;

/// <summary>
/// Configures the static files middleware to add the Cache-Control and Pragma HTTP headers. The cache duration is
/// controlled from configuration.
/// See http://andrewlock.net/adding-cache-control-headers-to-static-files-in-asp-net-core/.
/// </summary>
[ExcludeFromCodeCoverage]
public class ConfigureStaticFileOptions : IConfigureOptions<StaticFileOptions>
{
    private readonly CacheProfile? cacheProfile;

    public ConfigureStaticFileOptions(CacheProfileOptions cacheProfileOptions) =>
        this.cacheProfile = cacheProfileOptions
            .Where(x => string.Equals(x.Key, CacheProfileName.StaticFiles, StringComparison.Ordinal))
            .Select(x => x.Value)
            .SingleOrDefault();

    public void Configure(StaticFileOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        options.OnPrepareResponse = this.OnPrepareResponse;
    }

    /// <summary>
    /// Adds the Cache-Control and Pragma HTTP headers. The cache duration is controlled from configuration.
    /// See http://andrewlock.net/adding-cache-control-headers-to-static-files-in-asp-net-core/.
    /// </summary>
    private void OnPrepareResponse(StaticFileResponseContext context)
    {
        if (this.cacheProfile is not null)
        {
            context.Context.ApplyCacheProfile(this.cacheProfile);
        }
    }
}

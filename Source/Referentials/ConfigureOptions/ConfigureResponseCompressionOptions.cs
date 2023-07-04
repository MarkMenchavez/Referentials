namespace Referentials.ConfigureOptions;

using System.Diagnostics.CodeAnalysis;
using System.IO.Compression;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Options;
using Referentials.Options;

/// <summary>
/// Configures dynamic GZIP and Brotli response compression. This is turned off for HTTPS requests by default to avoid
/// the BREACH security vulnerability.
/// </summary>
[ExcludeFromCodeCoverage]
public class ConfigureResponseCompressionOptions :
    IConfigureOptions<ResponseCompressionOptions>,
    IConfigureOptions<BrotliCompressionProviderOptions>,
    IConfigureOptions<GzipCompressionProviderOptions>
{
    private readonly CompressionOptions compressionOptions;

    public ConfigureResponseCompressionOptions(CompressionOptions compressionOptions) =>
        this.compressionOptions = compressionOptions;

    public void Configure(ResponseCompressionOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        // Add additional MIME types (other than the built in defaults) to enable GZIP compression for.
        var customMimeTypes = this.compressionOptions?.MimeTypes ?? Enumerable.Empty<string>();
        options.MimeTypes = customMimeTypes.Concat(ResponseCompressionDefaults.MimeTypes);

        options.Providers.Add<BrotliCompressionProvider>();
        options.Providers.Add<GzipCompressionProvider>();
    }

    public void Configure(BrotliCompressionProviderOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        options.Level = CompressionLevel.Optimal;
    }

    public void Configure(GzipCompressionProviderOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        options.Level = CompressionLevel.Optimal;
    }
}

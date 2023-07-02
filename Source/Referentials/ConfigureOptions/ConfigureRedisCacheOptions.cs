namespace Referentials.ConfigureOptions;

using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Options;
using Referentials.Options;

/// <summary>
/// Configures Redis based distributed caching for the application.
/// </summary>
public class ConfigureRedisCacheOptions : IConfigureOptions<RedisCacheOptions>
{
    private readonly RedisOptions redisOptions;

    public ConfigureRedisCacheOptions(RedisOptions redisOptions) =>
        this.redisOptions = redisOptions;

    public void Configure(RedisCacheOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        options.ConfigurationOptions = this.redisOptions.ConfigurationOptions;
    }
}

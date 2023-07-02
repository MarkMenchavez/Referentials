namespace Referentials.Options;

using System.Runtime.Serialization;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// The caching options for the application.
/// </summary>
[Serializable]
#pragma warning disable CA1710 // Identifiers should have correct suffix
public class CacheProfileOptions : Dictionary<string, CacheProfile>
#pragma warning restore CA1710 // Identifiers should have correct suffix
{
    public CacheProfileOptions()
        : base()
    {
    }

    protected CacheProfileOptions(SerializationInfo serializationInfo, StreamingContext streamingContext)
        : base(serializationInfo, streamingContext)
    {
    }
}

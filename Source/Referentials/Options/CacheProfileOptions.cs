namespace Referentials.Options;

using System.Runtime.Serialization;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// The caching options for the application.
/// </summary>
[Serializable]
public class CacheProfileOptions : Dictionary<string, CacheProfile>
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

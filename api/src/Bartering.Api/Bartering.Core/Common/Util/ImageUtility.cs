namespace Bartering.Core.Common.Util;

internal static class ImageUtility
{
    /// <summary>
    /// Constructs a data URL with data encoded as a base64 string.
    /// </summary>
    /// <param name="data">The data part of the data URL.</param>
    /// <param name="mediaType">A MIME type that indicates the type of the data.</param>
    /// <param name="base64">Whether to specify 'base64' in the data URL.</param>
    /// <returns>A data URL in the format 'data:[&lt;media-type&gt;][;base64],&lt;data&gt;'.</returns>
    /// <seealso href="https://developer.mozilla.org/en-US/docs/Web/URI/Reference/Schemes/data"/>
    public static string ConstructDataUrl(string data, string mediaType = "image/jpeg", bool base64 = true)
    {
        var base64Specification = base64 ? ";base64" : "";

        return $"data:{mediaType}{base64Specification},{data}";
    }

    /// <inheritdoc cref="ConstructDataUrl(string,string,bool)"/>
    /// <summary>
    /// Constructs a data URL with data from a byte array as an equivalent base64 string.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="data"/> is null.</exception>
    public static string ConstructDataUrl(byte[] data, string mediaType = "image/jpeg", bool base64 = true)
    {
        return ConstructDataUrl(Convert.ToBase64String(data), mediaType, base64);
    }
}

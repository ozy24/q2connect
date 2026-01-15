namespace Q2Connect.Core.Protocol;

/// <summary>
/// Provides utilities for handling Quake II Out-of-Band (OOB) packet headers.
/// OOB packets are prefixed with 4 bytes: 0xFF 0xFF 0xFF 0xFF.
/// </summary>
public static class PacketHeader
{
    /// <summary>
    /// The standard OOB header bytes: 0xFF 0xFF 0xFF 0xFF
    /// </summary>
    public static readonly byte[] OobHeader = { 0xFF, 0xFF, 0xFF, 0xFF };

    /// <summary>
    /// Prepends the OOB header to packet data.
    /// </summary>
    /// <param name="data">The packet data to prepend the header to</param>
    /// <returns>A new byte array with the OOB header followed by the data</returns>
    public static byte[] PrependOobHeader(byte[] data)
    {
        var result = new byte[data.Length + 4];
        Array.Copy(OobHeader, 0, result, 0, 4);
        Array.Copy(data, 0, result, 4, data.Length);
        return result;
    }

    /// <summary>
    /// Checks if the data starts with an OOB header.
    /// </summary>
    /// <param name="data">The data to check</param>
    /// <returns>True if the data starts with the OOB header, false otherwise</returns>
    public static bool HasOobHeader(byte[] data)
    {
        if (data.Length < 4) return false;
        return data[0] == 0xFF && data[1] == 0xFF && data[2] == 0xFF && data[3] == 0xFF;
    }

    /// <summary>
    /// Removes the OOB header from packet data if present.
    /// </summary>
    /// <param name="data">The data to remove the header from</param>
    /// <returns>The data without the OOB header, or the original data if no header was present</returns>
    public static byte[] RemoveOobHeader(byte[] data)
    {
        if (!HasOobHeader(data)) return data;
        var result = new byte[data.Length - 4];
        Array.Copy(data, 4, result, 0, result.Length);
        return result;
    }
}





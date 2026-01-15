using System;
using System.Net;

namespace Q2Connect.Core.Protocol;

/// <summary>
/// Provides utilities for reading binary data in Quake II network protocol format.
/// The protocol uses big-endian byte order for multi-byte values.
/// </summary>
public static class ByteReader
{
    /// <summary>
    /// Reads a 16-bit unsigned integer in big-endian byte order.
    /// </summary>
    /// <param name="data">The byte array to read from</param>
    /// <param name="offset">The offset into the array to start reading</param>
    /// <returns>The 16-bit unsigned integer value</returns>
    /// <exception cref="ArgumentNullException">Thrown when data is null</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when offset is out of bounds</exception>
    public static ushort ReadBigEndianUInt16(byte[] data, int offset)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));
        if (offset < 0 || offset + 2 > data.Length)
            throw new ArgumentOutOfRangeException(nameof(offset), "Offset is out of bounds");

        return (ushort)((data[offset] << 8) | data[offset + 1]);
    }

    /// <summary>
    /// Parses a server address from binary data.
    /// Format: 4 bytes for IP address (IPv4) followed by 2 bytes for port (big-endian).
    /// </summary>
    /// <param name="data">The byte array containing the server address</param>
    /// <param name="offset">The offset into the array to start parsing</param>
    /// <returns>An IPEndPoint representing the server address, or null if insufficient data</returns>
    /// <exception cref="ArgumentNullException">Thrown when data is null</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when offset is negative</exception>
    public static IPEndPoint? ParseServerAddress(byte[] data, int offset)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));
        if (offset < 0)
            throw new ArgumentOutOfRangeException(nameof(offset), "Offset cannot be negative");
        if (data.Length < offset + 6)
            return null;

        var ipBytes = new byte[4];
        Array.Copy(data, offset, ipBytes, 0, 4);
        var port = ReadBigEndianUInt16(data, offset + 4);
        
        return new IPEndPoint(new IPAddress(ipBytes), port);
    }
}




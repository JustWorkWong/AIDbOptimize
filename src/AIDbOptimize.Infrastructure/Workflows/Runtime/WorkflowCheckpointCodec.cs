using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;

namespace AIDbOptimize.Infrastructure.Workflows.Runtime;

internal static class WorkflowCheckpointCodec
{
    public static EncodedCheckpoint Encode(string rawJson)
    {
        var bytes = Encoding.UTF8.GetBytes(rawJson);
        using var output = new MemoryStream();
        using (var gzip = new GZipStream(output, CompressionLevel.SmallestSize, leaveOpen: true))
        {
            gzip.Write(bytes, 0, bytes.Length);
        }

        var compressed = output.ToArray();
        var hash = Convert.ToHexString(SHA256.HashData(bytes)).ToLowerInvariant();
        return new EncodedCheckpoint(
            Convert.ToBase64String(compressed),
            "gzip+base64",
            hash,
            bytes.LongLength);
    }

    public static string Decode(string payloadCompressed, string payloadEncoding)
    {
        if (!string.Equals(payloadEncoding, "gzip+base64", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException($"Unsupported checkpoint encoding: {payloadEncoding}");
        }

        var compressed = Convert.FromBase64String(payloadCompressed);
        using var input = new MemoryStream(compressed);
        using var gzip = new GZipStream(input, CompressionMode.Decompress);
        using var output = new MemoryStream();
        gzip.CopyTo(output);
        return Encoding.UTF8.GetString(output.ToArray());
    }
}

internal sealed record EncodedCheckpoint(
    string PayloadCompressed,
    string PayloadEncoding,
    string PayloadSha256,
    long PayloadSizeBytes);

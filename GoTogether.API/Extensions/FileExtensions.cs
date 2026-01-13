namespace GoTogether.API.Extensions;

public static class FileExtensions
{
    public static readonly string[] AllowedImageTypes = ["image/jpeg", "image/png", "image/webp"];

    private static readonly Dictionary<string, List<byte[]>> _fileSignatures = new()
    {
        { "image/jpeg", [ [0xFF, 0xD8, 0xFF] ] },
        { "image/png",  [ [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A] ] },
        { "image/webp", [ [0x52, 0x49, 0x46, 0x46] ] },
    };

    public static async Task<bool> IsValidImageAsync(this IFormFile? file)
    {
        if (file == null || file.Length == 0 || !AllowedImageTypes.Contains(file.ContentType) || file.Length > 5 * 1024 * 1024) 
            return false;

        byte[] headerBytes = new byte[12];

        try
        {
            using var stream = file.OpenReadStream();
            await stream.ReadExactlyAsync(headerBytes.AsMemory(0, 12));

            if (stream.CanSeek) 
                stream.Position = 0;
        }
        catch (EndOfStreamException)
        {
            return false;
        }

        if (_fileSignatures.TryGetValue(file.ContentType, out var signatures))
            if (signatures.Any(sig => headerBytes.Take(sig.Length).SequenceEqual(sig)))
            {
                // WebP specific check (must have 'WEBP' at offset 8)
                if (file.ContentType == "image/webp")
                {
                    string webpCheck = System.Text.Encoding.ASCII.GetString(headerBytes, 8, 4);
                    return webpCheck == "WEBP";
                }
                return true;
            }

        return false;
    }
}
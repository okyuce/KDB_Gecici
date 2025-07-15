using Csb.YerindeDonusum.Domain.Addons.FileAddons;

namespace Csb.YerindeDonusum.Application.CustomAddons;

internal static class FileTypeVerifier
{
    private static FileTypeVerifyResult Unknown = new FileTypeVerifyResult
    {
        Name = "Bilinmeyen",
        Description = "Bilinmeye/geçersiz dosya tipi",
        IsVerified = false
    };

    private static IEnumerable<FileType> Types { get; set; }

    static FileTypeVerifier()
    {
        Types = new List<FileType>
        {
            new PdfFile(),
            new MsOfficeOldFiles(),
            new MsOfficeNewFiles(),
            new JpegFile(),
            new PngFile(),
            //new GifFile(),
            //new ZipFile(),
            //new Mp3File()
        }
        .OrderByDescending(x => x.SignatureLength)
            .ToList();
    }

    public static FileTypeVerifyResult Verify(string path)
    {
        FileTypeVerifyResult? result = null;

        if (!string.IsNullOrWhiteSpace(path))
        {
            using var file = File.OpenRead(path);

            foreach (var fileType in Types)
            {
                result = fileType.Verify(file);

                if (result.IsVerified)
                    break;
            }
        }

        return result?.IsVerified == true
               ? result
               : Unknown;
    }

    public static FileTypeVerifyResult Verify(byte[] bytes)
    {
        FileTypeVerifyResult? result = null;

        if (bytes != null && bytes.Length > 0)
        {
            foreach (var fileType in Types)
            {
                // result = fileType.Verify(bytes);
                result = fileType.Verify(bytes.Take(fileType.SignatureLength).ToArray());

                if (result.IsVerified)
                    break;
            }
        }

        return result?.IsVerified == true
               ? result
               : Unknown;
    }

    public static bool VerifyFileExtension(string fileName)
    {
        var result = false;

        if (!string.IsNullOrWhiteSpace(fileName))
        {
            var isTheFileExtensionAllowed = Types.Any(x => x.Extensions.Contains(Path.GetExtension(fileName).ToLowerInvariant()));

            if (isTheFileExtensionAllowed)
                result = true;
            else
                result = false;
        }

        return result;
    }

    public static bool VerifyFileExtensionByCustomTypes(string fileName, List<FileType> fileTypes)
    {
        if (!string.IsNullOrWhiteSpace(fileName))
            return fileTypes.Any(x => x.Extensions.Contains(Path.GetExtension(fileName).ToLowerInvariant().TrimStart('.')));
        return false;
    }
}
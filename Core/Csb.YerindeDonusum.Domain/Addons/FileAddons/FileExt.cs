namespace Csb.YerindeDonusum.Domain.Addons.FileAddons;

public sealed class JpegFile : FileType
{
    public JpegFile()
    {
        Name = "JPEG";
        Description = "JPEG IMAGE";
        FileExtension = ".jpg";
        AddExtensions("jpeg", "jpg");
        AddSignatures(
            new byte[] { 0xFF, 0xD8 }
        );
    }
}

//public sealed class JpgFile : FileType
//{
//    public JpgFile()
//    {
//        Name = "JPG";
//        Description = "JPG Image";
//        AddExtensions("jpg");
//        AddSignatures(
//            new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
//            new byte[] { 0xFF, 0xD8, 0xFF, 0xE1 },
//            new byte[] { 0xFF, 0xD8, 0xFF, 0xE8 }
//        );
//    }
//}

public sealed class Mp3File : FileType
{
    public Mp3File()
    {
        Name = "MP3";
        Description = "MP3 Audio File";
        AddExtensions("mp3");
        AddSignatures(
            new byte[] { 0x49, 0x44, 0x33 }
        );
    }
}

public sealed class PngFile : FileType
{
    public PngFile()
    {
        Name = "PNG";
        Description = "PNG Image";
        FileExtension = ".png";
        AddExtensions("png");
        AddSignatures(
            new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }
        );
    }
}

public sealed class GifFile : FileType
{
    public GifFile()
    {
        Name = "GIF";
        Description = "GIF Image";
        FileExtension = ".gif";
        AddExtensions("gif");
        AddSignatures(
            new byte[] { 0x47, 0x49, 0x46, 0x38 }
        );
    }
}

public sealed class ZipFile : FileType
{
    public ZipFile()
    {
        Name = "ZIP";
        Description = "ZIP File";
        FileExtension = ".zip";
        AddExtensions("zip");
        AddSignatures(
            new byte[] { 0x50, 0x4B, 0x03, 0x04 },
            new byte[] { 0x50, 0x4B, 0x4C, 0x49, 0x54, 0x45 },
            new byte[] { 0x50, 0x4B, 0x53, 0x70, 0x58 },
            new byte[] { 0x50, 0x4B, 0x05, 0x06 },
            new byte[] { 0x50, 0x4B, 0x07, 0x08 },
            new byte[] { 0x57, 0x69, 0x6E, 0x5A, 0x69, 0x70 }
        );
    }
}

public sealed class PdfFile : FileType
{
    public PdfFile()
    {
        Name = "PDF";
        Description = "PDF File";
        AddExtensions("pdf");
        FileExtension = ".pdf";
        AddSignatures(
            new byte[] { 0x25, 0x50, 0x44, 0x46, 0x2D, }
        );
    }
}

public sealed class MsOfficeOldFiles : FileType
{
    public MsOfficeOldFiles()
    {
        Name = "Ms Office";
        Description = "Ms Office";
        AddExtensions("doc", "xls", "ppt");
        AddSignatures(
            new byte[] { 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1 }
        );
    }
}

public sealed class MsOfficeNewFiles : FileType
{
    public MsOfficeNewFiles()
    {
        Name = "Ms Office X";
        Description = "Ms Office X";
        AddExtensions("docx", "xlsx", "pptx");
        AddSignatures(
            new byte[] { 0x50, 0x4B, 0x03, 0x04, 0x14 }
        );
    }
}

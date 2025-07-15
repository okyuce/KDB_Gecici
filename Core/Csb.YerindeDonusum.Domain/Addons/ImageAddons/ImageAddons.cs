using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Csb.YerindeDonusum.Domain.Addons.ImageAddons;

public static class ImageAddons
{
    public static byte[] ResizeImage(string filePath, string localPath, int appSettingsMaxWith, int appSettingsMaxQuality, int maxWidth = 1920, int maxQuality = 50)
    {
        if (maxWidth <= 0 || maxWidth > appSettingsMaxWith)
            maxWidth = appSettingsMaxWith;

        if (maxQuality <= 0 || maxQuality > appSettingsMaxQuality)
            maxQuality = appSettingsMaxQuality;

        using Image<Rgba32> image = Image.Load<Rgba32>(filePath);

        if (image.Size.Width >= maxWidth)
            image.Mutate(ctx => ctx.Resize(maxWidth, 0));

        image.Save(localPath, new JpegEncoder { Quality = maxQuality });

        using var fs = new FileStream(localPath, FileMode.OpenOrCreate);

        using var ms = new MemoryStream();

        image.Save(ms, new JpegEncoder { Quality = maxQuality });

        return ms.ToArray();
    }
}
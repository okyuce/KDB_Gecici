using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing;
using System.Text;

namespace Csb.YerindeDonusum.WebApp.Utilities.Helpers
{
    public class CapthaHelper
    {
        const string Letters = "2346789ABCDEFGHJKLMNPRTUVYZ";

        public static string GenerateRandomCode(int length = 5)
        {
            Random rand = new Random();
            int maxRand = Letters.Length - 1;

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                int index = rand.Next(maxRand);
                sb.Append(Letters[index]);
            }

            return sb.ToString();
        }

        public static Bitmap GenerateImage(string captchaText = null, int width = 150, int height = 40)
        {
            //First declare a bitmap and declare graphic from this bitmap
            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(bitmap);
            //And create a rectangle to delegete this image graphic 
            Rectangle rect = new Rectangle(0, 0, width, height);
            //And create a brush to make some drawings
            HatchBrush hatchBrush = new HatchBrush(HatchStyle.DottedGrid, Color.Aqua, Color.White);
            g.FillRectangle(hatchBrush, rect);

            //here we make the text configurations
            GraphicsPath graphicPath = new GraphicsPath();
            //add this string to image with the rectangle delegate
            graphicPath.AddString(captchaText ?? GenerateRandomCode(), FontFamily.GenericMonospace, (int)FontStyle.Bold, height, rect, null);
            //And the brush that you will write the text
            hatchBrush = new HatchBrush(HatchStyle.Percent20, Color.Black, Color.Red);
            g.FillPath(hatchBrush, graphicPath);
            //We are adding the dots to the image

            Random rnd = new Random();

            for (int i = 0; i < (int)(rect.Width * rect.Height / 50F); i++)
            {
                int x = rnd.Next(width);
                int y = rnd.Next(height);
                int w = rnd.Next(6);
                int h = rnd.Next(6);
                g.FillEllipse(hatchBrush, x, y, w, h);
            }
            //Remove all of variables from the memory to save resource
            hatchBrush.Dispose();
            g.Dispose();

            return bitmap;
        }
    }
}
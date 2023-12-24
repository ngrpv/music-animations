using System.Drawing;
using Kernel.Domain.Settings;
using Kernel.Domain.Utils;

namespace Kernel.Domain
{
    public class Constant : Renderable<Constant, ConstantSettings>
    {
        public override DirectBitmap GetBitmap(DirectBitmap bitmap, Func<Color, Color, Color> action, int _ = 0)
        {
            for (var i = 0; i < bitmap.Width; i++)
            {
                for (var j = 0; j < bitmap.Height; j++)
                {
                    var newColor = Settings.Color;
                    var oldColor = bitmap.GetPixel(i, j);
                    bitmap.SetPixel(i, j, action(newColor, oldColor));
                }
            }

            return bitmap;
        }

        public Constant(int width, int height) : base(width, height)
        {
        }
    }
}
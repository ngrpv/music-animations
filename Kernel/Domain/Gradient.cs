using System.Drawing;
using System.Numerics;
using Kernel.Domain.Settings;
using Kernel.Domain.Utils;

namespace Kernel.Domain
{
    public class Gradient : Renderable<Gradient, GradientSettings>
    {
        public override DirectBitmap GetBitmap(DirectBitmap bmp, Func<Color, Color, Color> action, int i =0)
        {
            for (var x = 0; x < Width; x++)
            for (var y = 0; y < Height; y++)
            {
                var dx = x / 256d;
                var dy = y / 256d;
                var complex = new Complex(dx, dy);
                var t = (2 * complex.Phase / Math.PI).ToInt();
                var newColor = Color.FromArgb(t, t, t);
                var oldColor = bmp.GetPixel(x, y);
                
                bmp.SetPixel(x, y, action(newColor, oldColor));
            }

            return bmp;
        }

        public Gradient(int width, int height) : base(width, height)
        {
        }
    }
}
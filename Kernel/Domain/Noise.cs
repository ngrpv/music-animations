using System.Drawing;
using Kernel.Domain.Settings;
using Kernel.Domain.Utils;

namespace Kernel.Domain;

public class Noise : Renderable<Noise, NoiseSettings>
{
    public override DirectBitmap GetBitmap(DirectBitmap bitmap, Func<Color, Color, Color> action, int i = 0)
    {
        var r = Settings.Random;
        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                var R = r.Next(Settings.Start, Settings.End);
                var G = r.Next(Settings.Start, Settings.End);
                var B = r.Next(Settings.Start, Settings.End);
                var newColor = Color.FromArgb(R, G, B);
                var oldColor = bitmap.GetPixel(x, y);
                bitmap.SetPixel(x, y, action(oldColor, newColor));
            }
        }

        return bitmap;
    }

    public Noise(int width, int height) : base(width, height)
    {
    }
}
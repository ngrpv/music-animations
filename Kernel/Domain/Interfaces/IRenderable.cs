using System.Drawing;
using Kernel.Domain.Utils;

namespace Kernel.Domain.Interfaces;

public interface IRenderable
{
    DirectBitmap GetBitmap();
    DirectBitmap GetBitmap(DirectBitmap bitmap = null, Func<Color, Color, Color> action = null, int i = 0);
}
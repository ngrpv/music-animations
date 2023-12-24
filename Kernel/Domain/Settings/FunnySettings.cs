using System.Drawing;

namespace Kernel.Domain.Settings;

public struct FunnySettings
{
    public readonly IReadOnlyList<double[]> Fft;
    public Color Color { get; set; }

    public FunnySettings(List<double[]> fft, Color color)
    {
        Fft = fft;
        Color = color;
    }
}
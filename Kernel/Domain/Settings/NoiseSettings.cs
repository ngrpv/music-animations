namespace Kernel.Domain.Settings;

public readonly struct NoiseSettings
{
    public NoiseSettings(int start, int end, Random random)
    {
        Start = start;
        End = end;
        Random = random;
    }

    public readonly int Start;
    public readonly int End;
    public readonly Random Random;
}
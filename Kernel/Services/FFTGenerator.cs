using Kernel.Services.Interfaces;
using Spectrogram;

namespace Kernel.Services;

public class FftGenerator
{
    private readonly IWavAudioProvider provider;

    public FftGenerator(IWavAudioProvider provider)
    {
        this.provider = provider;
    }
    
    public List<double[]> GetFft(string filename)
    {
        var (audio, sampleRate) = provider.ReadWav(filename);
        var sg = new SpectrogramGenerator(sampleRate, fftSize: 4096, stepSize: 200, maxFreq: 2000);
        sg.Add(audio);
        var fft = sg.GetFFTs();

        return fft;
    }
}
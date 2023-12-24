using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Kernel.Domain.Settings;
using Kernel.Domain;
using Kernel.Domain.Utils;
using Kernel.Services;
using Kernel.Services.Interfaces;

namespace WpfApp2;

public class VideoGenerator
{
    private readonly int width;
    private readonly int height;

    private readonly string tempFilesPath;
    private readonly List<double[]> fft;
    private ImageBase imageBase;
    private DirectBitmap bmp;

    public int FramesCount => fft.Count;

    private VideoGenerator(IWavAudioProvider audioProvider, int width, int height, string audioPath,
        string tempFilesPath)
    {
        this.tempFilesPath = tempFilesPath;
        if (!Directory.Exists(this.tempFilesPath))
            Directory.CreateDirectory(this.tempFilesPath);

        this.width = width;
        this.height = height;

        fft = new FftGenerator(audioProvider).GetFft(audioPath);
        bmp = new DirectBitmap(width, height);
        ClearTempDirectory();
    }

    public static VideoGenerator Funny(IWavAudioProvider audioProvider, int width, int height, string audioPath,
        string tempFilesPath)
    {
        return new VideoGenerator(audioProvider, width, height, audioPath, tempFilesPath)
            .WithFunny();
    }

    public static VideoGenerator Planets(IWavAudioProvider audioProvider, int width, int height, string audioPath,
        string tempFilesPath)
    {
        return new VideoGenerator(audioProvider, width, height, audioPath, tempFilesPath)
            .WithPlanets();
    }

    public static VideoGenerator ThreeD(IWavAudioProvider audioProvider, int width, int height, string audioPath,
        string tempFilesPath)
    {
        return new VideoGenerator(audioProvider, width, height, audioPath, tempFilesPath)
            .WithThreeD();
    }

    public static VideoGenerator PlanetsAndNoise(IWavAudioProvider audioProvider, int width, int height,
        string audioPath,
        string tempFilesPath)
    {
        return new VideoGenerator(audioProvider, width, height, audioPath, tempFilesPath)
            .WithPlanetsAndNoise();
    }
    public static VideoGenerator FunnyAndNoise(IWavAudioProvider audioProvider, int width, int height,
        string audioPath,
        string tempFilesPath)
    {
        return new VideoGenerator(audioProvider, width, height, audioPath, tempFilesPath)
            .WithFunnyAndNoise();
    }

    private VideoGenerator WithPlanets()
    {
        imageBase = PlanetsConfig();
        return this;
    }

    private VideoGenerator WithFunny()
    {
        imageBase = FunnyConfig();
        return this;
    }

    private VideoGenerator WithFunnyAndNoise()
    {
        imageBase =  ImageBase.Create()
            .Config(new ImageSettings(width, height))
            .Add<Funny>(f => f.Config(new FunnySettings(fft, Color.Chartreuse)))
            .Add<Noise>(f => f.Config(new NoiseSettings(1, 20, new Random())));
        return this;
    }


    private VideoGenerator WithThreeD()
    {
        imageBase = ThreeDConfig();
        return this;
    }

    private VideoGenerator WithPlanetsAndNoise()
    {
        imageBase = PlanetsAndNoiseConfig();
        return this;
    }

    public string GetFrame(int i)
    {
        var fileName = $@"{tempFilesPath}\{i}.bmp";

        if (File.Exists(fileName))
            return fileName;

        bmp = imageBase.GetBitmap(bmp, i);
        bmp.Bitmap.Save(fileName);
        bmp.Reset();

        return fileName;
    }

    private void ClearTempDirectory()
    {
        if (!Directory.Exists(tempFilesPath)) return;
        Directory.Delete(tempFilesPath, true);
        Directory.CreateDirectory(tempFilesPath);
    }

    private ImageBase FunnyConfig()
    {
        return ImageBase.Create()
            .Config(new ImageSettings(width, height))
            .Add<Funny>(f => f.Config(new FunnySettings(fft, Color.Green)));
    }

    private ImageBase ThreeDConfig()
    {
        return ImageBase.Create()
            .Config(new ImageSettings(width, height))
            .Add<ThreeD>(f => f.Config(new ThreeDSettings(fft)));
    }

    private ImageBase PlanetsConfig()
    {
        return ImageBase.Create()
            .Config(new ImageSettings(width, height))
            .Add<Planets>(f => f.Config(new PlanetsSettings(20, 10, 100, Brushes.Chartreuse, new Random())));
    }

    private ImageBase PlanetsAndNoiseConfig()
    {
        return ImageBase.Create()
            .Config(new ImageSettings(width, height))
            .Add<Planets>(f => f.Config(new PlanetsSettings(20, 10, 100, Brushes.Chartreuse, new Random())))
            .Add<Noise>(f => f.Config(new NoiseSettings(1, 233, new Random())));
    }
}
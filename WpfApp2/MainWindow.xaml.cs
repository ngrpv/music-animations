using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Kernel.Services;
using Microsoft.Win32;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MediaPlayer mediaPlayer = new();
        private static bool isStarted;
        private static bool videoInitialized;
        private CancellationTokenSource cts = new();

        private const string TempFiles =
            @"C:\Users\Garipov\RiderProjects\ProjectNice\WpfApp2\bin\Debug\net6.0-windows\temp_img";

        private readonly object lockObj = new object();

        public MainWindow()
        {
            InitializeComponent();
            PlayBtn.Click += (_, _) =>
            {
                while (!videoInitialized)
                {
                    Thread.Sleep(100);
                }

                isStarted = true;
                mediaPlayer.Play();
            };
            PauseBtn.Click += (_, _) =>
            {
                isStarted = false;
                mediaPlayer.Pause();
            };
        }

        private void StartImageUpdater(string audioPath, CancellationToken ct)
        {
            var generator = VideoGenerator.FunnyAndNoise(new WavAudioMonoProvider(16000), 600, 400, audioPath, TempFiles);
            var count = generator.FramesCount;
            while (true)
            {
                videoInitialized = true;
                SpinWait.SpinUntil(() => isStarted);

                var position = Dispatcher.Invoke(() => mediaPlayer.Position / mediaPlayer.NaturalDuration.TimeSpan);
                var i = (int)(position * count);
                if (i >= count || ct.IsCancellationRequested)
                    break;

                var frame = generator.GetFrame(i);
                Dispatcher.Invoke(() =>
                {
                    var img = new BitmapImage(new Uri(frame));
                    return ImageViewer1.Source = img;
                });
            }
        }


        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Audio files (*.mp3;*.wav;*.aac)|*.mp3;*.wav;*.aac"
            };
            cts.Cancel();
            cts = new CancellationTokenSource();
            if (openFileDialog.ShowDialog() == true)
            {
                mediaPlayer.Open(new Uri(openFileDialog.FileName));
                isStarted = false;
                Task.Run(() => StartImageUpdater(openFileDialog.FileName, cts.Token));
            }
        }
    }
}
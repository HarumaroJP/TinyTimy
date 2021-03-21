using System;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TinyTimy
{
    public partial class MainWindow : Window
    {
        Countdown timer;
        BitmapImage pinButtonTrue_image;
        BitmapImage pinButtonFalse_image;
        BitmapImage playButton_image;
        BitmapImage pauseButton_image;

        Settings settings;

        ColorConverter cc = new ColorConverter();
        Color lightMode
        {
            get
            {
                return (Color)cc.ConvertFrom("#FFFAFAFA");
            }
        }

        Color darkMode
        {
            get
            {
                return (Color)cc.ConvertFrom("#FF323232");
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            // ウィンドウをマウスのドラッグで移動できるようにする
            this.MouseLeftButtonDown += (sender, e) => { this.DragMove(); };

            //画像のスケーリングを滑らかにする
            RenderOptions.SetBitmapScalingMode(closeButton, BitmapScalingMode.Fant);
            RenderOptions.SetBitmapScalingMode(pinButton, BitmapScalingMode.Fant);
            RenderOptions.SetBitmapScalingMode(setButton, BitmapScalingMode.Fant);
            RenderOptions.SetBitmapScalingMode(playAndpauseButton, BitmapScalingMode.Fant);
            RenderOptions.SetBitmapScalingMode(restartButton, BitmapScalingMode.Fant);

            //初期化
            Initialize();
        }

        private void Initialize()
        {
            //画像リソースの読み込み
            pinButtonFalse_image = CreateBMP(@"/Images/pin-button-false.png");
            pinButtonTrue_image = CreateBMP(@"/Images/pin-button-true.png");
            playButton_image = CreateBMP(@"/Images/play-button.png");
            pauseButton_image = CreateBMP(@"/Images/pause-button.png");

            //TODO: 設定から読み込む
            settings.setTime = new TimeSpan(0, 0, 0, 0);
            settings.DisplayFormat = @"dd'd'':'hh':'mm':'ss";
            settings.isDarkMode = false;

            CreateTimer();
        }

        void CreateTimer()
        {
            //nullじゃなかったらとりあえず停止＆破棄
            if (timer != null)
            {
                timer.Destroy();
            }

            timer = new Countdown(settings.setTime);

            timer.Elapsed += (s, arge) =>
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    timerDisplay.Content = timer.ElapseDown.ToString(settings.DisplayFormat);

                    if (timer.ElapseDown == TimeSpan.Zero)
                    {
                        timerDisplay.Visibility = Visibility.Hidden;
                        finishDisplay.Visibility = Visibility.Visible;
                    }
                }));
            };

            timerDisplay.Content = timer.ElapseDown.ToString(settings.DisplayFormat);
        }

        void OpenSettings(object sender, RoutedEventArgs e)
        {
            SettingWindow win = new SettingWindow(settings);
            win.Closed += (s, args) =>
            {
                settings = win.settings;
                Border.Background = new SolidColorBrush(settings.isDarkMode ? darkMode : lightMode);
                timerDisplay.Foreground = new SolidColorBrush(settings.isDarkMode ? lightMode : darkMode);
                timerDisplay.Content = settings.setTime.ToString(settings.DisplayFormat);
                CreateTimer();
            };
            win.Show();
        }

        bool isFront = false;

        //最前面の状態を切り替える
        void ChangePinState(object sender, RoutedEventArgs e)
        {
            isFront = !isFront;
            pinButton.Source = isFront ? pinButtonTrue_image : pinButtonFalse_image;
            this.Topmost = isFront;
        }

        bool isPlaying = false;

        void PlayOrPause(object sender, RoutedEventArgs e)
        {
            if (timer == null) return;
            isPlaying = !isPlaying;

            if (timer.ElapseDown == TimeSpan.Zero)
            {
                timer.Destroy();

                timerDisplay.Visibility = Visibility.Hidden;
                finishDisplay.Visibility = Visibility.Visible;

                return;
            }

            if (isPlaying)
            {
                timer.Start();
            }
            else
            {
                timer.Stop();
            }

            playAndpauseButton.Source = isPlaying ? pauseButton_image : playButton_image;
        }

        void Reset(object sender, RoutedEventArgs e)
        {
            if (timer == null) return;

            isPlaying = false;
            timer.Stop();
            playAndpauseButton.Source = isPlaying ? pauseButton_image : playButton_image;

            timerDisplay.Visibility = Visibility.Visible;
            finishDisplay.Visibility = Visibility.Hidden;

            CreateTimer();
        }

        void OnClose(object sender, RoutedEventArgs e)
        {
            if (timer != null) timer.Destroy();
            Application.Current.Shutdown();
        }

        //extension
        BitmapImage CreateBMP(string uri)
        {
            BitmapImage image = new BitmapImage();

            image.BeginInit();
            image.UriSource = new Uri(uri, UriKind.RelativeOrAbsolute);
            image.EndInit();

            return image;
        }
    }

    class Countdown : System.Timers.Timer
    {
        readonly TimeSpan IntervalSpan = new TimeSpan(0, 0, 0, 1);
        public TimeSpan ElapseDown { get; private set; }

        public Countdown(TimeSpan timeSpan)
        {
            this.Elapsed += new ElapsedEventHandler(Update);
            this.Interval = IntervalSpan.TotalMilliseconds;
            this.ElapseDown = timeSpan;
        }

        void Update(object sender, ElapsedEventArgs e)
        {
            ElapseDown = ElapseDown.Subtract(IntervalSpan);
        }

        public void Destroy()
        {
            Stop();
            Dispose();
        }
    }

    public struct Settings
    {
        public TimeSpan setTime;
        public string DisplayFormat;
        public bool isDarkMode;
    }
}

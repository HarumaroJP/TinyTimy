using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TinyTimy
{
    /// <summary>
    /// SettingWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class SettingWindow : Window
    {
        public Settings settings;

        public SettingWindow(Settings settings)
        {
            this.settings = settings;
            InitializeComponent();
            displayFormat.Text = settings.DisplayFormat;
            isDarkMode.IsChecked = settings.isDarkMode;

            timer_day.Text = settings.setTime.Days.ToString();
            timer_hour.Text = settings.setTime.Hours.ToString();
            timer_minute.Text = settings.setTime.Minutes.ToString();
            timer_second.Text = settings.setTime.Seconds.ToString();
        }

        private void textBoxPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // 0-9のみ
            e.Handled = !new Regex("[0-9]").IsMatch(e.Text);
        }
        private void textBoxPrice_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            // 貼り付けを許可しない
            if (e.Command == ApplicationCommands.Paste)
            {
                e.Handled = true;
            }
        }

        private void OnSave(object sender, RoutedEventArgs e)
        {
            TimeSpan setTime = new TimeSpan(int.Parse(timer_day.Text), int.Parse(timer_hour.Text), int.Parse(timer_minute.Text), int.Parse(timer_second.Text));

            try
            {
                setTime.ToString(displayFormat.Text);
            }
            catch (FormatException)
            {
                formatWarn.Visibility = Visibility.Visible;
                return;
            }

            settings.setTime = setTime;
            settings.DisplayFormat = @displayFormat.Text;
            settings.isDarkMode = isDarkMode.IsChecked.Value;
            this.Close();
        }
    }
}

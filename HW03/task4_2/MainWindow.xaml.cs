using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;

namespace task4_2
{
    public partial class MainWindow
    {
        private const int WM_USER = 0x0400;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            HwndSource source = HwndSource.FromHwnd(new WindowInteropHelper(this).Handle);
            source.AddHook(WndProc);
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_USER)
            {
                ChangeStyles("Red", "Times New Roman", "20");
                handled = true;
            }

            return IntPtr.Zero;
        }

        public void ChangeStyles(string color, string font, string size)
        {
            try
            {
                Color textColor = (Color)ColorConverter.ConvertFromString(color);
                label.Foreground = new SolidColorBrush(textColor);
                textBox.Foreground = new SolidColorBrush(textColor);
                button.Foreground = new SolidColorBrush(textColor);

                FontFamily fontFamily = new FontFamily(font);
                label.FontFamily = fontFamily;
                textBox.FontFamily = fontFamily;
                button.FontFamily = fontFamily;

                double fontSize = double.Parse(size);
                label.FontSize = fontSize;
                textBox.FontSize = fontSize;
                button.FontSize = fontSize;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
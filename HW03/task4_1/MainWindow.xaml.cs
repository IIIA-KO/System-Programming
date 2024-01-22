using System.Runtime.InteropServices;
using System.Windows;

namespace task4_1
{
    public partial class MainWindow : Window
    {
        private IntPtr secondWindowHandle;
        const int WM_USER = 0x0400; 

        [DllImport("user32.dll")]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            secondWindowHandle = FindWindow(null, "MainWindowTask2");
            if (secondWindowHandle == IntPtr.Zero)
            {
                MessageBox.Show("Вікно не знайдено.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            SendMessage(secondWindowHandle, WM_USER, IntPtr.Zero, IntPtr.Zero);
        }
    }
}
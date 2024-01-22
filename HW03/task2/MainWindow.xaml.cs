using System.Runtime.InteropServices;
using System;
using System.Windows;
using System.Windows.Interop;

namespace task2
{
    public partial class MainWindow : Window
    {
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

        const int WM_SETTEXT = 0x000C;
        const int WM_CLOSE = 0x0010;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            // Отримати введений користувачем новий заголовок вікна
            string newTitle = NewTitleTextBox.Text;

            // Знайти вікно за ім'ям
            IntPtr hWnd = new WindowInteropHelper(this).Handle;

            if (hWnd != IntPtr.Zero)
            {
                // Визначити вибір користувача
                if (SetTitleRadioButton.IsChecked == true)
                {
                    // Відправити повідомлення про зміну заголовка вікна
                    SendMessage(hWnd, WM_SETTEXT, IntPtr.Zero, Marshal.StringToBSTR(newTitle));
                }
                else if (CloseWindowRadioButton.IsChecked == true)
                {
                    // Відправити повідомлення про закриття вікна
                    SendMessage(hWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                }
                else
                {
                    MessageBox.Show("Дія не обрана");
                }
            }
            else
            {
                MessageBox.Show("Вікно не знайдено");
            }
        }
    }
}

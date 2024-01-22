using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;

namespace task3
{
    public partial class MainWindow : Window
    {
        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool Beep(int frequency, int duration);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool MessageBeep(int uType);

        const int MB_ICONASTERISK = 0x00000040;

        private bool beepRunning;
        private int interval;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void btnStartBeep_Click(object sender, RoutedEventArgs e)
        {
            if (!beepRunning)
            {
                if (int.TryParse(txtInterval.Text, out interval) && interval > 0)
                {
                    beepRunning = true;
                    await Task.Run(() => BeepLoop());
                }
                else
                {
                    MessageBox.Show("Please enter a valid positive integer for the interval.", "Error");
                }
            }
        }

        private void btnStopBeep_Click(object sender, RoutedEventArgs e)
        {
            beepRunning = false;
        }

        private void BeepLoop()
        {
            while (beepRunning)
            {
                Beep(500, 200);
                MessageBeep(MB_ICONASTERISK);
                System.Threading.Thread.Sleep(interval);
            }
        }
    }
}

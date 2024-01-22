using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ConsoleMessageBox
{
    internal class Program
    {
        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall)]
        static extern void MessageBox(IntPtr hWd, string text, string caption, uint uType = 0x02);

        static void Main(string[] args)
        {
            IntPtr intPtr = Process.GetCurrentProcess().MainWindowHandle;

            MessageBox(intPtr, "Illia", "Name");
            MessageBox(intPtr, "Student", "Occupation");
            MessageBox(intPtr, "18", "Age");
        }
    }
}
using System.Runtime.InteropServices;

namespace ConsoleSelectDirectoryPath
{
    internal class Program
    {
        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        public static extern bool SHGetPathFromIDList(IntPtr pidl, IntPtr pszPath);

        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr SHBrowseForFolder(ref BROWSEINFO lpbi);

        [StructLayout(LayoutKind.Sequential)]
        public struct BROWSEINFO
        {
            public IntPtr hwndOwner;
            public IntPtr pidlRoot;
            public IntPtr pszDisplayName;
            public string lpszTitle;
            public uint ulFlags;
            public IntPtr lpfn;
            public IntPtr lParam;
            public int iImage;
        }

        static void Main(string[] args)
        {
            BROWSEINFO bi = new BROWSEINFO();
            bi.hwndOwner = IntPtr.Zero;
            bi.pidlRoot = IntPtr.Zero;
            bi.pszDisplayName = IntPtr.Zero;
            bi.lpszTitle = "Виберіть директорію";
            bi.ulFlags = 0x1;
            IntPtr result = SHBrowseForFolder(ref bi);

            if (result != IntPtr.Zero)
            {
                IntPtr pathPtr = Marshal.AllocHGlobal(260);
                SHGetPathFromIDList(result, pathPtr);
                string selectedDirectory = Marshal.PtrToStringAuto(pathPtr);
                Marshal.FreeHGlobal(pathPtr);

                Console.WriteLine("Обрана директорія: " + selectedDirectory);
            }
        }
    }
}
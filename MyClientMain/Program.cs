using MyMain;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace MyClientMain
{

    internal static class Program
    {
        [DllImport("User32.dll", SetLastError = true)]
        private static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);
        [DllImport("User32.dll")]
        public static extern Int32 SetForegroundWindow(int hWnd);
        // 걍 Assembly GUID로 
        private const string MutexName = "910fffc3-166d-4d7b-bc62-47252899bc43";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Set this variable to false if you do not want to request 
            // initial ownership of the named mutex.
            bool createdNew;
            _ = new Mutex(true, MutexName,
                                out createdNew);
            if (createdNew)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
            else
            {
                Process current = Process.GetCurrentProcess();

                foreach (Process process in Process.GetProcessesByName(current.ProcessName))
                {
                    if (process.Id != current.Id)
                    {
                        SwitchToThisWindow(process.MainWindowHandle, true);
                        break;
                    }
                }
            }
        }
    }
}

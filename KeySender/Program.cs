using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace KeySender
{
    class Program
    {
        private static Process process;

        private static readonly int KeyTimeout = 10;

        private static readonly string ProcessName = "<Your Process Name>";

        [DllImport("User32.dll")]
        private static extern int SetForegroundWindow(IntPtr point);

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        private static uint WM_KEYDOWN = 0x100, WM_KEYUP = 0x101, VM_CHAR = 0x0102;

        private static void SendKey(IntPtr key)
        {
            if (process.MainWindowHandle != IntPtr.Zero)
            {
                SetForegroundWindow(process.MainWindowHandle);

                PostMessage(process.MainWindowHandle, WM_KEYDOWN, IntPtr.Zero, IntPtr.Zero);

                System.Threading.Thread.Sleep(KeyTimeout);

                PostMessage(process.MainWindowHandle, VM_CHAR, key, IntPtr.Zero);

                PostMessage(process.MainWindowHandle, WM_KEYUP, IntPtr.Zero, IntPtr.Zero);
            }
        }

        private static void SendEnter()
        {
            if (process.MainWindowHandle != IntPtr.Zero)
            {
                PostMessage(process.MainWindowHandle, WM_KEYDOWN, (IntPtr)0x0D, IntPtr.Zero);
            }
        }

        static void Main(string[] args)
        {
            var message = "This message will be sent key by key to the process";

            process = Process.GetProcesses().Where(u => u.ProcessName == ProcessName).First();

            foreach (var c in message)
                SendKey((IntPtr)c);

            SendEnter();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace KeySender
{
    class Program
    {
        [DllImport("User32.dll")]
        private static extern int SetForegroundWindow(IntPtr point);

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        private static uint WM_KEYDOWN = 0x100, WM_KEYUP = 0x101, VM_CHAR = 0x0102;

        private static Process powerShellProcess;

        private static void SendKey(IntPtr key)
        {
            if (powerShellProcess.MainWindowHandle != IntPtr.Zero)
            {
                SetForegroundWindow(powerShellProcess.MainWindowHandle);

                PostMessage(powerShellProcess.MainWindowHandle, Program.WM_KEYDOWN, IntPtr.Zero, IntPtr.Zero);

                System.Threading.Thread.Sleep(100);

                PostMessage(powerShellProcess.MainWindowHandle, Program.VM_CHAR, key, IntPtr.Zero);

                PostMessage(powerShellProcess.MainWindowHandle, Program.WM_KEYUP, IntPtr.Zero, IntPtr.Zero);
            }
        }

        private static void SendEnter()
        {
            if (powerShellProcess.MainWindowHandle != IntPtr.Zero)
            {
                PostMessage(powerShellProcess.MainWindowHandle, WM_KEYDOWN, (IntPtr)0x0D, IntPtr.Zero);
            }
        }

        static void Main(string[] args)
        {
            try
            {
                var processes = Process.GetProcesses();

                foreach (var proc in processes)
                    if (proc.MainWindowTitle.Contains("qqq"))
                    {
                        powerShellProcess = proc;
                        break;
                    }
                        

                var message = "\"./RTM bat local\"";

                foreach (var c in message)
                    SendKey((IntPtr)c);

                SendEnter();
            }
            catch(Exception e)
            {
                Console.WriteLine("The process named `qqq` is not started.");
            }
        }
    }
}

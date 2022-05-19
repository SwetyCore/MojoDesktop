using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Interop;

namespace InGameWebView.Utils
{
    class WindowHelper
    {
        [DllImport("user32.dll")]
        public static extern bool GetAsyncKeyState(Keys vKey);
        [DllImport("user32.dll")]
        public static extern bool GetKeyState(Keys vKey);
        private const int GWL_HWNDPARENT = -8;
        [DllImport("user32.dll", EntryPoint = "SetWindowLongA")]
        private static extern int SetWindowLong(int hwnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll", EntryPoint = "FindWindowExA")]
        private static extern int FindWindowEx(int hWndParent, int hWndChildAfter, string lpszClass, string lpszWindow);
        [DllImport("user32.dll", EntryPoint = "FindWindowA")]
        private static extern int FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndlnsertAfter, int X, int Y, int cx, int cy, uint Flags);
        [DllImport("user32.dll", EntryPoint = "SetParent")]
        public static extern int SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
        [DllImport("user32.dll")]
        private static extern int GetForegroundWindow();

        public int ysHandle = 0x00050DC0;

        private static int FindYS()
        {
            return FindWindow("UnityWndClass", null);
        }

        IntPtr myhandle;
        MainWindow wd;
        public WindowHelper(MainWindow window)
        {
            ysHandle = FindYS();
            this.myhandle = new WindowInteropHelper(window).Handle;
            wd = window;
        }


        public void Enable()
        {

            try
            {

                ysHandle = FindYS();

                if (ysHandle == 0)
                {
                    throw new Exception();
                }

                //SetParent(new WindowInteropHelper(this).Handle, );

                SetWindowLong((int)myhandle, (-20), 0x80);

                SetWindowLong((int)myhandle, GWL_HWNDPARENT, ysHandle);
                SetWindowPos(myhandle, new IntPtr(0), 0, 0, 0, 0, 1 | 2);


            }
            catch (Exception ex)
            {
                MessageBox.Show("未找到原神进程，程序将以普通窗口方式运行！");
                //Topmost = true;
            }


            CheckKeyAsync();
        }

        

        public async Task CheckKeyAsync()
        {
            while (true)
            {
                var r = GetAsyncKeyState(Keys.T);
                int focus = GetForegroundWindow();
                //Console.WriteLine(focus);


                if (r)
                {
                    if (focus == ysHandle)
                    {

                        wd.Show = true;
                        await Task.Delay(1000);
                    }




                }
                else
                {
                    await Task.Delay(100);
                }
            }
        }
    }
}

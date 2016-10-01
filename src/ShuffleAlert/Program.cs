using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;

namespace ShuffleAlert
{
    public class Program
    {
        // required to hide the console window
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        [STAThread]
        public static void Main(string[] args)
        {
            // hide the console window
            ShowWindow(GetConsoleWindow(), SW_HIDE);

            // enable windows forms stuff
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);            

            // the notify icon with exit menu item
            var notifyIcon = new NotifyIcon()
            {
                Text = "Shuffle Chat Check",
                Visible = true,
                Icon = SystemIcons.Application,
                BalloonTipIcon = ToolTipIcon.Warning,
                BalloonTipTitle = "Shufflechat died",
                BalloonTipText = "MembersInChat reached zero"
            };
            
            notifyIcon.ContextMenu = new ContextMenu(
                new MenuItem[] {
                    new MenuItem("Exit", (s, e) => {
                        notifyIcon.Visible = false;
                        Application.Exit();
                    })
                }
            );

            // check class with action handlers
            var check = new ShuffleCheck();
            check.AlertOn = () =>
            {
                notifyIcon.Icon = SystemIcons.Error;
                notifyIcon.ShowBalloonTip(30 * 1000);
            };

            check.AlertOff = () =>
            {
                notifyIcon.Icon = SystemIcons.Application;
            };

            // main check loop
            Task.Run(async () => await check.Run());

            // run the "forms" application
            Application.Run();
        }
    }
}

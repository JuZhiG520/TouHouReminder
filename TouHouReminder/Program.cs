using Microsoft.Toolkit.Uwp.Notifications;

namespace TouHouReminder
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ToastNotificationManagerCompat.History.Clear();

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());

            ToastNotificationManagerCompat.Uninstall();
        }

        //const int SPI_SETDESKWALLPAPER = 0x0014;
        //const int SPIF_SENDWININICHANGE = 0x0002;
    }
}
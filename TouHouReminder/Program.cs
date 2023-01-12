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
            bool createdNew;
            using Mutex mutex = new(true, Application.ProductName, out createdNew);

            if (!createdNew)
            {
                Environment.Exit(0);
            }

            ToastNotificationManagerCompat.History.Clear();

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            try
            {
                Application.Run(new MainForm());
            }
            catch (Exception e)
            {
                using StreamWriter streamWriter = new($"{Application.StartupPath}\\log.txt");
                streamWriter.WriteLine("--------------------Time--------------------");
                streamWriter.WriteLine(DateTime.Now);
                streamWriter.WriteLine();
                streamWriter.WriteLine("--------------------Message--------------------");
                streamWriter.WriteLine(e.Message);
                streamWriter.WriteLine();
                streamWriter.WriteLine("--------------------TargetSite--------------------");
                streamWriter.WriteLine(e.TargetSite);
                streamWriter.WriteLine();
                streamWriter.WriteLine("--------------------StackTrace--------------------");
                streamWriter.WriteLine(e.StackTrace);
                streamWriter.Flush();

                throw;
            }
            

            ToastNotificationManagerCompat.Uninstall();
        }

        //const int SPI_SETDESKWALLPAPER = 0x0014;
        //const int SPIF_SENDWININICHANGE = 0x0002;
    }
}
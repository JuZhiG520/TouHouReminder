using Microsoft.Toolkit.Uwp.Notifications;
using Windows.UI.Notifications;

namespace TouHouReminder
{
    internal static class ToastManager
    {
        private static uint sequenceNumber = 0;

        public static void Send(string title, string text)
        {
            new ToastContentBuilder()
                .AddArgument("action", "viewConversation")
                .AddArgument("conversationId", ++sequenceNumber)
                .AddText(title)
                .AddText(text)
                .Show();
        }

        public static void Send(string title, string text, DateTime dateTime)
        {
            new ToastContentBuilder()
                .AddArgument("action", "viewConversation")
                .AddArgument("conversationId", ++sequenceNumber)
                .AddText(title)
                .AddText(text)
                .Schedule(dateTime);
        }

        public static void Send(string tag, string text, string title, string varStr, string status)
        {
            ToastContent content = new ToastContentBuilder()
                .AddText(text)
                .AddVisualChild(new AdaptiveProgressBar()
                {
                    Title = title,
                    Value = new BindableProgressBarValue("progressValue"),
                    ValueStringOverride = new BindableString("progressValueString"),
                    Status = new BindableString("progressStatus")
                })
                .GetToastContent();

            ToastNotification toast = new(content.GetXml())
            {
                Tag = tag,
                Data = new NotificationData() { SequenceNumber = 0 }
            };

            toast.Data.Values["progressValue"] = "0";
            toast.Data.Values["progressValueString"] = varStr;
            toast.Data.Values["progressStatus"] = status;

            ToastNotificationManagerCompat.CreateToastNotifier().Show(toast);
        }

        public static NotificationUpdateResult Update(string tag, string var, string varStr, string status)
        {
            NotificationData data = new() { SequenceNumber = ++sequenceNumber };

            data.Values["progressValue"] = var;
            data.Values["progressValueString"] = varStr;
            data.Values["progressStatus"] = status;

            return ToastNotificationManagerCompat.CreateToastNotifier().Update(data, tag);
        }
    }
}

using System.Text.Encodings.Web;
using System.Text.Json;

namespace TouHouReminder
{
    internal class ConfigLoader
    {
        private static void Write(string text)
        {
            using StreamWriter writer = File.CreateText("config.json");
            writer.WriteLine(text);
            writer.Flush();
        }

        private static void Generate()
        {
            FestivalCollection? collection;

            try
            {
                collection = MoeGirlParser.ParseMoeGirlCode();
            }
            catch
            {
                Write(Properties.Resources.ReservedString);
                ToastManager.Send("Completed", "The configuration file is initialized locally");

                return;
            }

            int totalCount = collection.GetTotalCount();

            ToastManager.Send("init", "Initializing configuration file...",
                "Festival timetable", $"0/{totalCount} festivals", "Initializing...");

            JsonSerializerOptions options = new()
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            string? text;

            try
            {
                text = JsonSerializer.Serialize(MoeGirlParser.ConvertFromTHBWiki(collection), options);

            }
            catch
            {
                Write(Properties.Resources.ReservedString);
                ToastManager.Update("init", "1", $"{totalCount}/{totalCount} festivals", "Completed (Local)");

                return;
            }

            Write(text);
            ToastManager.Update("init", "1", $"{totalCount}/{totalCount} festivals", "Completed");
        }

        public static FestivalCollection? Load(string fileName)
        {
            if (!File.Exists(fileName))
            {
                Generate();
            }

            JsonSerializerOptions options = new()
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            return JsonSerializer.Deserialize<FestivalCollection>(File.ReadAllText(fileName), options);
        }
    }
}

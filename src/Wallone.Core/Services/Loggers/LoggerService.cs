using System;
using System.IO;
using System.Threading.Tasks;
using Wallone.Core.Helpers;

namespace Wallone.Core.Services.Loggers
{
    public enum Message
    {
        Default,
        Warn,
        Error
    }
    public class LoggerService
    {
        private static bool isEnable;
        public static string FileName { get; private set; }


        public static string DefaultFileName()
        {
            return $"app-{DateTime.Now:yy-MM-dd}.log";
        }
        public static void SetFileName(string appLog = "app.log")
        {
            FileName = appLog;
        }

        public static string GetFolderPath()
        {
            return Platformer.GetHelper().GetCurrentFolder();
        }

        public static void Activate()
        {
            isEnable = true;
        }

        public static void Deactivate()
        {
            isEnable = false;
        }

        public static string GetFilePath()
        {
            return Path.Combine(GetFolderPath(), FileName);
        }

        public static async Task LogAsync(object useClass,string message, Message type = Message.Default)
        {
            try
            {
                if (isEnable)
                {
                    using (var file = new StreamWriter(GetFilePath(), true))
                    {
                        await file.WriteLineAsync(ContentFormatter(useClass, message, type));
                        file.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                using (var file = new StreamWriter("critical.txt", true))
                {
                    await file.WriteLineAsync(ex.Message);
                    file.Close();
                }
            }
            await Task.CompletedTask;
        }

        private static string ContentFormatter(object useClass, string message, Message type = Message.Default)
        {
            return $"{DateTime.Now} - {type} - {useClass} - {message}";
        }
    }
}
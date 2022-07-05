using System;
using System.IO;
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

        public static void Log(object useClass,string message, Message type = Message.Default)
        {
            try
            {
                if (isEnable)
                {
                    using (var file = new StreamWriter(GetFilePath(), true))
                    {
                        file.WriteLineAsync(ContentFormatter(useClass, message, type));
                        file.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                using (var file = new StreamWriter("critical.txt", true))
                {
                    file.WriteLineAsync(ex.Message);
                    file.Close();
                }
            }
        }

        private static string ContentFormatter(object useClass, string message, Message type = Message.Default)
        {
            return $"{DateTime.Now} - {type} - {useClass} - {message}";
        }
    }
}
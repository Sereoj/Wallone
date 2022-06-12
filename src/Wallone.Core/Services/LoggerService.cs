using System;
using System.IO;
using Wallone.Core.Helpers;

namespace Wallone.Core.Services
{
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
            var path = AppSettingsService.GetAppLocation();
            return path ?? Platformer.GetHelper().GetCurrentFolder();
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

        public static void Log(object useClass,string message)
        {
            if (isEnable)
            {
                using (var file = new StreamWriter(GetFilePath(), true))
                {
                    file.WriteLine(ContentFormatter(useClass, message));
                    file.Close();
                }
            }
        }

        private static string ContentFormatter(object useClass, string message)
        {
            return $"{DateTime.Now} - {useClass} - {message}";
        }
    }
}
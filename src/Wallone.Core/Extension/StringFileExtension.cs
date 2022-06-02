using System.IO;

namespace Wallone.Core.Extension
{
    public static class StringFileExtension
    {
        /// <summary>Создание файла</summary>
        /// <param name="Path"></param>
        public static void CreateFile(this string Path, string content)
        {
            File.WriteAllText(Path, content);
        }

        /// <summary>Проверка файла</summary>
        /// <param name="Path"></param>
        public static bool ExistsFile(this string Path)
        {
            return File.Exists(Path);
        }

        /// <summary>Чтение файла</summary>
        /// <param name="Path"></param>
        public static string ReadFile(this string Path)
        {
            return File.ReadAllText(Path);
        }

        /// <summary>Удаление файла</summary>
        /// <param name="Path"></param>
        public static void DeleteFile(this string Path)
        {
            File.Delete(Path);
        }
    }
}
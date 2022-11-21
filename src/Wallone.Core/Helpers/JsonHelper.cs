using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Wallone.Core.Services.Loggers;

namespace Wallone.Core.Helpers
{
    public class Json<T>
    {
        public static T Decode(string values)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(values);
            }
            catch (Exception e)
            {
                _ = LoggerService.LogAsync(typeof(Json<T>), $"{e.Message}");
                throw;
            }
        }
    }

    public class JsonHelper
    {
        public static bool IsValidJson(string strInput)
        {
            if (string.IsNullOrWhiteSpace(strInput)) return false;
            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
                (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
                try
                {
                    var obj = JToken.Parse(strInput);
                    return true;
                }
                catch (JsonReaderException jex)
                {
                    //Exception in parsing json
                    Console.WriteLine(jex.Message);
                    return false;
                }
                catch (Exception ex) //some other exception
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }

            return false;
        }
    }
}
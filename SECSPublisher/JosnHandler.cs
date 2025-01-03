using System;
using System.IO;
using Newtonsoft.Json;

namespace SECSPublisher
{
    public class JosnHandler
    {
        public static void WriteJosn(object result, string FilePath)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(FilePath))
                {
                    string json = JsonConvert.SerializeObject(result, Formatting.Indented);
                    sw.WriteLine(json);
                }
            }
            catch (Exception ex)
            {
                Logger.SECSLogger.Error($"Function {ex.TargetSite.Name} Error:{ex.Message}");
            }
            
        }
        public static T ReadJosn<T>(string FilePath)
        {
            T result = default(T);
            try
            {
                using (StreamReader sr = new StreamReader(FilePath))
                {
                    string json = sr.ReadToEnd();
                    result = JsonConvert.DeserializeObject<T>(json);
                }
            }
            catch (Exception ex)
            {
                Logger.SECSLogger.Error($"Function {ex.TargetSite.Name} Error:{ex.Message}");
            }
            return result;
        }

    }
}

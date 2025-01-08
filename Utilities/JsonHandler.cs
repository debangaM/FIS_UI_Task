using System.Text.Json;

namespace FIS_UI_Task.Utilities
{
    public class JsonHandler
    {
        private Dictionary<string, string> _jsonDictionary;

        public void LoadJson(string fileName)
        {
            string baseDirectory = AppContext.BaseDirectory;
            string folderName = "PageObjectLocators";
            string filePath = Path.Combine(baseDirectory, folderName, fileName+".json");
            try
            {
                string jsonContent = File.ReadAllText(filePath);

                _jsonDictionary = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonContent);
                Console.WriteLine("Loaded locators into memory from: "+filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading JSON: " + ex.Message);
            }
        }

        public string GetValue(string key)
        {
            if (_jsonDictionary != null && _jsonDictionary.TryGetValue(key, out string value))
            {
                return value;
            }
            return $"Key '{key}' not found in JSON.";
        }
    }
}

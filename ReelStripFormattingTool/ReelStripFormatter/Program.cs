using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

class Program
{
    static void Main()
    {
        int[][] inputArray = LoadInputArrayFromConfig("reel_config.json", "inputArray");

        JObject nameConfigObject = LoadConfigObject("name_config.json");
        JObject idConfigObject = LoadConfigObject("id_config.json");
        JObject typeConfigObject = LoadConfigObject("type_config.json");
        JObject codeConfigObject = LoadConfigObject("code_config.json");

        JArray jsonArray = new JArray();
        foreach (int[] section in inputArray)
        {
            JArray gridJsonArray = new JArray();
            for (int i = 0; i < section.Length; i++)
            {
                JObject itemObject = new JObject();
                itemObject["id"] = GetConfigValueForValue(section[i], idConfigObject);
                itemObject["name"] = GetConfigValueForValue(section[i], nameConfigObject);
                itemObject["type"] = GetConfigValueForValue(section[i], typeConfigObject);
                itemObject["code"] = GetConfigValueForValue(section[i], codeConfigObject);

                JArray itemArray = new JArray();
                itemArray.Add(i);
                itemArray.Add(itemObject);

                gridJsonArray.Add(itemArray);
            }

            jsonArray.Add(gridJsonArray);
        }

        JObject jsonObject = new JObject();
        jsonObject["strip_grid"] = jsonArray;

        string json = jsonObject.ToString(Formatting.Indented);
        Console.WriteLine(json);
    }

    static int[][] LoadInputArrayFromConfig(string configFile, string arrayKey)
    {
        JObject configObject = LoadConfigObject(configFile);
        if (configObject.ContainsKey(arrayKey))
        {
            return configObject[arrayKey].ToObject<int[][]>();
        }
        else
        {
            Console.WriteLine($"Array key '{arrayKey}' not found in the config file.");
            return new int[0][];
        }
    }

    static JObject LoadConfigObject(string configFile)
    {
        string configText = File.ReadAllText(configFile);
        return JObject.Parse(configText);
    }

    static dynamic GetConfigValueForValue(int value, JObject configObject)
    {
        string configKey = value.ToString();
        if (configObject.ContainsKey(configKey))
        {
            return configObject[configKey];
        }
        else
        {
            return GetDefaultValueForConfigType(configObject);
        }
    }

    static dynamic GetDefaultValueForConfigType(JObject configObject)
    {
        JToken firstValue = configObject.First;
        if (firstValue != null)
        {
            return firstValue;
        }
        else
        {
            Console.WriteLine("Config object is empty. Returning null as default value.");
            return null;
        }
    }
}

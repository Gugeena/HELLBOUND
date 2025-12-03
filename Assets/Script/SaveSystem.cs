using UnityEngine;
using System.IO;

public class SaveSystem
{
    private static string path = Path.Combine(Application.persistentDataPath, "GlobalInformation.json");

    public static GlobalSettings Load()
    {
        if(!File.Exists(path))
        {
            GlobalSettings settings = new GlobalSettings();
            settings.information = new Information();
            settings.audio = new AudioData();
            settings.keybinds = new KeybindsData();
            return settings;
        }

        string json = File.ReadAllText(path);
        GlobalSettings result = JsonUtility.FromJson<GlobalSettings>(json);

        return result;
    }

    public static void Save(GlobalSettings settings)
    {
        string json = JsonUtility.ToJson(settings, true);
        File.WriteAllText(path, json);
    }
}

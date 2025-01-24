using System.IO;
using UnityEngine;

public class Saver : MonoBehaviour
{
    public static Saver Instance;

    string filePath;
    PlayerData data = new PlayerData();

    private void Awake()
    {
        Instance = this;
        filePath = Path.Combine(Application.persistentDataPath, "data.json");

    }

   public void SaveInfo(PlayerData save)
    {
        File.WriteAllText(filePath, JsonUtility.ToJson(save));
    }

    public PlayerData LoadInfo()
    {
        if (File.Exists(filePath))
        {
            data = JsonUtility.FromJson<PlayerData>(File.ReadAllText(filePath));
        }
        else
        {
            SaveInfo(data);
        }
        return data;
    }
}

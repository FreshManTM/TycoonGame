using Palmmedia.ReportGenerator.Core.Common;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Saver : MonoBehaviour
{
    public static Saver Instance;

    string filePath;
    PlayerData data = new PlayerData();

    private void Awake()
    {
        DontDestroyOnLoad(this);
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
            //data.InventoryItems = new List<InventoryItem>();
            SaveInfo(data);
        }
        return data;
    }
}

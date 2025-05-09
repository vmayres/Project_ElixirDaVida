using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static string GetSavePath(int slot)
    {
        return Application.persistentDataPath + $"/save{slot}.dat";
    }

    public static void SaveGame(SaveData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = GetSavePath(data.saveSlot);
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SaveData LoadGame(int slot)
    {
        string path = GetSavePath(slot);

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();
            return data;
        }
        else
        {
            return null;
        }
    }

    public static void Delete(int slot)
    {
        string path = GetSavePath(slot);
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log($"Save slot {slot} deletado.");
        }
        else
        {
            Debug.Log($"Save slot {slot} não existe.");
        }
    }
}

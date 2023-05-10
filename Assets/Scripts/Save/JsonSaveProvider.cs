using UnityEngine;
using System.IO;

public class JsonSaveProvider<T> : ISaveProvider<T>
{
    public T TryGetSave(string fileName)
    {
        string filePath = GetFullFilePath(fileName);

        if (!File.Exists(filePath))
            return default;

        using StreamReader reader = new(filePath);
        string json = reader.ReadToEnd();
        Debug.Log(json);
        T save = JsonUtility.FromJson<T>(json);
        return save;
    }

    public void UpdateSave(T save, string fileName)
    {
        using StreamWriter writer = new(GetFullFilePath(fileName));
        string json = JsonUtility.ToJson(save);
        Debug.Log(json);
        writer.Write(json);
    }

    private string GetFullFilePath(string fileName) 
        => Application.persistentDataPath + Path.AltDirectorySeparatorChar + fileName + ".json";
}
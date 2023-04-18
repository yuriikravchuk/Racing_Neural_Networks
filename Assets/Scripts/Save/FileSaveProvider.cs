using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class FileSaveProvider<T> : ISaveProvider<T>
{
    private readonly BinaryFormatter _bf;

    public FileSaveProvider() 
        => _bf = new BinaryFormatter();

    public T TryGetSave(string fileName)
    {
        string filePath = GetFullFilePath(fileName);
        if (!File.Exists(filePath))
        {
            return default;
        }
        else
        {
            var fileStream = new FileStream(filePath, FileMode.Open);
            fileStream.Position = 0;
            T save = (T)_bf.Deserialize(fileStream);
            fileStream.Close();
            return save;
        }
    }

    public void UpdateSave(T save, string fileName)
    {
        var fileStream = new FileStream(GetFullFilePath(fileName), FileMode.Create);
        _bf.Serialize(fileStream, save);
        fileStream.Close();
    }

    private string GetFullFilePath(string fileName) 
        => Application.persistentDataPath + "/" + fileName + ".gm";
}

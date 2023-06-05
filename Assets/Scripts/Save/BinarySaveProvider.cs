using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
public class BinarySaveProvider : ISaveProvider
{
    private readonly BinaryFormatter _binaryFormater;

    public BinarySaveProvider()
        => _binaryFormater = new BinaryFormatter();

    public T TryGetSave<T>(string fileName)
    {
        string path = GetFullFilePath(fileName);
        if (!File.Exists(path))
            return default;

        var fileStream = new FileStream(path, FileMode.Open);
        fileStream.Position = 0;
        T save = (T)_binaryFormater.Deserialize(fileStream);
        fileStream.Close();
        return save;
    }

    public void UpdateSave<T>(T save, string fileName)
    {
        string path = GetFullFilePath(fileName);
        var fileStream = new FileStream(path, FileMode.Create);
        _binaryFormater.Serialize(fileStream, save);
        fileStream.Close();
    }

    private string GetFullFilePath(string fileName)
        => Application.streamingAssetsPath + Path.AltDirectorySeparatorChar + fileName + ".gm";
}
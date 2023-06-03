public interface ISaveProvider
{
    public T TryGetSave<T>(string fileName);
    public void UpdateSave<T>(T save, string fileName);
}
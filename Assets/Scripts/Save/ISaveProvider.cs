public interface ISaveProvider<T>
{
    public T TryGetSave(string fileName);
    public void UpdateSave(T save, string fileName);
}
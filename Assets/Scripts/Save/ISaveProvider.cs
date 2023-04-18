public interface ISaveProvider<T>
{
    T TryGetSave(string fileName);
    void UpdateSave(T save, string fileName);
}

public class SaveBinder
{
    private readonly ISaveProvider<BestAISave> _provider;
    private readonly ISaveHandler<BestAISave> _handler;
    private const string _fileName = "save";

    public SaveBinder(ISaveProvider<BestAISave> provider)
    {
        _provider = provider;
    }


    public void Load()
    {
        BestAISave save = _provider.TryGetSave(_fileName);
        _handler.Save = save;
    }

    public void Save()
    {
        BestAISave save = _handler.Save;
        _provider.UpdateSave(save, _fileName);
    }
}

public interface ISaveHandler<T>
{
    T Save { get; set; }
}
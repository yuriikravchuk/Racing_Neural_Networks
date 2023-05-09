using AI;
using System.Collections.Generic;

public class SaveBinder
{
    private readonly ISaveProvider<List<TrainingResults>> _provider;
    private readonly WeightsBalancer _handler;
    private const string _fileName = "save";

    public SaveBinder(ISaveProvider<List<TrainingResults>> provider, WeightsBalancer handler)
    {
        _provider = provider;
        _handler = handler;
    }


    public void Load()
    {
        _handler.Parents = _provider.TryGetSave(_fileName);
        //return _provider.TryGetSave(_fileName);
    }

    public void Save()
    {
        List<TrainingResults> save = _handler.Parents;
        _provider.UpdateSave(save, _fileName);
    }
}

public interface ISaveHandler<T>
{
    T Save { get; set; }
}
using AI;
using System.Collections.Generic;
using System.Linq;

public class SaveBinder
{
    private readonly ISaveProvider<IReadOnlyList<TrainingResults>> _provider;
    private readonly WeightsBalancer _handler;
    private const string _fileName = "save";

    public SaveBinder(ISaveProvider<IReadOnlyList<TrainingResults>> provider, WeightsBalancer handler)
    {
        _provider = provider;
        _handler = handler;
    }


    public void Load()
    {
        IReadOnlyList<TrainingResults> save = _provider.TryGetSave(_fileName);
        _handler.AddParents(save);
    }

    public void Save()
    {
        IReadOnlyList<TrainingResults> save = _handler.Parents;
        _provider.UpdateSave(save, _fileName);
    }
}
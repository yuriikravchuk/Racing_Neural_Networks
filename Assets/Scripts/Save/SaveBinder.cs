using AI;
using System.Collections.Generic;
using System.Linq;

public class SaveBinder
{
    private readonly ISaveProvider _provider;
    private const string _fileName = "save1";

    public SaveBinder() => _provider = new BinarySaveProvider();

    public IReadOnlyList<TrainingResults> Load()
    {
        IReadOnlyList<TrainingResults> save = _provider.TryGetSave<IReadOnlyList<TrainingResults>>(_fileName);
        //List<TrainingResults> results = new List<TrainingResults>();
        //foreach(TrainingResults result in save) {
        //    results.Add(new TrainingResults(result.Neurons, 0)) ;
        //}

        return save;
    }

    public void Save(IReadOnlyList<TrainingResults> save)
    {

        _provider.UpdateSave(save, _fileName);
    }
}
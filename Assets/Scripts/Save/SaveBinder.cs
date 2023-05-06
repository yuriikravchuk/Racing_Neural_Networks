    using AI;
public class SaveBinder
{
    private readonly ISaveProvider<BestAISave> _provider;

    public SaveBinder(WeightsBalancer trainer)
    {
        _provider = new JsonSaveProvider<BestAISave>();
    }


    public void Load()
    {
        //BestAISave save = _provider.TryGetSave();
        //_level.Init(save.Level);
        //_wallet.Init(save.Money);
    }

    public void Save()
    {

    }
}

    using AI;
public class SaveBinder
{
    private readonly ISaveProvider<BestAISave> _provider;

    public SaveBinder(WeightsBalancer trainer)
    {
        _provider = new FileSaveProvider<BestAISave>();
    }


    public void Bind()
    {
        //BestAISave save = _provider.TryGetSave();
        //_level.Init(save.Level);
        //_wallet.Init(save.Money);
    }
}

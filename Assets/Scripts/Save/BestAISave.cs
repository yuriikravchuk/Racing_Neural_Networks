using AI;
[System.Serializable]
public class BestAISave
{
    public readonly TrainingResults[] Save;

    public BestAISave(TrainingResults[] save)
    {
        Save = save;
    }
}
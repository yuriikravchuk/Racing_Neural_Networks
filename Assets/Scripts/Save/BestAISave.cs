using AI;
[System.Serializable]
class BestAISave
{
    public NeuralNetwork[] BestAi;

    public BestAISave(NeuralNetwork[] bestAi)
    {
        BestAi = bestAi;
    }
}
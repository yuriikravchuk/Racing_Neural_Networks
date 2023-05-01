namespace AI
{
    public struct TrainingResults
    {
        public readonly Neuron[][] Neurons;
        public readonly int Score;

        public TrainingResults(Neuron[][] neurons, int score)
        {
            Neurons = neurons;
            Score = score;
        }
    }
}
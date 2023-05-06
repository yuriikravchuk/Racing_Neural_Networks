namespace AI
{
    public readonly struct TrainingResults
    {
        public readonly Neuron[][] Neurons;
        public readonly float Score;

        public TrainingResults(Neuron[][] neurons, float score)
        {
            Neurons = neurons;
            Score = score;
        }
    }
}
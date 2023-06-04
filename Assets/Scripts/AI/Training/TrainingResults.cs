using System;

namespace AI
{
    [Serializable]
    public class TrainingResults
    {
        public readonly Neuron[][] Neurons;
        public readonly float Fitness;

        public TrainingResults(Neuron[][] neurons, float score)
        {
            Neurons = neurons;
            Fitness = score;
        }
    }
}
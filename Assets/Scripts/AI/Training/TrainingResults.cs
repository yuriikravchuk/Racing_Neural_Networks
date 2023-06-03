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

    [Serializable]
    public class TestResults
    {
        public readonly Neuron[][] Neurons;
        public readonly float Fitness;
        public readonly float Duration;

        public TestResults(Neuron[][] neurons, float score, float duration)
        {
            Neurons = neurons;
            Fitness = score;
            Duration = duration;
        }
    }
}
using System;
using System.Collections.Generic;

namespace AI
{
    [Serializable]
    public class Neuron
    {
        public IReadOnlyList<float> Weights => _weights;
        public float Bias { get; private set; }
        public float Value;

        private readonly float[] _weights;

        public Neuron(int childsCount = 0) {
            _weights = new float[childsCount];
        }

        public Neuron(IReadOnlyList<float> weights, float bias) : this(weights.Count)
        {
            for (int i = 0; i < weights.Count; i++)
                _weights[i] = weights[i];

            Bias = bias;
        }
    }
}
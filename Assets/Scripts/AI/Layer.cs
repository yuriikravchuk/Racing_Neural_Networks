using System;
using System.Collections.Generic;

namespace AI
{
    public class Layer
    {
        public int NeuronsCount => _neurons.Length;
        public IReadOnlyList<float> Values => _values;

        private readonly float[] _values;
        private Neuron[] _neurons;

        public Layer(int neyronsCount, int childNeuronsCount = 0)
        {
            _neurons = new Neuron[neyronsCount];
            _values = new float[neyronsCount];


            for (int i = 0; i < _neurons.Length;i++)
                _neurons[i] = new Neuron(childNeuronsCount);
        }

        public void SetNeurons(Neuron[] neurons) => _neurons = neurons;

        public Neuron[] CopyNeurons()
        {
            var neurons = new Neuron[NeuronsCount];

            for(int i = 0; i < _neurons.Length; i++)
                neurons[i] = new Neuron(_neurons[i].Weights, _neurons[i].Bias);
            
            return neurons;

        }

        public virtual void Update(IReadOnlyList<float> values)
        {
            for (int i = 0; i < NeuronsCount; i++)
            {
                float input = 0f;

                for (int j = 0; j < values.Count; j++)
                    input += values[j] * _neurons[i].Weights[j];

                float value = ActivationFunction(input + _neurons[i].Bias);
                SetValueToNeuron(i, value);
            }
        }

        protected void SetValueToNeuron(int index, float value)
        {
            _neurons[index].Value = value;
            _values[index] = value;
        }

        protected virtual float ActivationFunction(float x) => Sigmoid(x);

        protected float Sigmoid(float x)
            => 1.0f / (1.0f + (float)Math.Exp(-x));

        protected float Tanh(float x)
            => 2.0f / (1.0f + (float)Math.Exp(-2.0f * x)) - 1.0f;
    }
}
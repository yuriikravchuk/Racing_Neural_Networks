using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AI
{
    public class Layer
    {
        //public  int NeuronsCount => _neuronsCount;
        public int NeuronsCount => _neurons.Length;
        public IReadOnlyList<float> Values => _values;

        private float[] _values;
        private Neuron[] _neurons;

        private readonly Layer _child;

        public Layer(int neyronsCount, Layer child = null)
        {
            _child = child;
            _neurons = new Neuron[neyronsCount];
            _values = new float[neyronsCount];
            bool hasChild = child != null;
            for (int i = 0; i < _neurons.Length;i++)
                _neurons[i] = hasChild ? new Neuron(child.NeuronsCount) : new Neuron();

        }

        //public void SetNeurons(float[,] weights, float[] bias)
        //{
        //    for(int  i = 0; i < weights.GetLength(0); i++)
        //    {
        //        var weightsForNeuron = new float[weights.GetLength(1)];
        //        for(int j = 0; j < weightsForNeuron.Length; j++) 
        //        {
        //            weightsForNeuron[j] = weights[i, j];
        //        }
        //        _neurons[i].SetWeights(weightsForNeuron, bias[i]);
        //    }
        //}

        public void SetNeurons(Neuron[] neurons) => _neurons = neurons;

        public Neuron[] CopyNeurons()
        {
            var neurons = new Neuron[NeuronsCount];

            for(int i = 0; i < _neurons.Length; i++)
                neurons[i] = new Neuron(_neurons[i].Weights, _neurons[i].Bias);
            
            return neurons;

        }

        public void Update(IReadOnlyList<float> values)
        {
            if (_child == null)
            {
                for(int i = 0; i < _neurons.Length; i++)
                {
                    _neurons[i].Value = values[i];
                    _values[i] = values[i];
                }
                return;
            }

            for (int i = 0; i < NeuronsCount; i++)
            {
                float input = 0f;

                for (int j = 0; j < values.Count; j++)
                    input += values[j] * _neurons[i].Weights[j];

                float value = Sigmoid(input + _neurons[i].Bias);
                _neurons[i].Value = value;
                _values[i] = value;
            }
        }

        private float Sigmoid(float x)
            => 1.0f / (1.0f + (float)Math.Exp(-x));

        private float Tanh(float x)
            => 2.0f / (1.0f + (float)Math.Exp(-2.0f * x)) - 1.0f;
    }
}
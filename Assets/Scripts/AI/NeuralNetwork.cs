using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace AI
{
    public class NeuralNetwork
    {
        public readonly NeuralNetworkParameters Parameters;
        private readonly Layer[] _layers;

        public NeuralNetwork(NeuralNetworkParameters parameters)
        {
            Parameters = parameters;
            _layers = new Layer[Parameters.LayersCount];
            _layers[0] = new Layer(Parameters.NeuronsInLayerCount[0]);

            for (int i = 1; i < Parameters.LayersCount; i++)
                _layers[i] = new Layer(Parameters.NeuronsInLayerCount[i], _layers[i - 1]);
        }

        public IReadOnlyList<float> GetOutputs(IReadOnlyList<float> inputs)
        {
            if (inputs.Count != _layers[0].NeuronsCount)
                throw new ArgumentOutOfRangeException();

            _layers[0].Update(inputs);

            for (int i = 1; i < Parameters.LayersCount; i++)
                _layers[i].Update(_layers[i-1].Values);

            return _layers.Last().Values;
        }

        public Neuron[][] CopyNeurons()
        {
            var neurons = new Neuron[Parameters.LayersCount][];
            for (int i = 0; i < _layers.Length; i++)
            {
                Neuron[] neuronsInLayer = _layers[i].CopyNeurons();
                neurons[i] = neuronsInLayer;
            }
            return neurons;
        }

        public void SetNeurons(Neuron[][] neurons)
        {
            for (int i = 0; i < _layers.Length; i++)
                _layers[i].SetNeurons(neurons[i]);
        }
    }

    public class Neuron
    {
        public IReadOnlyList<float> Weights => _weights;
        public float Bias { get; private set; }
        public float Value;

        private float[] _weights;

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
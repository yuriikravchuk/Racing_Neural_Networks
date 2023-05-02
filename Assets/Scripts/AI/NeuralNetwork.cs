using System;
using System.Collections.Generic;
using System.Linq;

namespace AI
{
    public class NeuralNetwork
    {
        private readonly Layer[] _layers;

        public NeuralNetwork(IReadOnlyList<int> layersSize)
        {
            var layersCount = layersSize.Count;
            _layers = new Layer[layersCount];
            _layers[0] = new InputLayer(layersSize[0]);

            for (int i = 1; i < layersCount - 1; i++)
                _layers[i] = new Layer(layersSize[i], layersSize[i-1]);

            _layers[layersCount - 1] = new OutputLayer(layersSize[^1], layersSize[^2]);
        }

        public IReadOnlyList<float> GetOutputs(IReadOnlyList<float> inputs)
        {
            if (inputs.Count != _layers[0].NeuronsCount)
                throw new ArgumentOutOfRangeException();

            _layers[0].Update(inputs);

            for (int i = 1; i < _layers.Length; i++)
                _layers[i].Update(_layers[i-1].Values);

            return _layers.Last().Values;
        }

        public Neuron[][] CopyNeurons()
        {
            var neurons = new Neuron[_layers.Length][];
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
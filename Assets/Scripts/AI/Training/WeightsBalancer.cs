using System.Collections.Generic;
using System;
using System.Linq;

namespace AI
{
    public class WeightsBalancer
    {
        public IReadOnlyList<TrainingResults> Parents => _parents;
        private List<TrainingResults> _parents;
        private const float _minWeight = -1f, _maxWeight = 1f, _mutationRate = 0.0000005f, _minBias = -1, _maxBias = 1, _maxFitnessDifference = 0.03f;
        private const int _maxParentsCount = 32;
        private readonly Random _random;

        public WeightsBalancer()
        {
            _random = new Random();
            _parents = new List<TrainingResults>();
        }

        public void AddParents(IReadOnlyList<TrainingResults> parents)
        {
            float minFitness = _parents.Count > 0 ? _parents.Min(element => element.Score) : 0;
            float lowerFitnessLimit = minFitness * (1 - _maxFitnessDifference);

            foreach (var parent in parents)
            {
                _parents.Add(parent);

                if (_parents.Count > _maxParentsCount)
                    _parents.RemoveAt(0);
            }
        }

        public Neuron[][] UniformCross()
        {
            var firstParrent = Parents[0].Neurons;
            int layersCount = firstParrent.Length;
            var result = new Neuron[layersCount][];

            result[0] = new Neuron[firstParrent[0].Length];

            for (int i = 0; i < result[0].Length; i++)
                result[0][i] = new Neuron();

            for (int layerIndex = 1; layerIndex < layersCount; layerIndex++)
            {
                int neuronsCount = firstParrent[layerIndex].Length;
                int weightsCount = firstParrent[layerIndex - 1].Length;
                result[layerIndex] = new Neuron[neuronsCount];

                for (int x = 0; x < neuronsCount; x++)
                {
                    float[] weights = new float[weightsCount];
                    for (int y = 0; y < weightsCount; y++)
                    {
                        float[] weightsFromParents = Parents.Select(item => item.Neurons[layerIndex][x].Weights[y]).ToArray();
                        float weight = GetRandomValue(weightsFromParents);
                        TryMutate(ref weight);
                        weights[y] = weight;

                    }
                    float[] parentBiases = Parents.Select(item => item.Neurons[layerIndex][x].Bias).ToArray();
                    float bias = GetRandomValue(parentBiases);
                    TryMutate(ref bias);
                    result[layerIndex][x] = new Neuron(weights, bias);
                }
            } 
            return result;
        }

        public Neuron[][] GetRandomNeurons(IReadOnlyList<int> layersSize)
        {
            int layersCount = layersSize.Count;
            var result = new Neuron[layersCount][];
            result[0] = new Neuron[layersSize[0]];

            for(int i = 0; i < layersSize[0]; i++)
                result[0][i] = new Neuron();

            for (int layerIndex = 1; layerIndex < layersCount; layerIndex++)
            {
                result[layerIndex] = new Neuron[layersSize[layerIndex]];
                int neuronsCount = result[layerIndex].Length;
                int childNeuronsCount = result[layerIndex - 1].Length;

                for (int x = 0; x < neuronsCount; x++)
                {
                    var neuronWeights = new float[childNeuronsCount];

                    for (int y = 0; y < childNeuronsCount; y++)
                        neuronWeights[y] = GetRandomInRange(_minWeight, _maxWeight);

                    float bias = GetRandomInRange(_minBias, _maxBias);
                    result[layerIndex][x] = new Neuron(neuronWeights, bias);
                }
            }
            return result;
        }

        private float GetRandomValue(float[] values)
        {
            int index = _random.Next(values.Length);
            return values[index];
        }

        private void TryMutate(ref float value)
        {
            float random = GetRandomInRange(0f, 1f);
            if (random <= _mutationRate)
                value = GetRandomInRange(_minWeight, _maxWeight);
        }

        //private float GetRandomInRange(float first, float second) => Random.Range(first, second);

        private float GetRandomInRange(float min, float max) => (float)(_random.NextDouble() * (max - min) + min);
    }
}
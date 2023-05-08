using System.Collections.Generic;
using System;
using System.Linq;
//using UnityEngine;

namespace AI
{
    public class WeightsBalancer
    {
        public IReadOnlyList<TrainingResults> Parents => _parents;

        private const float _minWeight = -1f, _maxWeight = 1f, _mutationRate = 0.001f, _minBias = -1, _maxBias = 1;
        private const int _maxParentsCount = 4;
        private readonly Random _random;
        private List<TrainingResults> _parents;

        public WeightsBalancer()
        {
            _random = new Random();
            _parents = new List<TrainingResults>();
        }

        public void AddParents(IReadOnlyList<TrainingResults> parents)
        {
            _parents.AddRange(parents);
            _parents = _parents.OrderByDescending(element => element.Score).Take(_maxParentsCount).ToList();
        }

        public Neuron[][] UniformCross(IReadOnlyList<Neuron[][]> parents)
        {
            var firstParrent = parents[0];
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
                        float[] weightsFromParents = parents.Select(item => item[layerIndex][x].Weights[y]).ToArray();
                        float weight = GetRandomValue(weightsFromParents);
                        TryMutate(ref weight);
                        weights[y] = weight;

                    }
                    float[] parentBiases = parents.Select(item => item[layerIndex][x].Bias).ToArray();
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
using System.Collections.Generic;
using System;
using System.Linq;

namespace AI
{
    public class WeightsBalancer
    {
        public IReadOnlyList<TrainingResults> Results => _results;
        public event Action<IReadOnlyList<float>> ScoresChanged;

        private readonly List<TrainingResults> _results;
        private const float _minWeight = -1f, _maxWeight = 1f, _minBias = -1, _maxBias = 1, _mutationRate = 0.04f;
        private const int _maxBestParentsCount = 6;
        private const int _maxRegularParentsCount = 0;
        private readonly Random _random;

        public WeightsBalancer()
        {
            _random = new Random();
            _results = new List<TrainingResults>();
        }

        public void SetResults(IEnumerable<TrainingResults> results)
        {
            if (results == null || results.Count() == 0) { return; }

            List<TrainingResults> bestParents = _results.Take(_maxBestParentsCount).ToList();
            List<TrainingResults> regularParents = _results.Skip(_maxBestParentsCount).ToList();

            regularParents.AddRange(results);
            regularParents = regularParents.GroupBy(item => item.Fitness).Select(d => d.First()).ToList();
            if (regularParents.Count > _maxRegularParentsCount)
                regularParents = regularParents.Skip(regularParents.Count - _maxRegularParentsCount).ToList();

            float minFitness = _results.Count > 0 ? _results.Take(_maxBestParentsCount).Min(item => item.Fitness) : 0;

            foreach (var newParent in results)
            {
                if (newParent.Fitness < minFitness)
                    continue;

                int parentIndex = bestParents.FindIndex(item => IsEqual(item.Neurons, newParent.Neurons));

                if (parentIndex == -1)
                    bestParents.Add(newParent);
                else if (bestParents[parentIndex].Fitness < newParent.Fitness)
                    bestParents[parentIndex] = newParent;
            }

            bestParents = bestParents.OrderByDescending(item => item.Fitness).Take(_maxBestParentsCount).ToList();


            _results.Clear();
            _results.AddRange(bestParents);
            _results.AddRange(regularParents);

            if (Results.Count > 0)
                ScoresChanged?.Invoke(Results.Select(item => item.Fitness).ToList());
        }

        public Neuron[][] UniformCross(float mutationMultiplyier)
        {
            var firstParrent = Results[0].Neurons;
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
                        float[] weightsFromParents = Results.Select(item => item.Neurons[layerIndex][x].Weights[y]).ToArray();
                        float weight = GetRandomValue(weightsFromParents);
                        TryMutate(ref weight, mutationMultiplyier);
                        weights[y] = weight;

                    }

                    float[] parentBiases = Results.Select(item => item.Neurons[layerIndex][x].Bias).ToArray();
                    float bias = GetRandomValue(parentBiases);
                    TryMutate(ref bias, mutationMultiplyier);
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

            for (int i = 0; i < layersSize[0]; i++)
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

        private void TryMutate(ref float value, float mutationMultiplyier)
        {
            float random = GetRandomInRange(0f, 1f);
            if (random <= _mutationRate * mutationMultiplyier)
                value = GetRandomInRange(_minWeight, _maxWeight);
        }

        private float GetRandomInRange(float min, float max) => (float)(_random.NextDouble() * (max - min) + min);

        private bool IsEqual(Neuron[][] one, Neuron[][] other)
        {
            int layersCount = one.Length;
            for (int layerIndex = 0; layerIndex < layersCount; layerIndex++)
            {
                int neuronsCount = one[layerIndex].Length;

                for (int neuronIndex = 0; neuronIndex < neuronsCount; neuronIndex++)
                {
                    int weightsCount = one[layerIndex][neuronIndex].Weights.Count;

                    for (int weightIndex = 0; weightIndex < weightsCount; weightIndex++)
                    {
                        if (one[layerIndex][neuronIndex].Weights[weightIndex] != other[layerIndex][neuronIndex].Weights[weightIndex])
                            return false;
                    }
                }
            }

            return true;
        }
    }
}
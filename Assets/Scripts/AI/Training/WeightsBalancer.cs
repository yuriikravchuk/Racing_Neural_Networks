using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AI
{
    public class WeightsBalancer
    {
        private const float _minWeight = -1f, _maxWeight = 1f, _mutationRate = 0.01f, _minBias = -1, _maxBias = 1;
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
                int neuronsCountInChild = firstParrent[layerIndex - 1].Length;
                result[layerIndex] = new Neuron[neuronsCount];

                for (int x = 0; x < neuronsCount; x++)
                {
                    float[] weights = new float[neuronsCountInChild];
                    for (int y = 0; y < neuronsCountInChild; y++)
                    {
                        float[] weightsFromParents = parents.Select(item => item[layerIndex][x].Weights[y]).ToArray();
                        float weight = GetRandomValue(weightsFromParents);
                        TryMutate(ref weight);
                        
                    }
                    float[] parentBiases = parents.Select(item => item[layerIndex][x].Bias).ToArray();
                    float bias = GetRandomValue(parentBiases);
                    result[layerIndex][x] = new Neuron(weights, bias);
                }
            } 
            return result;
        }

        public Neuron[][] GetRandomNeurons(NeuralNetworkParameters parameters)
        {
            var result = new Neuron[parameters.LayersCount][];
            result[0] = new Neuron[parameters.NeuronsInLayerCount[0]];

            for(int i = 0; i < parameters.NeuronsInLayerCount[0]; i++)
                result[0][i] = new Neuron();

            for (int layerIndex = 1; layerIndex < parameters.LayersCount; layerIndex++)
            {
                result[layerIndex] = new Neuron[parameters.NeuronsInLayerCount[layerIndex]];
                int neuronsCount = result[layerIndex].Length;
                int neuronsCountInChild = result[layerIndex - 1].Length;


                for (int x = 0; x < neuronsCount; x++)
                {
                    var neuronWeights = new float[neuronsCountInChild];

                    for (int y = 0; y < neuronsCountInChild; y++)
                        neuronWeights[y] = GetRandomInRange(_minWeight, _maxWeight);

                    float bias = GetRandomInRange(_minBias, _maxBias);
                    result[layerIndex][x] = new Neuron(neuronWeights, bias);
                }
            }
            return result;
        }

        private float GetRandomValue(float[] values)
        {
            float randomValue = GetRandomInRange(0f, 1f);
            float chanceStep = 1f / values.Length;

            for (int i = 0; i < values.Length; i++)
            {
                if (chanceStep * (i + 1) >= randomValue)
                    return values[i];
            }
            throw new System.IndexOutOfRangeException();
        }

        private void TryMutate(ref float value)
        {
            if (GetRandomInRange(0f, 1f) <= _mutationRate)
                value = GetRandomInRange(_minWeight, _maxWeight);
        }

        private float GetRandomInRange(float first, float second) => Random.Range(first, second);
    }
}
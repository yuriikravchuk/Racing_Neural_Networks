using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AI
{
    public class WeightsBalancer
    {
        private const float _minWeight = 0f, _maxWeight = 1f, _mutationRate = 0.01f;

        public void UniformCross(NeuralNetwork[] parents, NeuralNetwork[] children)
        {
            IEnumerable<float[][,]> parentsWeights = parents.Select(parent => parent.CopyWeights());
            NeuralNetworkParameters parameters = parents[0].Parameters;
            foreach(var child in children)
                UniformCross(parentsWeights, child);
        }

        private void UniformCross(IEnumerable<float[][,]> parentsWeights, NeuralNetwork child)
        {
            var childWeights = child.CopyWeights();
            for (int layerIndex = 0; layerIndex < child.Parameters.LayersCount - 1; layerIndex++)
            {
                int neuronsCount = childWeights[layerIndex].GetLength(0);
                int neuronsCountInChild = childWeights[layerIndex].GetLength(1);

                for (int x = 0; x < neuronsCount; x++)
                {
                    for (int y = 0; y < neuronsCountInChild; y++)
                    {
                        float[] valuesFromParents = parentsWeights.Select(item => item[layerIndex][x, y]).ToArray();
                        float weight = GetRandomValue(valuesFromParents);
                        TryMutate(ref weight);
                        childWeights[layerIndex][x, y] = weight;
                    }
                }
            }
            child.SetWeights(childWeights);
        }

        public void SetRandomWeights(NeuralNetwork[] networks)
        {
            NeuralNetworkParameters parameters = networks[0].Parameters;
            for (int i = 0; i < networks.Length; i++)
            {
                var weights = new float[parameters.LayersCount - 1][,];
                for (int layerIndex = 0; layerIndex < parameters.LayersCount - 1; layerIndex++)
                {
                    GetNeuronsCountFromLayerIndex(parameters, layerIndex, out int neuronsInChild, out int neuronsCount);

                    for (int x = 0; x < neuronsCount; x++)
                    {
                        for (int y = 0; y < neuronsInChild; y++)
                        {
                            weights[layerIndex][x, y] = GetRandomWeight();
                        }
                    }
                }
                networks[i].SetWeights(weights);
            }
        }

        private void GetNeuronsCountFromLayerIndex(NeuralNetworkParameters parameters, int layerIndex, out int neuronsInChild, out int neuronsCount)
        {
            if (layerIndex == 0)
            {
                neuronsCount = parameters.NeuronsInHidenLayersCount;
                neuronsInChild = parameters.InputsCount;
            }

            else if (layerIndex == parameters.LayersCount - 1)
            {
                neuronsCount = parameters.OutputsCount;
                neuronsInChild = parameters.NeuronsInHidenLayersCount;
            }
            else
            {
                neuronsCount = neuronsInChild = parameters.NeuronsInHidenLayersCount;
            }
        }

        private float GetRandomWeight() 
            => Random.Range(_minWeight, _maxWeight);

        private float GetRandomValue(float[] values)
        {
            float randomValue = Random.Range(0f, 1f);
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
            if (Random.Range(0f, 1f) <= _mutationRate)
                value = GetRandomWeight();
        }
    }
}
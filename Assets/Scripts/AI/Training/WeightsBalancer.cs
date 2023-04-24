using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AI
{
    public class WeightsBalancer
    {
        private const float _minWeight = -1f, _maxWeight = 1f, _mutationRate = 0.05f;

        public void UniformCross(IEnumerable<float[][,]> parentsWeights, NeuralNetwork child)
        {
            var childWeights = child.CopyWeights();
            for (int layerIndex = 0; layerIndex < childWeights.Length; layerIndex++)
            {
                int neuronsCount = childWeights[layerIndex].GetLength(0);
                int neuronsCountInChild = childWeights[layerIndex].GetLength(1);

                for (int x = 0; x < neuronsCount; x++)
                {
                    for (int y = 0; y < neuronsCountInChild; y++)
                    {
                        float[] valuesFromParents = parentsWeights.Select(item => item[layerIndex][x, y]).ToArray();
                        float weight = GetRandomOfValues(valuesFromParents);
                        TryMutate(ref weight);
                        childWeights[layerIndex][x, y] = weight;
                    }
                }
            }
            child.SetWeights(childWeights);
        }

        public void SetRandomWeights(NeuralNetwork networks)
        {
            var weights = networks.CopyWeights();
            for (int layerIndex = 0; layerIndex < weights.Length; layerIndex++)
            {
                int neuronsCount = weights[layerIndex].GetLength(0);
                int neuronsCountInChildLayer = weights[layerIndex].GetLength(1);

                for (int x = 0; x < neuronsCount; x++)
                {
                    for (int y = 0; y < neuronsCountInChildLayer; y++)
                    {
                        weights[layerIndex][x, y] = GetRandomWeight();
                    }
                }
            }
            networks.SetWeights(weights);
        }

        private float GetRandomWeight() 
            => GetRandomValue(_minWeight, _maxWeight);

        private float GetRandomOfValues(float[] values)
        {
            float randomValue = GetRandomValue(0f, 1f);
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

        private float GetRandomValue(float first, float second)
        {
            //int seed = (int)System.DateTime.Now.Ticks;
            //Random.InitState(seed);

            return Random.Range(first, second);
        }
    }
}
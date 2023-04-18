using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class WeightsBalancer
    {
        private const float _minWeight = 0f, _maxWeight = 1f, _mutationRate = 0.01f;

        public void UniformCross(NeuralNetwork[] parents, NeuralNetwork[] children)
        {
            float[][,] fatherWeights = parents[0].CopyWeights();
            float[][,] motherWeights = parents[1].CopyWeights();
            Parameters parameters = parents[0].Parameters;
            for (int childIndex = 0; childIndex < children.Length; childIndex++)
            {
                var childWeights = new float[parameters.LayersCount - 1][,];
                for (int layerIndex = 0; layerIndex < parameters.LayersCount-1; layerIndex++)
                {
                    int neuronsCount = fatherWeights[layerIndex].GetLength(0);
                    int neuronsCountInChild = fatherWeights[layerIndex].GetLength(1);

                    for (int x = 0; x < neuronsCount; x++)
                    {
                        for (int y = 0; y < neuronsCountInChild; y++)
                        {
                            float weight = GetRandomOfTwo(fatherWeights[layerIndex][x, y], motherWeights[layerIndex][x, y]);
                            TryMutate(ref weight);
                            childWeights[layerIndex][x, y] = weight;
                        }
                    }
                }
                children[childIndex].SetWeights(childWeights);
            }
        }

        public void SetRandomWeights(NeuralNetwork[] networks)
        {
            Parameters parameters = networks[0].Parameters;
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

        private void GetNeuronsCountFromLayerIndex(Parameters parameters, int layerIndex, out int neuronsInChild, out int neuronsCount)
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

        private float GetRandomOfTwo(float first, float second) 
            => Random.Range(0f, 1f) > 0.5 ? first : second;

        private void TryMutate(ref float value)
        {
            if (Random.Range(0f, 1f) <= _mutationRate)
                value = GetRandomWeight();
        }
    }
}
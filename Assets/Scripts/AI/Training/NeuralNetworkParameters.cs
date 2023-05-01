using System.Collections.Generic;

namespace AI
{
    public struct NeuralNetworkParameters
    {
        public readonly IReadOnlyList<int> NeuronsInLayerCount;
        public int LayersCount => NeuronsInLayerCount.Count;
        public NeuralNetworkParameters(int[] neuronsInLayerCount)
        {
            NeuronsInLayerCount = neuronsInLayerCount;
        }
    }
}
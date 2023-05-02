using System.Collections.Generic;

namespace AI
{
    public struct NeuralNetworkParameters
    {
        public readonly IReadOnlyList<int> LayersSize;
        public int LayersCount => LayersSize.Count;
        public NeuralNetworkParameters(int[] neuronsInLayerCount)
        {
            LayersSize = neuronsInLayerCount;
        }
    }
}
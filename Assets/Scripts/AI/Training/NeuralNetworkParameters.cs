namespace AI
{
    public struct NeuralNetworkParameters
    {
        public readonly int InputsCount, OutputsCount, LayersCount, NeuronsInHidenLayersCount;
        public NeuralNetworkParameters(int inputsCount, int outputsCount, int layersCount, int neuronsInHidenLayersCount)
        {
            InputsCount = inputsCount;
            OutputsCount = outputsCount;
            LayersCount = layersCount;
            NeuronsInHidenLayersCount = neuronsInHidenLayersCount;
        }
    }
}
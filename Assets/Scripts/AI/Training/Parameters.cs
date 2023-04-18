public struct Parameters
{
    public readonly int InputsCount, OutputsCount, LayersCount, NeuronsInHidenLayersCount;
    public Parameters(int inputsCount, int outputsCount, int layersCount, int neuronsInHidenLayersCount)
    {
        InputsCount = inputsCount;
        OutputsCount = outputsCount;
        LayersCount = layersCount;
        NeuronsInHidenLayersCount = neuronsInHidenLayersCount;
    }
}

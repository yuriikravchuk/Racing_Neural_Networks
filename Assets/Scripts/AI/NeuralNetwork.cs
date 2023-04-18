using System;

namespace AI
{
    public class NeuralNetwork
    {
        public readonly Parameters Parameters;
        private readonly Layer[] _layers;

        public NeuralNetwork(Parameters parameters)
        {
            Parameters = parameters;
            _layers = new Layer[Parameters.LayersCount];
            _layers[0] = new Layer(Parameters.InputsCount);

            for (int i = 1; i < Parameters.LayersCount - 1;)
                _layers[i] = new Layer(Parameters.NeuronsInHidenLayersCount, _layers[i - 1]);

            var output = new Layer(Parameters.OutputsCount, _layers[Parameters.LayersCount - 2]);

            _layers[Parameters.LayersCount - 1] = output;
        }

        public void GetOutputs(float[] inputs)
        {
            if (inputs.Length != _layers[0].NeuronsCount)
                throw new ArgumentOutOfRangeException();

            for (int i = 0; i < _layers[0].NeuronsCount; i++)
                _layers[0].Values[i] = inputs[i];

            for (int i = 1; i < Parameters.LayersCount; i++)
                _layers[i].Update();
        }

        public float[][,] CopyWeights()
        {
            var weights = new float[Parameters.LayersCount - 1][,];
            for (int i = 1; i < _layers.Length; i++)
            {
                float[,] weightsInLayer = (float[,])_layers[i].Weights.Clone();
                weights[i - 1] = weightsInLayer;
            }
            return weights;
        }

        public void SetWeights(float[][,] weights)
        {
            for (int i = 0; i < Parameters.LayersCount - 1;)
                _layers[i + 1].Weights = weights[i];
        }
    }
}
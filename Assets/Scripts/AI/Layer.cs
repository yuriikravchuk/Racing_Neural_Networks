using System;

namespace AI
{
    public class Layer
    {
        public int NeuronsCount => _neuronsCount;
        public float[] Values { get; private set; }
        public float[,] Weights;

        private readonly Layer _child;
        private readonly int _neuronsCount;
        private readonly int _neuronsInChildCount;

        public Layer(int neyronsCount, Layer child = null)
        {
            _neuronsCount = neyronsCount;
            Values = new float[_neuronsCount];
            _child = child;
            if (_child != null)
            {
                Weights = new float[_neuronsCount, _child.NeuronsCount];
                _neuronsInChildCount = Weights.GetLength(1);
            }
        }

        public Layer(float[,] weights, Layer child) : this(weights.GetLength(0), child)
            => Weights = weights;

        public void Update()
        {
            if (_child == null)
                return;

            for (int i = 0; i < _neuronsCount; i++)
            {
                float input = 0f;
                for (int j = 0; j < _neuronsInChildCount; j++)
                {
                    input += _child.Values[j] * Weights[i, j];
                }
                Values[i] = Sigmoid(input);
            }
        }

        private float Sigmoid(float x)
            => 1.0f / (1.0f + (float)Math.Exp(-x));
    }
}
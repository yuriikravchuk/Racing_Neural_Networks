using System.Collections.Generic;

namespace AI
{
    public class InputLayer : Layer
    {
        public InputLayer(int neyronsCount) : base(neyronsCount) { }

        public override void Update(IReadOnlyList<float> values)
        {
            for (int i = 0; i < NeuronsCount; i++)
                SetValueToNeuron(i, values[i]);
        }
    }
}
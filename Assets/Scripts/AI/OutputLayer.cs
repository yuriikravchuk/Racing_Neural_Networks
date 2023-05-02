namespace AI
{
    public class OutputLayer : Layer
    {
        public OutputLayer(int neyronsCount, int childNeuronsCount) : base(neyronsCount, childNeuronsCount) { }

        protected override float ActivationFunction(float x) => Tanh(x);
    }
}
namespace AI
{
    public struct TrainingResults
    {
        public readonly float[][,] Weights;
        public readonly int Score;

        public TrainingResults(float[][,] weights, int score)
        {
            Weights = weights;
            Score = score;
        }
    }
}
using System.Collections.Generic;
using AI;

public interface IEnemiesHandler
{
    IReadOnlyList<TrainingResults> Results { get; }
    void SetResults(IEnumerable<TrainingResults> results);
}
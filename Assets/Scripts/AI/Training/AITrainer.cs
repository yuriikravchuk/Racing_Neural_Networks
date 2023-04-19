using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;

public class AITrainer : MonoBehaviour
{
    private IReadOnlyList<NeuralNetwork> _networks;

    private List<NeuralNetwork> _bestNetworks;
    public void Init(List<NeuralNetwork> bestAI)
    {
        _bestNetworks = bestAI == null ? new List<NeuralNetwork>() : bestAI;
    }

    private void GetBestAI()
    {
        int[] bestScores = new int[3];

        foreach (var network in _networks)
        {

        }
    }
}

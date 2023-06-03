using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    
    public void UpdateScores(IReadOnlyList<float> scores)
    {
        _text.text = "Fitness: \n";

        for (int i = 0; i < scores.Count; i++)
        {
            _text.text += $"{i}) {scores[i]}\n";
        }
    }
}

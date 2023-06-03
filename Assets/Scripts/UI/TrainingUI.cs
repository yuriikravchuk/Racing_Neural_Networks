using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TrainingUI : MonoBehaviour
{
    [SerializeField] private Button _saveButton;
    [SerializeField] private Button _loadButton;
    [SerializeField] private Button _startTrainingButton;

    public event UnityAction SaveButtonClicked
    {
        add => _saveButton.onClick.AddListener(value); 
        remove => _saveButton.onClick.RemoveListener(value);
    }

    public event UnityAction LoadButtonClicked
    {
        add => _loadButton.onClick.AddListener(value); 
        remove  => _loadButton.onClick.RemoveListener(value); 
    }

    public event UnityAction StartTrainingButtonClicked
    {
        add => _startTrainingButton.onClick.AddListener(value); 
        remove => _startTrainingButton.onClick.RemoveListener(value); 
    }


}
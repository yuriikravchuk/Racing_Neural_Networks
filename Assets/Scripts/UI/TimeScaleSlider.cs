using UnityEngine;
using TMPro;
using System;

public class TimeScaleSlider : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    public void UpdateText(float value)
    {
        value = (float)Math.Round(value, 2);
        _text.text = "TimeScale: " + value.ToString();
        Time.timeScale = value;
    }
}

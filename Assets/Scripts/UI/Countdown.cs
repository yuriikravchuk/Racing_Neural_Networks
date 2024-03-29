using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Countdown : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private float _countdownTime = 3f;
    public event Action Counted;

    private float _currentTime;

    private void OnEnable() => _currentTime = _countdownTime;
    public IEnumerator CountToZero()
    {
        while (_currentTime > 0)
        {
            float deltaTime = Time.deltaTime;
            _currentTime = Mathf.Max(_currentTime - deltaTime, 0);
            _text.text = ((int)_currentTime + 1).ToString();
            yield return null;
        }

        Counted?.Invoke();
        gameObject.SetActive(false);
        yield break;
    }
}

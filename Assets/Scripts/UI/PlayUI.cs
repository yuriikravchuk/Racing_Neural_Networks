using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _playerSpeed;
    [SerializeField] private Countdown _countdown;
    [SerializeField] private GameObject _goText;
    [SerializeField] private Button _playButton;

    public event UnityAction PlayButtonClicked
    {
        add => _playButton.onClick.AddListener(value);
        remove => _playButton.onClick.RemoveListener(value);
    }

    public event Action ÑountdownCompleted
    {
        add => _countdown.Counted += value;
        remove => _countdown.Counted -= value;
    }


    public void SetPlayerSpeed(int value) => _playerSpeed.text = $"Speed: {value * 2}";

    public void ShowPlaySpeed() => _playerSpeed.gameObject.SetActive(true);

    public void HidePlaySpeed() => _playerSpeed.gameObject.SetActive(false);

    public void ShowCountingTime() => _countdown.gameObject.SetActive(true);

    public void HideCountingTime() => _countdown.gameObject.SetActive(false);

    public void ShowGoText() => _goText.SetActive(true);

    public void StartCountdown()
    {
        _countdown.gameObject.SetActive(true);
        StartCoroutine(_countdown.CountToZero());
    }

    public void ShowPlayButton() => _playButton.gameObject.SetActive(true);

    public void HidePlayButton() => _playButton.gameObject.SetActive(false);
}

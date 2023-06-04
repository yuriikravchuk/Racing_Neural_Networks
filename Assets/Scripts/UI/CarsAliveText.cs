using TMPro;
using UnityEngine;

public class CarsAliveText : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    public void OnCarsAliveCountChanged(int count) => _text.text = $"Cars alive: {count}";
}

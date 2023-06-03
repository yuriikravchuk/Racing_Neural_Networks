using System.Collections.Generic;
using UnityEngine;

public class MapsHandler : MonoBehaviour
{
    [SerializeField] private List<Map> _maps;

    public Map CurrentMap => _maps[_currentIndex];

    private int _currentIndex = 0;

    public void SwitchMap()
    {
        int nextIndex = _currentIndex == _maps.Count - 1 ? 0 : _currentIndex + 1;
        _maps[_currentIndex].gameObject.SetActive(false);
        _maps[nextIndex].gameObject.SetActive(true);
        _currentIndex = nextIndex;
    }
}

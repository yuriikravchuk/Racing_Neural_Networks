using System.Collections.Generic;
using UnityEngine;

public class MapsHandler : MonoBehaviour
{
    [SerializeField] private List<Map> _maps;

    private int _currentIndex = 0;

    public Map GetNextMap()
    {
        int lastIndex = _maps.Count - 1;
        int nextIndex = _currentIndex == lastIndex ? 0 : _currentIndex + 1;
        _maps[_currentIndex].gameObject.SetActive(false);
        _maps[nextIndex].gameObject.SetActive(true);
        _currentIndex = nextIndex;
        return _maps[_currentIndex];
    }
}

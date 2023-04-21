using UnityEngine;

public class CarView : MonoBehaviour
{
    [SerializeField] private Material _playerMaterial;
    [SerializeField] private Material _enemyMaterial;
    [SerializeField] private MeshRenderer _renderer;

    public void SetPlayerColor() => _renderer.materials[0] = _playerMaterial;

    public void SetEnemyColor() => _renderer.materials[0] = _enemyMaterial;
}

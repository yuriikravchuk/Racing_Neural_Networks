using UnityEngine;

public class CarView : MonoBehaviour
{
    [SerializeField] private MeshRenderer _renderer;

    public void SetColor(Color color) => _renderer.materials[0].color = color;

}

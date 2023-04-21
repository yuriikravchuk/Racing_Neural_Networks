using pool;
using UnityEngine;
using AI;

public class TrainingSceneStarter : MonoBehaviour
{
    [SerializeField] private Enemy enemyPrefab;
    private Pool<Enemy> enemyProvider;
    private void Awake()
    {
        //enemyProvider = new Pool<Enemy>(enemyPrefab);
    }
}

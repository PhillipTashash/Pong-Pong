using UnityEngine;

public class SpawnBall : MonoBehaviour
{
    [SerializeField] private GameObject ballPrefab;

    void Start()
    {
        SpawnNewBall();
    }

    public void SpawnNewBall()
    {
        Instantiate(ballPrefab);
    }
}

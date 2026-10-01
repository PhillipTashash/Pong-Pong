using UnityEngine;

public class SpawnBall : MonoBehaviour
{
    public GameObject ballPrefab;
    void Start()
    {
        Instantiate(ballPrefab);
    }
}

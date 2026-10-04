using UnityEngine;

public class BallScore : MonoBehaviour
{
    //TODO Something is fucky about the score system. Play test this somemore and be away that is not working 100%.
    private SpawnBall ballSpawner;
    private scoreKeeper scoreTracker;
    [SerializeField] private string tagNameOfLeftScore = "LeftScore";
    [SerializeField] private string tagNameOfRightScore = "RightScore";

    void Start()
    {
        ballSpawner = FindFirstObjectByType<SpawnBall>();
        scoreTracker = FindFirstObjectByType<scoreKeeper>();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag(tagNameOfLeftScore))
        {
            Score(true);
        }else if (col.gameObject.CompareTag(tagNameOfRightScore))
        {
            Score(false);
        }
    }

    void Score(bool isLeftScore)
    {
        if (scoreTracker == null)
        {
            Debug.LogError("No scoreKeeper component was found in the scene.", this);
            return;
        }

        scoreTracker.AddScore(isLeftScore);
        DestroyBall();
        ballSpawner.SpawnNewBall();
    }

    void DestroyBall()
    {
        Destroy(gameObject);
    }
}

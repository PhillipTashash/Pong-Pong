using UnityEngine;

public class BallScore : MonoBehaviour
{
    private SpawnBall ballSpawner;

    int leftScore = 0;
    int rightScore = 0;
    [SerializeField] private int pointsPerScore = 1;
    [SerializeField] private int pointsToWin = 5;
    [SerializeField] private string tagNameOfLeftScore = "LeftScore";
    [SerializeField] private string tagNameOfRightScore = "RightScore";

    void Start()
    {
        ballSpawner = FindFirstObjectByType<SpawnBall>();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag(tagNameOfLeftScore))
        {
            Score(true);
            Destroy(gameObject);
        }else if (col.gameObject.CompareTag(tagNameOfRightScore))
        {
            Score(false);
            Destroy(gameObject);
        }
    }

    void Score(bool isLeftScore)
    {
        if (isLeftScore)
        {
            leftScore += pointsPerScore;
            Debug.Log("Left Score: " + leftScore);
        }
        else
        {
            rightScore += pointsPerScore;
            Debug.Log("Right Score: " + rightScore);
        }

        ballSpawner.SpawnNewBall();
        Score(false);
    }
}

using UnityEngine;

public class scoreKeeper : MonoBehaviour
{
    [SerializeField] private int leftScore = 0;
    [SerializeField] private int rightScore = 0;
    [SerializeField] private int pointsToAdd = 1;
    [SerializeField] private int pointsToWin = 5;

    public void AddScore(bool isLeftScore)
    {
        if (isLeftScore)
        {
            leftScore += pointsToAdd;
            Debug.Log("Left Score: " + leftScore);
        }
        else
        {
            rightScore += pointsToAdd;
            Debug.Log("Right Score: " + rightScore);
        }
    }
}

using UnityEngine;
using TMPro;
using System.Collections;

public class scoreKeeper : MonoBehaviour
{
    private static WaitForSeconds _waitForSeconds3 = new WaitForSeconds(3f);
    [SerializeField] private int leftScore = 0;
    [SerializeField] private int rightScore = 0;
    [SerializeField] private int pointsToAdd = 1;
    [SerializeField] private int pointsToWin = 5;
    [SerializeField] private string leftPlayerName = "Left Player";
    [SerializeField] private string rightPlayerName = "Right Player";

    [SerializeField] private TMP_Text leftScoreText;
    [SerializeField] private TMP_Text rightScoreText;
    [SerializeField] private TMP_Text winnerPopup;

    public void AddScore(bool isLeftScore)
    {

        AudioManager.Instance.PlayGoal();

        if (isLeftScore)
        {
            leftScore += pointsToAdd;
            UpdateScoreText();
            Debug.Log("Left Score: " + leftScore);
            CheckWinCondition();
        }
        else
        {
            rightScore += pointsToAdd;
            UpdateScoreText();
            Debug.Log("Right Score: " + rightScore);
            CheckWinCondition();
        }
    }

    private void CheckWinCondition()
    {
        if (leftScore >= pointsToWin)
        {
            Debug.Log("Left Player Wins!");
            ResetScore();
            StartCoroutine(ShowWinnerPopupCoroutine(1));            
        }
        else if (rightScore >= pointsToWin)
        {
            Debug.Log("Right Player Wins!");
            ResetScore();
            StartCoroutine(ShowWinnerPopupCoroutine(2));
        }
    }

    private void ResetScore()
    {
        leftScore = 0;
        rightScore = 0;
    }

    private void UpdateScoreText()
    {

        if (leftScoreText != null)
        {
            leftScoreText.text = leftScore.ToString();
        }
        else
        {
            Debug.LogWarning("Left score text is not assigned.");
        }

        if (rightScoreText != null)
        {
            rightScoreText.text = rightScore.ToString();
        }
        else
        {
            Debug.LogWarning("Right score text is not assigned.");
        }
    }

    private IEnumerator ShowWinnerPopupCoroutine(int winner)
    {
        if (winnerPopup != null)
        {
            if (winner == 1)
            {
                winnerPopup.text = leftPlayerName + " Wins!";
                winnerPopup.gameObject.SetActive(true);
            }
            else if (winner == 2)
            {
                winnerPopup.text = rightPlayerName + " Wins!";
                winnerPopup.gameObject.SetActive(true);
            }
        }
        else
        {
            Debug.LogWarning("Winner popup text is not assigned.");
        }

        yield return _waitForSeconds3; // Display the popup for 3 seconds

        if (winnerPopup != null)
        {
            winnerPopup.gameObject.SetActive(false);
        }
    }

}

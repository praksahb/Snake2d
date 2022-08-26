using UnityEngine;
using TMPro;

public class ScoreController : MonoBehaviour
{
    private TextMeshProUGUI scoreTracker;
    private int score = 0;

    private void Awake()
    {
        Debug.Log(score);
        scoreTracker = gameObject.GetComponent<TextMeshProUGUI>();
    }

    public void IncrementScore(int value)
    {
        score += value;
        UpdateScore();
    }

    public void DecrementScore(int value)
    {
        score -= value;
        UpdateScore();
    }

    public void UpdateScore()
    {
        scoreTracker.SetText("Score: {0}", score);
    }
}

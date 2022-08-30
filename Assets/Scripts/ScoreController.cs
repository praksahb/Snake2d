using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    private TextMeshProUGUI scoreTracker;
    private int score;

    private void Awake()
    {
        score = 0;
        scoreTracker = gameObject.GetComponent<TextMeshProUGUI>();
    }

    public void ScoreUpdater(int value)
    {
        score += value;
        UpdateScore();
    }

    public void UpdateScore()
    {
        scoreTracker.SetText("Score: {0}", score);
    }

    public int GetScore()
    {
        return score;
    }
}

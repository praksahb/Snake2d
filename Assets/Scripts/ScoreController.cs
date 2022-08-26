using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    private TextMeshProUGUI scoreTracker;
    private int score = 0;

    private void Awake()
    {
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
}

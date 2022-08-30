using TMPro;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject gameOverMenu;

    public TextMeshProUGUI currentScore;
    public TextMeshProUGUI highScore;

    public PlayerController playerController;
    public ScoreController scoreController;

    private int cScore;
    private int hScore;

    private bool IsPaused;
    private bool IsGameOver = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            IsPaused = !IsPaused;
            TogglePauseMenu();
        }

        if (IsPaused && Input.GetKeyDown(KeyCode.Q))
            ExitGame();

        if(IsGameOver)
        {
            if(Input.GetKeyDown(KeyCode.Q))
                ExitGame();
            if (Input.GetKeyDown(KeyCode.R))
            {
                RestartGame();
                Time.timeScale = 1;
            }
        }
    }

    private void TogglePauseMenu()
    {
        if (IsPaused)
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
        if (!IsPaused)
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void ToggleGameOverMenu()
    {
        gameOverMenu.SetActive(true);
        IsGameOver = true;
        Time.timeScale = 0;

        //get current score
        GetCurrentScore();

        // get high score
        GetHighScore();

        //compare cScore and hScore
        CompareUpdateScore();

        UpdateHighScoreUI();
    }

    private void GetCurrentScore()
    {
        cScore = scoreController.GetScore();
        UpdateCurrentScoreUI();
    }

    private void UpdateCurrentScoreUI()
    {
        currentScore.SetText("Your Score: {0}", cScore);
    }

    private void GetHighScore()
    {
        hScore = PlayerPrefs.GetInt("HighScore");
    }

    private void UpdateHighScoreUI()
    {
        highScore.SetText("High Score: {0}", hScore);
    }

    private void CompareUpdateScore()
    {
        if (cScore > hScore)
        {
            PlayerPrefs.SetInt("HighScore", cScore);
            GetHighScore();
        }
    }

    private void ExitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }

    private void RestartGame()
    {
        playerController.ReloadLevel();
    }
}

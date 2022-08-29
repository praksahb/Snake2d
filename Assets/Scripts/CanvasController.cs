using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    public GameObject pauseMenu;

    private bool IsPaused;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            IsPaused = !IsPaused;
            TogglePauseMenu();
        }

        if (IsPaused && Input.GetKeyDown(KeyCode.Q))
            ExitGame();
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

    private void ExitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }
}

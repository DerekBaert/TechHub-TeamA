using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        Debug.Log("Time Scale is now ZERO"); // Check your console when you pause!
    }

    public void Home()
    {
        SceneManager.LoadScene("Main Menu");
        Time.timeScale = 1;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }
    void Update()
{
    if (Input.GetKeyDown(KeyCode.Escape))
    {
        if (pauseMenu.activeSelf)
        {
            Resume();
        }
        else
        {
            // Don't allow pausing if the player is already dead
            if (LevelManager.instance != null && !LevelManager.instance.isGameOver)
            {
                Pause();
            }
        }
    }
}
}

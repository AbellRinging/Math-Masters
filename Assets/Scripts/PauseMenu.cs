using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;

    
    public GameObject ResumeButton;
    public GameObject ReturnButton;
    public GameObject RestartButton;
    public GameObject CustomizeButton;
    public GameObject AboutMeButton;

    [HideInInspector] public bool GameIsPaused = false;

    private bool PlayerDied = false;
    private PlayerMainScript MainScript;
    private int CurrentScene;
    public void SpecifyPauseMenu(PlayerMainScript Main, int currentScene)
    {
        MainScript = Main;
        CurrentScene = currentScene;
        if(CurrentScene != 1)
        {
            ReturnButton.SetActive(true);
            RestartButton.SetActive(true);
            return;
        }
        CustomizeButton.SetActive(true);
        AboutMeButton.SetActive(true);
    }

    void Update()
    {
        if(!PlayerDied && Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause(false);
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause(bool DidPlayerDie) // Not a button
    {
        PlayerDied = DidPlayerDie;
        
        pauseMenuUI.SetActive(true);
        if (PlayerDied)
        {
            ResumeButton.SetActive(false);
        }

        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void GoToSamosTown()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SamosTown");
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(CurrentScene);
    }

    public void Customize()
    {

    }

    public void AboutMe()
    {

    }

    public void QuitGame()
    {
        Debug.Log("Quitting game");
    }
}

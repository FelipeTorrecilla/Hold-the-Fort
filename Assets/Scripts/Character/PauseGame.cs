using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PauseGame : MonoBehaviour
{
    public GameObject pausePanel;
    public CharacterController characterController;
    private bool isPaused = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                Pause();
            }
        }
    }

    private void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f; // Pause the game
        pausePanel.SetActive(true); // Activate the pause panel
        characterController.enabled = false; // Disable the CharacterController

        DisableScriptsOnChildrenObjects(characterController.gameObject, false);
    }

    private void DisableScriptsOnChildrenObjects(GameObject parentObject, bool state)
    {
        MonoBehaviour[] scripts = parentObject.GetComponentsInChildren<MonoBehaviour>();

        foreach (MonoBehaviour script in scripts)
        {
            if (script != this) // Exclude the PauseGame script itself
            {
                script.enabled = state;
            }
        }
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // Resume the game
        pausePanel.SetActive(false); // Deactivate the pause panel
        characterController.enabled = true; // Enable the CharacterController

        DisableScriptsOnChildrenObjects(characterController.gameObject, true);
    }
    
    public void MainMenu()
    {
        ResumeGame();
        SceneManager.LoadScene("MainMenuScene");
    }
    
    public void Quit()
    {
        ResumeGame();
        Application.Quit();
    }
}





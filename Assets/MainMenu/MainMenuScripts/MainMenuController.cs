using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject mapSelectPanel;
    public GameObject settingsPanel;
    public GameObject creditsPanel;

    public Button selectMapButton;
    public Button settingsButton;
    public Button creditsButton;
    public Button quitButton;

    [SerializeField] private GameObject currentPanel;

    private void Start()
    {
        // Attach button click listeners
        selectMapButton.onClick.AddListener(OnSelectMapButtonClick);
        settingsButton.onClick.AddListener(OnSettingsButtonClick);
        creditsButton.onClick.AddListener(OnCreditsButtonClick);
        quitButton.onClick.AddListener(OnQuitButtonClick);
    }

    private void OnSelectMapButtonClick()
    {
        DisableCurrentPanel();
        mapSelectPanel.SetActive(true);
    }

    private void OnSettingsButtonClick()
    {
        DisableCurrentPanel();
        settingsPanel.SetActive(true);
    }

    private void OnCreditsButtonClick()
    {
        DisableCurrentPanel();
        creditsPanel.SetActive(true);
    }

    private void OnQuitButtonClick()
    {
        Application.Quit();
    }

    private void DisableCurrentPanel()
    {
        currentPanel.SetActive(false);
    }

    public void GoToMainMenuPanel()
    {
        mainMenuPanel.SetActive(true);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
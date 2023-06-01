using UnityEngine;
using UnityEngine.UI;

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
        mainMenuPanel.SetActive(false);
        mapSelectPanel.SetActive(true);
    }

    private void OnSettingsButtonClick()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    private void OnCreditsButtonClick()
    {
        mainMenuPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }

    private void OnQuitButtonClick()
    {
        Application.Quit();
    }
}


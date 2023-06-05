using UnityEngine;
using UnityEngine.UI;

public class BackButton : MonoBehaviour
{
    public MainMenuController mainMenuController;
    [SerializeField]private GameObject currentPanel;
    [SerializeField] private Button backButton;

    private void Start()
    {
        backButton.onClick.AddListener(OnBackButtonClick);
    }

    private void OnBackButtonClick()
    {
        currentPanel.SetActive(false);
        mainMenuController.GoToMainMenuPanel();
    }
}
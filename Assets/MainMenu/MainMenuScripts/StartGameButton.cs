using UnityEngine;
using UnityEngine.UI;

public class StartGameButton : MonoBehaviour
{
    public MainMenuController mainMenuController;

    private Button startGameButton;

    private void Start()
    {
        startGameButton = GetComponent<Button>();
        startGameButton.onClick.AddListener(StartGameButtonClick);
    }

    private void StartGameButtonClick()
    {
        mainMenuController.StartGame();
    }
}


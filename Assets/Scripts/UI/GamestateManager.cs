using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{
    [SerializeField] private GameObject winScreenPanel;
    [SerializeField] private GameObject failScreenPanel;

    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;

    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private StartRoomExitTrigger winTrigger;

    private void Start()
    {
        winScreenPanel.SetActive(false);
        failScreenPanel.SetActive(false);

        restartButton.onClick.AddListener(Restart);
        mainMenuButton.onClick.AddListener(GoToMainMenu);

        playerHealth.OnDeath += ShowFailScreen;
        winTrigger.OnWin += ShowWinScreen;

        Debug.Log("GameStateManager: Start() fertig, winTrigger = " + winTrigger.name + " (InstanceID " + winTrigger.GetInstanceID() + ")");
    }

    private void ShowWinScreen()
    {
        Debug.Log("GameStateManager.ShowWinScreen() wurde aufgerufen");
        winScreenPanel.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void ShowFailScreen()
    {
        failScreenPanel.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
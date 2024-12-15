using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private Text scoreText;
    [SerializeField] private Text livesText;
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject gameplayPanel;
    [SerializeField] private GameObject gameOverPanel;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Suscribirse a eventos
        GameManager.Instance.OnScoreUpdated += UpdateScoreDisplay;
        GameManager.Instance.OnLivesUpdated += UpdateLivesDisplay;
        GameManager.Instance.OnGameStateChanged += HandleGameStateChange;
    }

    private void UpdateScoreDisplay(int score)
    {
        scoreText.text = "Score: " + score;
    }

    private void UpdateLivesDisplay(int lives)
    {
        livesText.text = "Lives: " + lives;
    }

    private void HandleGameStateChange(GameManager.GameState state)
    {
        switch (state)
        {
            case GameManager.GameState.MainMenu:
                mainMenuPanel.SetActive(true);
                gameplayPanel.SetActive(false);
                gameOverPanel.SetActive(false);
                break;
            case GameManager.GameState.Playing:
                mainMenuPanel.SetActive(false);
                gameplayPanel.SetActive(true);
                gameOverPanel.SetActive(false);
                break;
            case GameManager.GameState.GameOver:
                mainMenuPanel.SetActive(false);
                gameplayPanel.SetActive(false);
                gameOverPanel.SetActive(true);
                break;
        }
    }

    public void StartGameButtonClicked()
    {
        GameManager.Instance.StartGame();
    }

    public void RestartGameButtonClicked()
    {
        GameManager.Instance.ResetGame();
        GameManager.Instance.StartGame();
    }
}

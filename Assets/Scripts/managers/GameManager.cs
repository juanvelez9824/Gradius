using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public enum GameState { MainMenu, Playing, Paused, GameOver }
    public GameState CurrentState { get; private set; }
    
    [SerializeField] private int initialLives = 3;
    public int Lives { get; private set; }
    public int Score { get; private set; }
    public int CurrentLevel { get; private set; } = 1;

    public event Action<GameState> OnGameStateChanged;
    public event Action<int> OnScoreUpdated;
    public event Action<int> OnLivesUpdated;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeGame();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeGame()
    {
        ResetGame();
    }

    public void ResetGame()
    {
        Score = 0;
        Lives = initialLives;
        CurrentLevel = 1;
        ChangeGameState(GameState.MainMenu);
    }

    public void UpdateScore(int points)
    {
        Score += points;
        OnScoreUpdated?.Invoke(Score);
    }

    public void LoseLife()
    {
        Lives--;
        OnLivesUpdated?.Invoke(Lives);

        if (Lives <= 0)
        {
            ChangeGameState(GameState.GameOver);
        }
    }

    public void ChangeGameState(GameState newState)
    {
        CurrentState = newState;
        OnGameStateChanged?.Invoke(newState);
    }

    public void StartGame()
    {
        ChangeGameState(GameState.Playing);
    }
}

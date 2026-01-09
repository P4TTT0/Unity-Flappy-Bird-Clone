using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FlappyBird.Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public GameState CurrentState { get; private set; } = GameState.Menu;
        public int Score { get; private set; } = 0;

        public event Action<GameState> OnGameStateChanged;
        public event Action<int> OnScoreChanged;

        private void Awake()
        {
            if (Instance is not null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            SetState(GameState.Playing);
        }

        public void StartGame()
        {
            Score = 0;
            OnScoreChanged?.Invoke(Score);
            SetState(GameState.Playing);
        }

        public void GameOver()
        {
            SetState(GameState.GameOver);
        }

        public void AddScore(int amount = 1)
        {
            if (CurrentState != GameState.Playing)
                return;

            Score += amount;
            OnScoreChanged?.Invoke(Score);
        }

        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void SetState(GameState newState)
        {
            if (CurrentState == newState)
                return;

            CurrentState = newState;
            OnGameStateChanged?.Invoke(CurrentState);
        }
    }
}



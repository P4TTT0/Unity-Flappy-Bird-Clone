using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FlappyBird.Core
{
    public class GameManager : MonoBehaviour
    {
        private const string HIGH_SCORE_KEY = "HIGH_SCORE";

        public static GameManager Instance { get; private set; }
        public GameState CurrentState { get; private set; } = GameState.Menu;
        public int Score { get; private set; }
        public int HighScore { get; private set; }

        public event Action<GameState> OnGameStateChanged;
        public event Action<int> OnScoreChanged;

        private void Awake()
        {
            Instance = this;
            LoadHighScore();
        }

        private void Start()
        {
            SetState(GameState.Menu);
        }

        private void LoadHighScore()
        {
            HighScore = PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0);
        }

        private void SaveHighScore()
        {
            PlayerPrefs.SetInt(HIGH_SCORE_KEY, HighScore);
            PlayerPrefs.Save();
        }

        public void StartGame()
        {
            Score = 0;
            OnScoreChanged?.Invoke(Score);
            SetState(GameState.Playing);
        }

        public void GameOver()
        {
            if (Score > HighScore)
            {
                HighScore = Score;
                SaveHighScore();
            }

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



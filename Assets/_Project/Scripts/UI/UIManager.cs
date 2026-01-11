using FlappyBird.Core;
using System.Collections;
using UnityEngine;

namespace FlappyBird.UI
{
    public class UIManager : MonoBehaviour
    {
        [Header("Panels")]
        [SerializeField] private GameObject _menuPanel;
        [SerializeField] private GameObject _hudPanel;
        [SerializeField] private GameObject _gameOverPanel;

        [Header("Game Over Animation")]
        [SerializeField] private float _gameOverDelay = 0.8f;
        [SerializeField] private float _slideDuration = 0.4f;

        [Header("Game Over Information")]
        [SerializeField] private DigitDisplay _scoreDisplay;
        [SerializeField] private DigitDisplay _bestDisplay;

        private RectTransform _gameOverRect;
        private Vector2 _gameOverHiddenPos;
        private Vector2 _gameOverVisiblePos;

        private void Awake()
        {
            _gameOverRect = _gameOverPanel.GetComponent<RectTransform>();
            _gameOverVisiblePos = Vector2.zero;
            _gameOverHiddenPos = new Vector2(0, -Screen.height);
        }

        private void Start()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
                HandleGameStateChanged(GameManager.Instance.CurrentState);
            }
        }

        private void OnDisable()
        {
            if (GameManager.Instance != null)
                GameManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
        }

        private void HandleGameStateChanged(GameState state)
        {
            _menuPanel.SetActive(state == GameState.Menu);
            _hudPanel.SetActive(state == GameState.Playing);

            if (state == GameState.GameOver)
            {
                _scoreDisplay.SetNumber(GameManager.Instance.Score);
                _bestDisplay.SetNumber(GameManager.Instance.HighScore);

                StartCoroutine(ShowGameOverWithDelay());
            }
            else
            {
                _gameOverPanel.SetActive(false);
                _gameOverRect.anchoredPosition = _gameOverHiddenPos;
            }
        }

        private IEnumerator ShowGameOverWithDelay()
        {
            yield return new WaitForSeconds(_gameOverDelay);

            _gameOverPanel.SetActive(true);
            yield return SlideInGameOver();
        }

        public void StartGame()
        {
            if (GameManager.Instance != null) 
                GameManager.Instance.StartGame();
        }

        public void RestartGame()
        {
            if (GameManager.Instance != null)
                GameManager.Instance.RestartGame();
        }

        private IEnumerator SlideInGameOver()
        {
            float elapsed = 0f;

            while (elapsed < _slideDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / _slideDuration;

                _gameOverRect.anchoredPosition =
                    Vector2.Lerp(_gameOverHiddenPos, _gameOverVisiblePos, t);

                yield return null;
            }

            _gameOverRect.anchoredPosition = _gameOverVisiblePos;
        }
    }
}

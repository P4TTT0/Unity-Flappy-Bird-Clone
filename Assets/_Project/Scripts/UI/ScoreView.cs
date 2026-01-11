using FlappyBird.Core;
using UnityEngine;
using UnityEngine.UI;

namespace FlappyBird.UI
{
    public class ScoreView : MonoBehaviour
    {
        [Header("Setup")]
        [SerializeField] private Image _digitPrefab;
        [SerializeField] private Sprite[] _digitSprites; 

        private void Start()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnScoreChanged += UpdateScore;
                UpdateScore(GameManager.Instance.Score);
            }
        }

        private void OnDestroy()
        {
            if (GameManager.Instance != null)
                GameManager.Instance.OnScoreChanged -= UpdateScore;
        }

        private void UpdateScore(int score)
        {
            Clear();

            string scoreText = score.ToString();
            foreach (char c in scoreText)
            {
                // Aca estamos restando ASCII '0' para obtener el valor numérico del dígito.
                // '0' = 48, '1' = 49, ..., '9' = 57
                // Por ejemplo, '3' - '0' = 51 - 48 = 3
                int digit = c - '0';
                CreateDigit(digit);
            }
        }

        private void CreateDigit(int digit)
        {
            Image img = Instantiate(_digitPrefab, transform);
            img.sprite = _digitSprites[digit];
        }

        private void Clear()
        {
            foreach (Transform child in transform)
                Destroy(child.gameObject);
        }
    }
}

using FlappyBird.Audio;
using FlappyBird.Core;
using UnityEngine;

namespace FlappyBird.Obstacles
{
    public class ScoreZone : MonoBehaviour
    {
        private bool _scored = false;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_scored)
                return;

            if (!other.CompareTag("Player"))
                return;

            if (GameManager.Instance == null)
                return;

            _scored = true;
            GameManager.Instance.AddScore(1);
            AudioManager.Instance?.PlayPoint();
        }
    }
}
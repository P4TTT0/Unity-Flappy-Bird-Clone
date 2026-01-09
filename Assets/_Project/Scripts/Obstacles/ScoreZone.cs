using FlappyBird.Core;
using UnityEngine;

namespace FlappyBird.Obstacles
{
    public class ScoreZone : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                GameManager.Instance?.AddScore(1);
            }
        }
    }
}
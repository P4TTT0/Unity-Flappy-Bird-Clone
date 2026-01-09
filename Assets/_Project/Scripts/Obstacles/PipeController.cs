using FlappyBird.Core;
using UnityEngine;

namespace FlappyBird.Obstacles
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PipeController : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 2f;

        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (GameManager.Instance == null)
                return;

            if (GameManager.Instance.CurrentState != GameState.Playing)
                return;

            Vector2 newPosition = _rb.position + Vector2.left * _moveSpeed * Time.fixedDeltaTime;
            _rb.MovePosition(newPosition);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Despawn"))
            {
                Destroy(gameObject);
            }
        }
    }
}

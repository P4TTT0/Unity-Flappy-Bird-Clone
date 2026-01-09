using FlappyBird.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FlappyBird.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float _jumpForce = 5f;

        private Rigidbody2D _rigidbody;
        private bool _isAlive = true;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            if (GameManager.Instance != null)
                GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
        }

        private void OnDisable()
        {
            if (GameManager.Instance != null)
                GameManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
        }

        private void Update()
        {
            if (!_isAlive)
                return;

            if (GameManager.Instance == null || GameManager.Instance.CurrentState != GameState.Playing)
                return;

            if (Mouse.current == null)
                return;

            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                Jump();
            }
        }

        private void Jump()
        {
            // Se resetea la velocidad a 0 antes de aplicar la fuerza para un salto consistente
            _rigidbody.linearVelocity = Vector2.zero;
            // Se aplica una fuerza hacia arriba para el salto. Se usa Impulse para un efecto instantáneo.
            _rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        }

        private void HandleGameStateChanged(GameState state)
        {
            if (state == GameState.GameOver)
            {
                _isAlive = false;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!_isAlive)
                return;

            if (GameManager.Instance != null)
                GameManager.Instance.GameOver();
        }
    }
}

using FlappyBird.Core;
using System;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FlappyBird.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float _jumpForce = 5f;

        [Header("Rotation")]
        [SerializeField] private float _rotationSpeed = 10f;
        [SerializeField] private float _maxRotationUp = 15f;
        [SerializeField] private float _maxRotationDown = -90f;
        [SerializeField] private float _glideDelaySeconds = 0.5f;

        [Header("Death")]
        [SerializeField] private float _deathJumpForce = 3f;
        [SerializeField] private float _deathGravityScale = 3f;

        private Rigidbody2D _rigidbody;
        private bool _isAlive = true;
        private float _timeSinceJumpSeconds = 0f;

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

            _timeSinceJumpSeconds += Time.deltaTime;
            UpdateRotation();
        }

        private void Jump()
        {
            // Se resetea la velocidad a 0 antes de aplicar la fuerza para un salto consistente
            _rigidbody.linearVelocity = Vector2.zero;
            // Se aplica una fuerza hacia arriba para el salto. Se usa Impulse para un efecto instantáneo.
            _rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            // Resetear el timer del salto
            _timeSinceJumpSeconds = 0f;
        }

        /// <summary>
        /// Se calcula una rotacion objetivo en base al estado de movimiento del jugador (Subiendo, cayendo o planeando) y se interpola suavemente hacia esa rotación.
        /// </summary>
        private void UpdateRotation()
        {
            // Calcular la rotación objetivo basada en la velocidad vertical
            float targetRotation;

            if (_rigidbody.linearVelocity.y > 0 || _timeSinceJumpSeconds < _glideDelaySeconds)
            {
                // Subiendo o en periodo de planeo - rotar hacia arriba
                targetRotation = _maxRotationUp;
            }
            else
            {
                // Clampeamos el valor de la velocidad divido un maximo para obtener un valor entre 0 y 1
                float fallSpeedNormalized = Mathf.Clamp01(-_rigidbody.linearVelocity.y / 10f);
                // Buscamos una rotacion hacia abajo basada en la velocidad de caída
                targetRotation = Mathf.Lerp(0, _maxRotationDown, fallSpeedNormalized);
            }

            // Normalizamos la rotacion entre -180 y 180 grados para evitar problemas de interpolacion
            float currentZRotation = transform.eulerAngles.z;
            if (currentZRotation > 180f) currentZRotation -= 360f;

            // Interpolamos suavemente hacia la rotación objetivo
            float newRotation = Mathf.Lerp(currentZRotation, targetRotation, _rotationSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0, 0, newRotation);
        }

        private void Die()
        {
            if (!_isAlive)
                return;

            _isAlive = false;

            // Reset de velocidad
            _rigidbody.linearVelocity = Vector2.zero;

            // Pequeño impulso hacia arriba
            _rigidbody.AddForce(Vector2.up * _deathJumpForce, ForceMode2D.Impulse);

            // Aumentamos gravedad para que caiga más fuerte
            _rigidbody.gravityScale = _deathGravityScale;

            // Desactivamos colisiones
            Collider2D collider = GetComponent<Collider2D>();
            if (collider != null)
                collider.enabled = false;

            // Avisamos al GameManager
            if (GameManager.Instance != null)
                GameManager.Instance?.GameOver();

            // Shake de cámara
            CameraShake.Instance?.Shake();
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

            if (collision.collider.CompareTag("Obstacle") || collision.collider.CompareTag("Ground"))
            {
                Die();
            }
        }
    }
}

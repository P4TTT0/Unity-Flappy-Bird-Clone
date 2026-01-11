using UnityEngine;

namespace FlappyBird.Player
{
    // Se hizo este script para manejar la animación de aleteo del jugador usando sprites en lugar de un Animator, ya que es más eficiente para este caso simple y no requiere la complejidad de un sistema de animación completo.
    [RequireComponent(typeof(SpriteRenderer))]
    public class PlayerFlapAnimation : MonoBehaviour
    {
        [Header("Frames")]
        [SerializeField] private Sprite[] _frames = new Sprite[3];

        [Header("Timing")]
        [SerializeField] private float _frameRate = 12f; 

        private SpriteRenderer _renderer;
        private int _frameIndex;
        private float _timer;

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();

            if (_frames is { Length: > 0 } && _frames[0] != null)
                _renderer.sprite = _frames[0];
        }

        private void Update()
        {
            if (_frames == null || _frames.Length == 0)
                return;

            _timer += Time.deltaTime;
            float frameDuration = 1f / Mathf.Max(_frameRate, 0.01f);

            while (_timer >= frameDuration)
            {
                _timer -= frameDuration;
                _frameIndex = (_frameIndex + 1) % _frames.Length;
                _renderer.sprite = _frames[_frameIndex];
            }
        }
    }
}

using UnityEngine;

namespace FlappyBird.UI
{
    public class FloatingUI : MonoBehaviour
    {
        [Header("Floating Settings")]
        [SerializeField] private float _amplitude = 20f; 
        [SerializeField] private float _frequency = 1f;  

        private RectTransform _rectTransform;
        private Vector2 _startPosition;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _startPosition = _rectTransform.anchoredPosition;
        }

        private void Update()
        {
            // Se calcula el desplazamiento vertical usando una función seno
            // El tiempo se multiplica por la frecuencia para controlar la velocidad del movimiento
            // La amplitud controla la altura máxima del movimiento
            float offsetY = Mathf.Sin(Time.time * _frequency) * _amplitude;
            _rectTransform.anchoredPosition = _startPosition + Vector2.up * offsetY;
        }
    }
}

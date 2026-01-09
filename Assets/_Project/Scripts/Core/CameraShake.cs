using UnityEngine;
using System.Collections;

namespace FlappyBird.Core
{
    public class CameraShake : MonoBehaviour
    {
        public static CameraShake Instance { get; private set; }

        [SerializeField] private float _shakeDuration = 0.15f;
        [SerializeField] private float _shakeStrength = 0.2f;

        private Vector3 _originalPosition;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            _originalPosition = transform.localPosition;
        }

        public void Shake()
        {
            StopAllCoroutines();
            StartCoroutine(ShakeCoroutine());
        }

        private IEnumerator ShakeCoroutine()
        {
            float elapsed = 0f;

            while (elapsed < _shakeDuration)
            {
                Vector2 offset = Random.insideUnitCircle * _shakeStrength;
                transform.localPosition = _originalPosition + new Vector3(offset.x, offset.y, 0f);

                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.localPosition = _originalPosition;
        }
    }
}

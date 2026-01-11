using UnityEngine;
using FlappyBird.Core;

namespace FlappyBird.Obstacles
{
    public class PipeSpawner : MonoBehaviour
    {
        [Header("Debug")]
        [SerializeField] private bool _generatePipes = true;

        [Header("Spawn")]
        [SerializeField] private GameObject _pipePrefab;
        [SerializeField] private float _spawnInterval = 2f;
        [SerializeField] private float _minY = -1.8f;
        [SerializeField] private float _maxY = 3f;

        private float _timer;

        private void Update()
        {
            if (GameManager.Instance == null)
                return;

            if (GameManager.Instance.CurrentState != GameState.Playing)
                return;

            _timer += Time.deltaTime;

            if (_timer >= _spawnInterval)
            {
                SpawnPipe();
                _timer = 0f;
            }
        }

        private void SpawnPipe()
        {
            if (!_generatePipes)
                return;

            float randomY = Random.Range(_minY, _maxY);
            Vector3 spawnPosition = new Vector3(transform.position.x, randomY, 0f);

            Instantiate(_pipePrefab, spawnPosition, Quaternion.identity);
        }
    }
}

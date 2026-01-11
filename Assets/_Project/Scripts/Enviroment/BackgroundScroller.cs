using System.Collections.Generic;
using UnityEngine;
using FlappyBird.Core;

namespace FlappyBird.Environment
{
    public sealed class BackgroundScroller : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float _scrollSpeed = 2f;

        [Header("Setup")]
        [Tooltip("Optional. If empty, all direct children will be used as tiles.")]
        [SerializeField] private List<Transform> _tiles = new();
        [SerializeField] private float _tileWidthOffset = 1f;

        private float _tileWidth;
        private int _tileCount;

        private void Awake()
        {
            if (_tiles.Count == 0)
            {
                _tiles = new List<Transform>(transform.childCount);
                for (int i = 0; i < transform.childCount; i++)
                    _tiles.Add(transform.GetChild(i));
            }

            _tileCount = _tiles.Count;
            if (_tileCount == 0)
            {
                Debug.LogError("[GroundScroller] No tiles assigned or found as children.");
                enabled = false;
                return;
            }

            var renderer = _tiles[0].GetComponent<SpriteRenderer>();
            if (renderer == null)
            {
                Debug.LogError("[GroundScroller] First tile has no SpriteRenderer.");
                enabled = false;
                return;
            }

            _tileWidth = renderer.bounds.size.x;

            _tiles.Sort((a, b) => a.position.x.CompareTo(b.position.x));
        }

        private void Update()
        {
            if (GameManager.Instance == null || GameManager.Instance.CurrentState != GameState.Playing)
                return;

            float delta = _scrollSpeed * Time.deltaTime;

            // Mover todo el ground (hijos incluidos)
            transform.position += Vector3.left * delta;

            // Reciclar: si el tile más a la izquierda salió del todo, lo mandamos al final
            var leftMost = _tiles[0];
            float leftMostRightEdge = leftMost.position.x + (_tileWidth * _tileWidthOffset);

            // Umbral: cuando su borde derecho pasa por un punto a la izquierda del origen
            // Ajustá este valor si tu cámara está muy corrida.
            if (leftMostRightEdge < -_tileWidth)
            {
                var rightMost = _tiles[_tileCount - 1];
                float newX = rightMost.position.x + _tileWidth;

                leftMost.position = new Vector3(newX, leftMost.position.y, leftMost.position.z);

                // Rotamos la lista: el que era primero pasa a ser último
                _tiles.RemoveAt(0);
                _tiles.Add(leftMost);
            }
        }
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InfoModalController : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private GameObject _root;         
    [SerializeField] private RectTransform _card;      
    [SerializeField] private Button _backdropButton;   

    [Header("Animation")]
    [SerializeField] private float _slideDuration = 0.25f;
    [SerializeField] private float _hiddenYOffset = -900f;

    private Vector2 _visiblePos;
    private Vector2 _hiddenPos;

    private Coroutine? _animationRoutine;
    private bool _isOpen;

    private void Awake()
    {
        _visiblePos = _card.anchoredPosition;
        _hiddenPos = _visiblePos + new Vector2(0f, _hiddenYOffset);

        _backdropButton.onClick.AddListener(Close);

        _root.SetActive(false);
        _card.anchoredPosition = _hiddenPos;
        _isOpen = false;
    }

    private void Update()
    {
        if (!_isOpen)
            return;

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Close();
        }
    }

    public void Open()
    {
        if (_isOpen)
            return;

        _isOpen = true;
        _root.SetActive(true);

        StartSlide(_hiddenPos, _visiblePos, onComplete: null);
    }

    public void Close()
    {
        if (!_isOpen)
            return;

        _isOpen = false;

        StartSlide(_card.anchoredPosition, _hiddenPos, onComplete: () =>
        {
            _root.SetActive(false);
        });
    }

    private void StartSlide(Vector2 from, Vector2 to, System.Action? onComplete)
    {
        if (_animationRoutine != null)
            StopCoroutine(_animationRoutine);

        _animationRoutine = StartCoroutine(SlideRoutine(from, to, onComplete));
    }

    private IEnumerator SlideRoutine(Vector2 from, Vector2 to, System.Action? onComplete)
    {
        float elapsed = 0f;

        while (elapsed < _slideDuration)
        {
            elapsed += Time.unscaledDeltaTime; 
            float t = Mathf.Clamp01(elapsed / _slideDuration);

            float eased = 1f - Mathf.Pow(1f - t, 3f);

            _card.anchoredPosition = Vector2.LerpUnclamped(from, to, eased);
            yield return null;
        }

        _card.anchoredPosition = to;
        onComplete?.Invoke();
        _animationRoutine = null;
    }
}

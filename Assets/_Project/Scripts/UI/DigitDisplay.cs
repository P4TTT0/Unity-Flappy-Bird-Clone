using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DigitDisplay : MonoBehaviour
{
    [SerializeField] private GameObject _digitPrefab;
    [SerializeField] private Sprite[] _digitSprites;
    [SerializeField] private float _spacing = 32f;

    private readonly List<GameObject> _digits = new();

    public void SetNumber(int value)
    {
        Clear();

        string text = value.ToString();

        for (int i = 0; i < text.Length; i++)
        {
            int digit = text[i] - '0';

            GameObject go = Instantiate(_digitPrefab, transform);
            Image img = go.GetComponent<Image>();
            img.sprite = _digitSprites[digit];

            RectTransform rt = go.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(i * _spacing, 0);

            _digits.Add(go);
        }
    }

    private void Clear()
    {
        foreach (var d in _digits)
            Destroy(d);

        _digits.Clear();
    }
}

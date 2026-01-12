using FlappyBird.Audio;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VolumeToggleButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image _icon;
    [SerializeField] private Sprite _volumeOnSprite;
    [SerializeField] private Sprite _volumeOffSprite;

    private void Start()
    {
        RefreshIcon();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.Instance.ToggleMute();
        RefreshIcon();
    }

    private void RefreshIcon()
    {
        _icon.sprite = AudioManager.Instance.IsMuted ? _volumeOffSprite : _volumeOnSprite;
    }
}

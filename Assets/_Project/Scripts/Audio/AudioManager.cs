using UnityEngine;

namespace FlappyBird.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        private const string MUTE_KEY = "IS_MUTED";

        [Header("SFX")]
        [SerializeField] private AudioClip _wing;
        [SerializeField] private AudioClip _point;
        [SerializeField] private AudioClip _hit;
        [SerializeField] private AudioClip _die;
        [SerializeField] private AudioClip _swoosh;

        [Header("UI Sounds")]
        [SerializeField] private AudioClip uiClick;

        [Header("Configuration")]
        public bool IsMuted { get; private set; }

        private AudioSource _audioSource;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            IsMuted = PlayerPrefs.GetInt(MUTE_KEY, 0) == 1;
            ApplyVolume();

            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.playOnAwake = false;
        }

        public void PlayWing() => Play(_wing);
        public void PlayPoint() => Play(_point);
        public void PlayHit() => Play(_hit);
        public void PlayDie() => Play(_die);
        public void PlaySwoosh() => Play(_swoosh);
        public void PlayUIClick() => Play(uiClick);

        private void Play(AudioClip clip)
        {
            if (clip == null)
                return;

            _audioSource.PlayOneShot(clip);
        }

        public void ToggleMute()
        {
            IsMuted = !IsMuted;
            PlayerPrefs.SetInt(MUTE_KEY, IsMuted ? 1 : 0);
            PlayerPrefs.Save();

            ApplyVolume();
        }

        private void ApplyVolume()
        {
            AudioListener.volume = IsMuted ? 0f : 1f;
        }
    }
}

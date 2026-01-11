using UnityEngine;

namespace FlappyBird.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [Header("SFX")]
        [SerializeField] private AudioClip _wing;
        [SerializeField] private AudioClip _point;
        [SerializeField] private AudioClip _hit;
        [SerializeField] private AudioClip _die;
        [SerializeField] private AudioClip _swoosh;

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

            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.playOnAwake = false;
        }

        public void PlayWing() => Play(_wing);
        public void PlayPoint() => Play(_point);
        public void PlayHit() => Play(_hit);
        public void PlayDie() => Play(_die);
        public void PlaySwoosh() => Play(_swoosh);

        private void Play(AudioClip clip)
        {
            if (clip == null)
                return;

            _audioSource.PlayOneShot(clip);
        }
    }
}

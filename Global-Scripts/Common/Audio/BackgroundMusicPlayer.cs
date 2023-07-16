using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using Time = UnityEngine.Time;

namespace DaftAppleGames.Common.Audio
{
    public class BackgroundMusicPlayer : MonoBehaviour
    {
        [BoxGroup("Music Clips")]
        public AudioClip[] backgroundMusicClips;
        
        [BoxGroup("Play Settings")]
        public int startWithClipNumber = 0;
        [BoxGroup("Play Settings")]
        public float fadeInTime = 2.0f;

        [BoxGroup("Start Settings")]
        public bool playOnStart = true;
        [BoxGroup("Start Settings")]
        public float delayBeforeStart = 1.0f;
        
        [BoxGroup("Multi Clip Settings")]
        public bool cycleAllClips = true;
        [BoxGroup("Multi Clip Settings")]
        public float swapWhenSecondsLeft = 2.0f;

        [FoldoutGroup("Debug")]
        [SerializeField]
        private int _currentClip = 0;
        [FoldoutGroup("Debug")]
        [SerializeField]
        private float _currentClipLength;
        [FoldoutGroup("Debug")]
        [SerializeField]
        private float _playedSoFar;
        [FoldoutGroup("Debug")]
        [SerializeField]
        private bool _inFade = false;
        [FoldoutGroup("Debug")]
        [SerializeField]
        private bool _isPlaying;

        private int _totalClips;

        private AudioSource _audioSource;

        // Start is called before the first frame update
        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            _totalClips = backgroundMusicClips.Length;

            _isPlaying = false;
            
            if (playOnStart)
            {
                StartCoroutine(PlayAfterDelay(delayBeforeStart));
            }
        }

        /// <summary>
        /// Begin playing after given number of seconds
        /// </summary>
        /// <param name="delay"></param>
        /// <returns></returns>
        private IEnumerator PlayAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            Debug.Log("Playing");
            Play();
        }
        
        /// <summary>
        /// Wait until clip is almost finished, fade out and fade in next
        /// </summary>
        private void Update()
        {
            if (!_isPlaying)
            {
                return;
            }
            _playedSoFar = _audioSource.time;

            if (_audioSource.time + swapWhenSecondsLeft >= _currentClipLength && !_inFade)
            {
                _inFade = true;
                int nextClip = _currentClip++;
                if(nextClip > _totalClips)
                {
                    nextClip = 0;
                }
                _currentClip = nextClip;
                FadeOut(true);
            }
        }

        /// <summary>
        /// Start playing the default clip
        /// </summary>
        public void Play()
        {
            _currentClip = startWithClipNumber;
            FadeIn();
        }

        /// <summary>
        /// Fade in the current clip
        /// </summary>
        private void FadeIn()
        {
            StartCoroutine(FadeInAsync());
        }

        /// <summary>
        /// Async coroutine to fade in an audiosource fromn 0 volume.
        /// </summary>
        /// <param name="audioClip"></param>
        /// <param name="fadePeriod"></param>
        /// <returns></returns>
        private IEnumerator FadeInAsync()
        {
            _audioSource.Stop();
            _audioSource.volume = 0;
            _audioSource.clip = backgroundMusicClips[_currentClip];
            _currentClipLength = _audioSource.clip.length;
            _audioSource.Play();
            _isPlaying = true;

            float time = 0.0f;

            // Lerp up to target volume
            while (time < fadeInTime)
            {
                _audioSource.volume = Mathf.Lerp(0, 1.0f, time / fadeInTime);
                time += Time.deltaTime;
                yield return null;
            }

            _audioSource.volume = 1.0f;
            _inFade = false;
        }

        /// <summary>
        /// Fade out the current clip and start a new one, if specified.
        /// </summary>
        /// <param name="newClip"></param>
        private void FadeOut(bool playNext)
        {
            StartCoroutine(FadeOutAsync(playNext));
        }

        /// <summary>
        /// Async Coroutine to fade an audiosource to 0 volume over time
        /// and start a new clip, if specified
        /// </summary>
        /// <param name="newClip"></param>
        /// <returns></returns>
        private IEnumerator FadeOutAsync(bool playNext)
        {
            float startVolume = _audioSource.volume;
            float time = 0.0f;

            // Lerp in volume
            while (time < fadeInTime)
            {
                _audioSource.volume = Mathf.Lerp(startVolume, 0.0f, time / fadeInTime);
                time += Time.deltaTime;
                yield return null;
            }

            // Fade in the new clip, if specified
            if(playNext)
            {
                _currentClip++;
                if(_currentClip == _totalClips)
                {
                    _currentClip = 0;
                }
                FadeIn();
            }
            else
            {
                _isPlaying = false;
            }
            _inFade = false;
        }
    }
}
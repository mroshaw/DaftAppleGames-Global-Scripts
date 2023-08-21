using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using Time = UnityEngine.Time;

namespace DaftAppleGames.Common.Audio
{
    public enum BackgroundMusicType { Clip, Group }

    public class BackgroundMusicPlayer : MonoBehaviour
    {
        [BoxGroup("Music Clips")]
        public BackgroundMusicClip[] backgroundMusicClips;

        [BoxGroup("Play Settings")]
        public float fadeInTime = 2.0f;

        [BoxGroup("Start Settings")]
        public bool playOnStart = true;
        [BoxGroup("Start Settings")]
        public string startGroup;
        [BoxGroup("Start Settings")]
        public string startClip;
        [BoxGroup("Start Settings")]
        public float delayBeforeStart = 1.0f;
        [BoxGroup("Group Settings")]
        public float swapWhenSecondsLeft = 2.0f;

        [FoldoutGroup("Events")]
        public UnityEvent<string> ClipStartedEvent;
        [FoldoutGroup("Events")]
        public UnityEvent<string> ClipFinishedEvent;
        [FoldoutGroup("Events")]
        public UnityEvent<string> ClipGroupStartedEvent;
        [FoldoutGroup("Events")]
        public UnityEvent<string> ClipGroupFinishedEvent;

        [FoldoutGroup("Debug")]
        [SerializeField]
        private int _currentClip = 0;
        [FoldoutGroup("Debug")]
        [SerializeField]
        private BackgroundMusicType _currentClipType;
        [FoldoutGroup("Debug")]
        [SerializeField]
        private string _currentClipName;
        [FoldoutGroup("Debug")]
        [SerializeField]
        public string _currentClipGroupName;
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

        private List<BackgroundMusicClip> _currentClipGroup;

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
            if (startClip != String.Empty)
            {
                PlayByName(startClip);
                yield break;
            }

            if (startGroup != String.Empty)
            {
                PlayByGroup(startGroup);
                yield break;
            }
        }

        /// <summary>
        /// Begin playing the specified clip
        /// </summary>
        /// <param name="clipName"></param>
        /// <param name="endClipDelegate"></param>
        public void PlayByName(string clipName)
        {
            _currentClipType = BackgroundMusicType.Clip;
            _currentClipGroup = new List<BackgroundMusicClip>();
            foreach (BackgroundMusicClip clip in backgroundMusicClips)
            {
                if (clip.ClipName == clipName)
                {
                    _currentClipGroup.Add(clip);
                    break;
                }
            }
            _currentClipName = clipName;
            Play();
        }

        /// <summary>
        /// Play the specified group of audio clips
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="endGroupDelegate"></param>
        public void PlayByGroup(string groupName)
        {
            _currentClipType = BackgroundMusicType.Group;
            _currentClipGroup = new List<BackgroundMusicClip>();

            foreach (BackgroundMusicClip clip in backgroundMusicClips)
            {
                if (clip.GroupName == groupName)
                {
                    _currentClipGroup.Add(clip);
                }
            }
            _currentClipGroupName = groupName;
            ClipGroupStartedEvent.Invoke(groupName);
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
                FadeOut(_currentClipType == BackgroundMusicType.Group);
            }
        }

        /// <summary>
        /// Start playing the default clip
        /// </summary>
        public void Play()
        {
            _totalClips = _currentClipGroup.Count;
            _currentClip = 0;
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
            _audioSource.clip = _currentClipGroup[_currentClip].AudioClip;
            _currentClipLength = _audioSource.clip.length;
            _audioSource.Play();
            // ClipStartedEvent.Invoke(_currentClipGroup[_currentClip].ClipName);
            // _currentClipGroup[_currentClip].ClipStartEvent.Invoke();
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

            _currentClipGroup[_currentClip].ClipFinishEvent.Invoke();


            if (_currentClipType == BackgroundMusicType.Clip)
            {
                // ClipFinishedEvent.Invoke(_currentClipName);
            }

            if (_currentClipType == BackgroundMusicType.Group)
            {
                // ClipGroupFinishedEvent.Invoke(_currentClipGroupName);
            }

            // Fade in the new clip, if specified
            if(_currentClipType == BackgroundMusicType.Group && playNext)
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

        [Serializable]
        public class BackgroundMusicClip
        {
            public string ClipName;
            public string GroupName;
            public bool Loop;
            public AudioClip AudioClip;
            [FoldoutGroup("Events")]
            public UnityEvent ClipStartEvent;
            [FoldoutGroup("Events")]
            public UnityEvent ClipFinishEvent;
        }
    }
}
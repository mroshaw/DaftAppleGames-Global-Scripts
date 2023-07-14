using UnityEngine;
using System;

namespace DaftAppleGames.Common.Characters
{
    public class Character : MonoBehaviour
    {
        [Header("Character Settings")]
        public string characterName;

        [Header("Debug")]
        public bool enableDebug = false;

        [Header("Audio FX")]
        public AudioSource audioSource;
        public AudioClip[] deathAudioClips;
        public AudioClip[] hitAudioClips;

        private int _hitClips;
        private int _deathClips;
        
        // Start is called before the first frame update
        public virtual void Start()
        {
            if (!audioSource)
            {
                audioSource = GetComponent<AudioSource>();
            }

            _hitClips = hitAudioClips.Length;
            _deathClips = deathAudioClips.Length;
        }

        /// <summary>
        /// Hook into OnDeath
        /// </summary>
        public void PlayHitAudio()
        {
            System.Random random = new System.Random();
            int randomClipNum = random.Next(0, _hitClips);
            audioSource.PlayOneShot(hitAudioClips[randomClipNum]);
        }

        /// <summary>
        /// Hook into OnHit
        /// </summary>
        public void PlayDeathAudio()
        {
            System.Random random = new System.Random();
            int randomClipNum = random.Next(0, _deathClips);
            audioSource.PlayOneShot(deathAudioClips[randomClipNum]);
        }
        
        // Update is called once per frame
        public virtual void Update()
        {

        }
    }
}
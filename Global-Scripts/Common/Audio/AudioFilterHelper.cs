using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;

namespace DaftAppleGames.Common.Audio
{
    public class AudioFilterHelper : MonoBehaviour
    {
        public AudioMixerSnapshot indoorSnapshot;
        public AudioMixerSnapshot outdoorSnapshot;

        /// <summary>
        /// Fade in the filter effects
        /// </summary>
        [Button("Fade In")]
        public void FadeInFilters()
        {
            indoorSnapshot.TransitionTo(0.1f);
        }

        /// <summary>
        /// Fade out the filter effects
        /// </summary>
        [Button("Fade Out")]
        public void FadeOutFilters()
        {
            outdoorSnapshot.TransitionTo(0.1f);
        }
    }
}

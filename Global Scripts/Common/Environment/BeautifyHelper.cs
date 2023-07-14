#if BEAUTIFY3
using BeautifyEffect;
using UnityEngine;

namespace DaftAppleGames.Common.Environment 
{
    public class BeautifyHelper : MonoBehaviour
    {
        [Header("Beautify Settings")]
        public BeautifyProfile[] profiles;
        
        private Beautify _beautify;

        private void Start()
        {
            FindBeautify();
        }

        /// <summary>
        /// Public method to set a new Beautify profile
        /// </summary>
        /// <param name="profileIndex"></param>
        public void EnableProfile(int profileIndex)
        {
            _beautify.profile = profiles[profileIndex];
        }
        
        // Public Instance for accessing the full API
        public Beautify BeautifyInstance => _beautify;

        /// <summary>
        /// Enables the Beautify component
        /// </summary>
        public void Enable()
        {
            if (_beautify)
            {
                _beautify.enabled = true;
            }
        }

        /// <summary>
        /// Disabled the Beautify component
        /// </summary>
        public void Disable()
        {
            if (_beautify)
            {
                _beautify.enabled = false;
            }
        }

        /// <summary>
        /// Find the Beautify component instance
        /// </summary>
        private void FindBeautify()
        {
            _beautify = FindObjectOfType<Beautify>();
        }
    }
}
#endif
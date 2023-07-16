using UnityEngine;

namespace DaftAppleGames.Common.Utils
{

    public class DestroyOnStart : MonoBehaviour
    {
        [Header("General Settings")]
        public bool destroy = true;

        /// <summary>
        /// Destroy or de-activate on start
        /// </summary>
        void Start()
        {
            if(destroy)
            {
                Destroy(this.gameObject);
            }
            else
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}
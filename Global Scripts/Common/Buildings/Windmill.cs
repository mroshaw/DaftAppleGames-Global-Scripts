using Sirenix.OdinInspector;
using TheVegetationEngine;
using UnityEngine;

namespace DaftAppleGames.Common.Buildings
{
    /// <summary>
    /// Simple Windmill blade spinner Monobehaviour
    /// </summary>
    [ExecuteInEditMode]
    public class Windmill : MonoBehaviour
    {
        [BoxGroup("General Settings")]
        public GameObject windMillBlades;
        [BoxGroup("Speed Settings")]
        public float rotateSpeed = 0.5f;
        public bool syncToTveWind = false;

        private bool _isTvePresent = false;
        
        /// <summary>
        /// Configure the windmill blades
        /// </summary>
        public void Start()
        {
            if (!windMillBlades)
            {
                windMillBlades = this.gameObject;
            }

            if (TVEManager.Instance != null)
            {
                _isTvePresent = true;
            }
        }

        /// <summary>
        /// Rotate the windmill
        /// </summary>
        public void Update()
        {
            if (syncToTveWind && _isTvePresent)
            {
                windMillBlades.transform.Rotate(TVEManager.Instance.globalMotion.windPower, 0.0f, 0.0f, Space.Self);
            }
            else
            {
                windMillBlades.transform.Rotate(rotateSpeed, 0.0f, 0.0f, Space.Self);
            }
        }
    }
}
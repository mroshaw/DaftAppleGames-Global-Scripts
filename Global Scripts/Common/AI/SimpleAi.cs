using UnityEngine;
using Sirenix.OdinInspector;

namespace DaftAppleGames.Common.AI
{
    public class SimpleAi : MonoBehaviour
    {
        // Public serializable properties
        [FoldoutGroup("General Settings")]
        public string setting1;
        
        [FoldoutGroup("Simple Ai")]
        public string setting2;
        [FoldoutGroup("Simple Ai")]
        public string setting3;
        
        // Public properties
        public string Property1
        {
            get { return _privateField; }
            set { _privateField = value; }
        }
        
        // Private fields
        private string _privateField;

        #region UNITY_EVENTS
        /// <summary>
        /// Subscribe to events
        /// </summary>   
        private void OnEnable()
        {
            
        }
        
        /// <summary>
        /// Unsubscribe from events
        /// </summary>   
        private void OnDisable()
        {
            
        }

        /// <summary>
        /// Configure the component on awake
        /// </summary>   
        private void Awake()
        {
            
        }
    
        /// <summary>
        /// Configure the component on start
        /// </summary>
        private void Start()
        {
            
        }
        #endregion
    }
}

#if PIXELCRUSHERS
using System.Linq;
using PixelCrushers.DialogueSystem;
using UnityEngine;


namespace DaftAppleGames
{
    /// <summary>
    /// Simple wrapper to call Dialogue Barks when player enters NPC collider area
    /// </summary>
    public class ProximityBark : BarkStarter
    {
        [Header("Bark Targets")]
        [Tooltip("Target to whom bark is addressed. Leave unassigned to just bark into the air.")]
        public Transform target;
        public string[] targetTags;
        
        /// <summary>
        /// Bark when the collider enters the trigger
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            if (enabled && (!DialogueManager.isConversationActive || allowDuringConversations) && !DialogueTime.isPaused)
            {
                if (targetTags.Any(other.tag.Contains))
                {
                    TryBark(target);
                }
            }
        }
    }
}
#endif
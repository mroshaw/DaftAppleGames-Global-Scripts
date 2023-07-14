#if PIXELCRUSHERS
using PixelCrushers.DialogueSystem;
using UnityEngine;

namespace DaftAppleGames.Common.Quests
{
    public class StartDialogue : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// Starts an "internal" dialog wiith the player
        /// </summary>
        /// <param name="player"></param>
        public void StartInternalDialog(GameObject player)
        {
            DialogueActor actor = player.GetComponent<DialogueActor>();
            if (!actor)
            {
                return;
            }
        }
    }
}
#endif
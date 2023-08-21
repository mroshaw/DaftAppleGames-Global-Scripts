using UnityEngine;

namespace DaftAppleGames.Common.Tutorials
{
    [CreateAssetMenu(fileName = "TutorialContent", menuName = "Game/Tutorial", order = 1)]
    public class TutorialContent : ScriptableObject
    {
        // Public serializable properties
        [Header("General Settings")]
        public string heading;
        [Multiline(10)]
        public string content;
        public Sprite image;
    }
}

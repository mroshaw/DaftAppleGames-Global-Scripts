using DaftAppleGames.Editor.Common;
using UnityEditor.EditorTools;
using UnityEngine;

namespace DaftAppleGames.Editor.Buildings
{
    /// <summary>
    /// Scriptable Object to store Editor usable instances of the Player Character Configuration
    /// </summary>
    [CreateAssetMenu(fileName = "PropColliderEditorSettings", menuName = "Settings/Buildings/PropColliderEditor", order = 1)]
    public class PropColliderEditorSettings : EditorToolSettings
    {
        public string[] boxSearchStrings;
        public string[] capsuleSearchStrings;
        public string[] sphereSearchStrings;
        public string[] meshSearchStrings;
    }
}
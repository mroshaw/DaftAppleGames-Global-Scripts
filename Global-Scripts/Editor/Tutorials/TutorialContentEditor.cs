using DaftAppleGames.Common.Tutorials;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace DaftAppleGames.Editor.Tutorials
{
    [CustomEditor(typeof(TutorialContent))]
    public class TutorialContentEditor : OdinEditor
    {
        public TutorialContent tutorialContent;
        
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            tutorialContent = target as TutorialContent;
        }
    }
}

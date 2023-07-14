using System;
using Sirenix.OdinInspector;

namespace DaftAppleGames.Common.GameControllers
{
    [Serializable]
    public class FixedScene
    {
        [TableColumnWidth(150, Resizable = true)]
        public string sceneName;
        [TableColumnWidth(150, Resizable = true)]
        public FixedSceneType sceneType;
    }
}

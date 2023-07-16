using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DaftAppleGames.Common.GameControllers
{
    [Serializable]
    public class AdditiveScene
    {
        [TableColumnWidth(150, Resizable = true)]
        public string sceneName;
        [TableColumnWidth(70, Resizable = false)]
        public bool edit;
        [TableColumnWidth(70, Resizable = false)]
        public bool inGame;
        [TableColumnWidth(70, Resizable = false)]
        public bool inEditor;
        [TableColumnWidth(90, Resizable = false)]
        public SceneType sceneType;

        [Button("Load")]
        private void Load()
        {
            Debug.Log($"Loading: {sceneName}");
        }

        private AsyncOperation _sceneOp;
        private SceneLoadStatus _loadStatus = SceneLoadStatus.None;

        public AsyncOperation SceneOp
        {
            get => _sceneOp;
            set => _sceneOp = value;
        }

        public SceneLoadStatus LoadStatus
        {
            get => _loadStatus;
            set => _loadStatus = value;
        }
    }
}

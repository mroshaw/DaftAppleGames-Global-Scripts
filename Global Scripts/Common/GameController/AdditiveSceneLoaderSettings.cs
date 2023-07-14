using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace DaftAppleGames.Common.GameControllers
{
    /// <summary>
    /// Scriptable Object: TODO Purpose and Summary
    /// </summary>
    [CreateAssetMenu(fileName = "AdditiveSceneLoaderSettings", menuName = "GameController/Additive Scene Loader Settings", order = 1)]
    public class AdditiveSceneLoaderSettings : ScriptableObject
    {
        [FoldoutGroup("Scene Paths")]
        public string controlScenePath;
        [FoldoutGroup("Scene Paths")]
        public string areaScenePath;
        [FoldoutGroup("Scene Paths")]
        public string terrainScenePath;
        [FoldoutGroup("Additive Scenes")]
        [TableList]
        public List<AdditiveScene> additiveScenes;
        [TableList]
        public List<FixedScene> fixedScenes;
    }
}

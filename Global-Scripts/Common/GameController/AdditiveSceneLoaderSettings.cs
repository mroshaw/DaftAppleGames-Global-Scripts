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
        [BoxGroup("Scene Paths")]
        public string controlScenePath;
        [BoxGroup("Scene Paths")]
        public string areaScenePath;
        [BoxGroup("Scene Paths")]
        public string terrainScenePath;
        [BoxGroup("Additive Scenes")]
        [TableList]
        public List<AdditiveScene> additiveScenes;
        [TableList]
        public List<FixedScene> fixedScenes;
    }
}

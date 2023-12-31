using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using DaftAppleGames.Common.GameControllers;

namespace DaftAppleGames.Editor
{
    [CustomEditor(typeof(AdditiveSceneLoadManager))]
    public class AdditiveSceneLoadManagerEditor : OdinEditor
    {
        public AdditiveSceneLoadManager sceneLoader;

        /// <summary>
        /// Show the Editor window
        /// </summary>
        public override void OnInspectorGUI()
        {
            sceneLoader = target as AdditiveSceneLoadManager;
            if (GUILayout.Button("Load Scenes in Editor"))
            {
                LoadAllScenes();
            }
            DrawDefaultInspector();
        }

        /// <summary>
        /// Loads all scenes selected into Editor
        /// </summary>
        private void LoadAllScenes()
        {
            foreach(AdditiveScene additiveScene in sceneLoader.additiveSceneSettings.additiveScenes)
            {
                if (additiveScene.edit)
                {
                    LoadScene(additiveScene);
                }
                else
                {
                    UnloadScene(additiveScene);
                }
            }
        }

        /// <summary>
        /// Check if we should load scenes immediately and do so
        /// </summary>
        public void Awake()
        {
            sceneLoader = target as AdditiveSceneLoadManager;
            EditorSceneManager.sceneOpened -= LoadEditorScenes;
            EditorSceneManager.sceneOpened += LoadEditorScenes;
        }

        public void OnDestroy()
        {
            EditorSceneManager.sceneOpened -= LoadEditorScenes;
        }

        /// <summary>
        /// Event handler to load all scenes once this scene has loaded
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="mode"></param>
        private void LoadEditorScenes(Scene scene, OpenSceneMode mode)
        {
            if (sceneLoader.gameObject.scene == scene || sceneLoader.loadScenesOnLoad)
            {
                LoadAllScenes();
            }
        }
        
        /// <summary>
        /// Load scene into Editor
        /// </summary>
        /// <param name="additiveScene"></param>
        private void LoadScene(AdditiveScene additiveScene)
        {
            // Check if it's already loaded
            Scene currentScene = EditorSceneManager.GetSceneByName(additiveScene.sceneName);
            if (!currentScene.IsValid())
            {
                if(additiveScene.edit)
                {
                    string fullPath = "";
                    switch (additiveScene.sceneType)
                    {
                        case SceneType.Area:
                            fullPath = $"{Application.dataPath}/{sceneLoader.additiveSceneSettings.areaScenePath}/{additiveScene.sceneName}.unity";
                            break;
                        case SceneType.Terrain:
                            fullPath = $"{Application.dataPath}/{sceneLoader.additiveSceneSettings.terrainScenePath}/{additiveScene.sceneName}.unity";
                            break;
                        default:
                            fullPath = $"{Application.dataPath}/{sceneLoader.additiveSceneSettings.controlScenePath}/{additiveScene.sceneName}.unity";
                            break;
                    }
                    EditorSceneManager.OpenScene(fullPath, OpenSceneMode.Additive);
                }
            }
        }
        
        /// <summary>
        /// Unload and remove scene from Editor, if it's present
        /// </summary>
        /// <param name="additiveScene"></param>
        private void UnloadScene(AdditiveScene additiveScene)
        {
            Scene scene = EditorSceneManager.GetSceneByName(additiveScene.sceneName);
            if (scene.IsValid())
            {
                EditorSceneManager.CloseScene(scene, true);                
            }
        }
    }
}
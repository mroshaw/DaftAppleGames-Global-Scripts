using System;
using System.Collections.Generic;
using System.Linq;
using DaftAppleGames.Editor.Common.Performance;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;
#if HDRP_PIPELINE
using UnityEngine.Rendering.HighDefinition;
#endif
namespace DaftAppleGames.Editor.Common
{
    [Serializable]
    public class MeshSettings
    {
        [BoxGroup("Editor Search Settings")]
        public EditorTools.EditorSearchCriteria searchCriteria;
#if HDRP_PIPELINE
        [BoxGroup("Mesh Lighting (HDRP)")]
        public LightLayerEnum lightLayers;
        [BoxGroup("Mesh Lighting (HDRP)")]
        public bool contributeGi;
        [BoxGroup("Mesh Lighting (HDRP)")]
        public bool staticShadowCaster = true;
        [BoxGroup("Mesh Lighting (HDRP)")]
        public ReceiveGI receiveGi = ReceiveGI.Lightmaps;
        [BoxGroup("Mesh Lighting (HDRP)")]
        public LightProbeUsage lightProbes;
        [BoxGroup("Mesh Lighting (HDRP)")]
        public ReflectionProbeUsage reflectionProbes;
        [BoxGroup("Mesh Lighting (HDRP)")]
        public MotionVectorGenerationMode motionVectors;
        [BoxGroup("Mesh Lighting (HDRP)")]
        public bool dynamicOcclusion;
#else
        [BoxGroup("Mesh Lighting")]
        public ShadowCastingMode castShadows;
        [BoxGroup("Mesh Lighting")]
        public bool receiveShadows;
        [BoxGroup("Mesh Lighting")]
        public bool contributeGi;
        [BoxGroup("Mesh Lighting")]
        public ReceiveGI receiveGi;
        [BoxGroup("Mesh Lighting")]
        public LightProbeUsage lightProbes;
        [BoxGroup("Mesh Lighting")]
        public ReflectionProbeUsage reflectionProbes;
        [BoxGroup("Mesh Lighting")]
        public MotionVectorGenerationMode motionVectors;
        [BoxGroup("Mesh Lighting")]
        public bool dynamicOcclusion;
#endif
    }
    
    public class MeshTools : MonoBehaviour
    {
        /// <summary>
        /// Updates all MeshRenders with given settings
        /// </summary>
        /// <param name="meshRenderers"></param>
        /// <param name="settings"></param>
        public static void UpdateMeshProperties(MeshRenderer[] meshRenderers, MeshSettings settings)
        {
            foreach (MeshRenderer meshRenderer in meshRenderers)
            {
                Debug.Log($"Update Meshes - {meshRenderer.gameObject.name}...");
#if HDRP_PIPELINE
                // int bitmask = (int)meshRenderer.renderingLayerMask;
                // bitmask = LightTools.CalculateBitmask(bitmask, settings.lightLayers);
                // meshRenderer.renderingLayerMask = (uint)bitmask;
                meshRenderer.renderingLayerMask = (uint)settings.lightLayers;
                meshRenderer.staticShadowCaster = settings.staticShadowCaster;
                meshRenderer.receiveGI = settings.receiveGi;
                meshRenderer.lightProbeUsage = settings.lightProbes;
                meshRenderer.motionVectorGenerationMode = settings.motionVectors;
                meshRenderer.allowOcclusionWhenDynamic = settings.dynamicOcclusion;
#else
                meshRenderer.shadowCastingMode = settings.castShadows;
                meshRenderer.receiveShadows = settings.receiveShadows;
                meshRenderer.receiveGI = settings.receiveGi;
                meshRenderer.lightProbeUsage = settings.lightProbes;
                meshRenderer.reflectionProbeUsage = settings.reflectionProbes;
                meshRenderer.motionVectorGenerationMode = settings.motionVectors;
                meshRenderer.allowOcclusionWhenDynamic = settings.dynamicOcclusion;
#endif
            }
        }
        
        /// <summary>
        /// Returns all MeshRenderers in gameobject passed, using the search criteria
        /// </summary>
        /// <param name="gameObjects"></param>
        /// <param name="includeInactive"></param>
        /// <param name="searchCriteria"></param>
        /// <returns></returns>
        public static MeshRenderer[] FindMeshRenderersInGameObjects(GameObject[] gameObjects, bool includeInactive, 
            EditorTools.EditorSearchCriteria searchCriteria)
        {
            List<MeshRenderer> allMeshRenderersInGameObjects = new();
            
            // Process all GameObjects - assumed to be top level "buildings"
            foreach (GameObject buildingGameObject in gameObjects)
            {
                Transform[] childTransforms = buildingGameObject.GetComponentsInChildren<Transform>(true);

                foreach (Transform currentTransform in childTransforms)
                {
                    GameObject currentGameObject = currentTransform.gameObject;
                    // Look for GameObjects that meet the search criteria
                    if ((searchCriteria.ParentGameObjectNames.Length == 0 || searchCriteria.ParentGameObjectNames.Any(currentGameObject.name.Contains)) &&
                        (searchCriteria.ParentGameObjectLayers.Length == 0 || searchCriteria.ParentGameObjectLayers.Any(LayerMask.LayerToName(currentGameObject.layer).Contains)) &&
                        (searchCriteria.ParentGameObjectLayers.Length == 0 || searchCriteria.ParentGameObjectLayers.Any(currentGameObject.tag.Contains)))
                    {
                        // Process all components
                        MeshRenderer[] allMeshRenderers = currentGameObject.GetComponentsInChildren<MeshRenderer>(includeInactive);
                        foreach (MeshRenderer meshRenderer in allMeshRenderers)
                        {
                            // Look for child components
                            if(searchCriteria.ComponentNames.Length == 0 || searchCriteria.ComponentNames.Any(meshRenderer.name.Contains))
                            {
                                allMeshRenderersInGameObjects.Add(meshRenderer);
                            }
                        }
                    }
                }
            }
            return allMeshRenderersInGameObjects.ToArray();
        }
    }
}
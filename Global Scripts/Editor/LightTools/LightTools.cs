using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
#if HDRP_PIPELINE
using UnityEngine.Rendering.HighDefinition;
#endif

namespace DaftAppleGames.Editor.Common.Performance
{
    public class LightTools : MonoBehaviour
    {
        /// <summary>
        /// Find all renderers with matching criteria in parent Game Object and children
        /// </summary>
        /// <param name="parentGameObject"></param>
        /// <returns></returns>
        public static Light[] FindLightsInGameObject(GameObject parentGameObject)
        {
            Light[] allLights = parentGameObject.GetComponentsInChildren<Light>(true);

            return allLights;
        }

        /// <summary>
        /// Find all lights in all gameobjects passed in
        /// </summary>
        /// <param name="allGameObjects"></param>
        /// <returns></returns>
        public static Light[] FindLightsInGameObjects(GameObject[] allGameObjects)
        {
            Light[] allLights = Array.Empty<Light>();

            foreach (GameObject currentGo in allGameObjects)
            {
                Light[] gameObjectLights = FindLightsInGameObject(currentGo);
                allLights = allLights.Concat(gameObjectLights).ToArray();
            }

            return allLights;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="light"></param>
        /// <param name="settings"></param>
        private static void ConfigureSpotLight(Light light, LightingEditorSettings settings)
        {
#if HDRP_PIPELINE
#else
            light.intensity = settings.spotLightIntensity;
            light.cullingMask = settings.spotLightCullingMask;
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="light"></param>
        /// <param name="settings"></param>
        private static void ConfigurePointLight(Light light, LightingEditorSettings settings)
        {
            Debug.Log($"LightTools: ConfigurePointLight: Configuring {light.gameObject.name}...");
#if HDRP_PIPELINE
            LensFlare lensFlare = light.GetComponent<LensFlare>();
            if (lensFlare)
            {
                DestroyImmediate(lensFlare);
            }
            HDAdditionalLightData lightData = light.GetComponent<HDAdditionalLightData>();
            if (!lightData)
            {
                lightData = light.AddComponent<HDAdditionalLightData>();
            }

            lightData.intensity = settings.pointLightIntensity;
            lightData.range = settings.pointLightRange;
            lightData.shapeRadius = settings.pointLightRadius;
            light.renderingLayerMask = (int)settings.pointLightLightLayers;
            lightData.lightlayersMask = settings.pointLightLightLayers;
            lightData.EnableColorTemperature(true);
            lightData.surfaceTint = settings.pointLightsurfaceTint;
            light.colorTemperature = settings.pointLightcolorTemperature;

#else
            light.lightmapBakeType = settings.pointLightMode;
            light.range = settings.pointLightRange;
            light.intensity = settings.pointLightIntensity;
            light.bounceIntensity = settings.pointLightIndirectMultiplier;
            light.shadows = settings.pointLightShadowType;
            light.shadowRadius = settings.pointLightBakedShadowRadius;
            light.renderMode = settings.pointLightRenderMode;
            light.color = settings.pointLightColor;
            light.cullingMask = settings.pointLightCullingMask;
            light.shadowResolution = settings.pointLightShadowResolution;
#endif
            #if BAKERY
            ConfigureBakeryPointLight(light, settings);
            #endif
            Debug.Log($"LightTools: ConfigurePointLight: Configuring {light.gameObject.name} done.");
        }

        private static void ConfigureAreaLight(Light light, LightingEditorSettings settings)
        {
#if HDRP_PIPELINE
            HDAdditionalLightData lightData = light.GetComponent<HDAdditionalLightData>();
            if (!lightData)
            {
                lightData = light.AddComponent<HDAdditionalLightData>();
            }
            lightData.intensity = settings.spotLightIntensity;
            lightData.range = settings.spotLightRange;
            lightData.shapeRadius = settings.spotLightRadius;
            // int bitmask = (int)lightData.lightlayersMask;
            // bitmask = CalculateBitmask(bitmask, settings.lightLayers);
            // light.renderingLayerMask = bitmask;
            light.renderingLayerMask = (int)settings.spotLightLightLayers;
#else
            light.intensity = settings.areaLightIntensity;
            light.cullingMask = settings.areaLightcullingMask;
#endif
        }

        private static void ConfigureDirectionalLight(Light light, LightingEditorSettings settings)
        {
#if HDRP_PIPELINE
            HDAdditionalLightData lightData = light.GetComponent<HDAdditionalLightData>();
            if (!lightData)
            {
                lightData = light.AddComponent<HDAdditionalLightData>();
            }
#else
            light.intensity = settings.directionalLightIntensity;
            light.cullingMask = settings.directionalLightcullingMask;
#endif
        }

        /// <summary>
        /// Parse all lights, and configure those of the given type
        /// </summary>
        /// <param name="lights"></param>
        /// <param name="lightType"></param>
        /// <param name="settings"></param>
        public static void ConfigureLightsOfType(Light[] lights, LightType lightType, LightingEditorSettings settings)
        {
            foreach (Light light in lights)
            {
                if (light.type == lightType)
                {
                    switch (light.type)
                    {
                        case LightType.Point:
                            ConfigurePointLight(light, settings);
                            break;
                        case LightType.Spot:
                            ConfigureSpotLight(light, settings);
                            break;
                        case LightType.Area:
                            ConfigureAreaLight(light, settings);
                            break;
                        case LightType.Directional:
                            ConfigureDirectionalLight(light, settings);
                            break;
                    }
                }
            }
        }

        #if BAKERY
        private static void ConfigureBakeryPointLight(Light light, LightingEditorSettings settings)
        {
            BakeryPointLight bakeryPointLight = light.GetComponent<BakeryPointLight>();
            if (!bakeryPointLight)
            {
                bakeryPointLight = light.AddComponent<BakeryPointLight>();
            }

            bakeryPointLight.intensity = settings.pointLightIntensity;
            bakeryPointLight.cutoff = settings.pointLightRange;

        }
        #endif
        private static void MatchBakeryLight()
        {
 
        }
        
        #if BAKERY
        private static void ConfigureBakeryDirectLight(Light light, LightingEditorSettings settings)
        {
            BakeryDirectLight bakeryDirectLight = light.GetComponent<BakeryDirectLight>();
            if (!bakeryDirectLight)
            {
                bakeryDirectLight = light.AddComponent<BakeryDirectLight>();
            }
        }
        #endif
        public static int CalculateBitmask(int currentBitmask, Array enumValues)
        {
#if HDRP_PIPELINE
            //int originalBitVal = currentBitmask;
            LightLayerEnum targetLayer = LightLayerEnum.LightLayerDefault;
            foreach (LightLayerEnum current in enumValues)
            {
                // if everything is not set, the inverse in SetBitmask
                // will set all bits to 0, as if nothing was selected
                // so we can just ignore it here
                // it would probably also mess with decal layers
                if (current == LightLayerEnum.Everything) continue;
 
                int layerBitVal = (int)current;
 
                bool set = current == targetLayer;
                //if (set) Debug.Log("Set " + current);
                currentBitmask = SetBitmask(currentBitmask, layerBitVal, set);
            }
 
            //Debug.Log("| Bitmask : " + currentBitmask +
            //        "\r\n| Original: " + originalBitVal);
 
            return currentBitmask;
#else
            return 0;
#endif
            
        }

        public static int SetBitmask(int bitmask, int bitVal, bool set)
        {
#if HDRP_PIPELINE
            if (set)
                // or "|" will add the value, the 1 at the right position
                bitmask |= bitVal;
            else
                // and "&" will multiply the value, but we take the inverse
                // so the bit position is 0 while all others are 1
                // everything stays as it is, except for the one value
                // which will be set to 0
                bitmask &= ~bitVal;
 
            return bitmask;
#else
            return 0;
#endif

        }
    }
}

using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

#if HDRP_PIPELINE
using UnityEngine.Rendering.HighDefinition;
#else
using UnityEngine.Rendering;
#endif
namespace DaftAppleGames.Editor.Common.Performance
{
    public enum LightMode
    {
        Baked,
        Mixed,
        Realtime
    };

    /// <summary>
    /// Scriptable Object to store Editor usable instances of the Player Character Configuration
    /// </summary>
    [CreateAssetMenu(fileName = "LightSettings", menuName = "Settings/Lighting/LightSettings", order = 1)]
    public class LightingEditorSettings : EditorToolSettings
    {
        [BoxGroup("General Settings")]
        public bool createIfMissing = false;
#if HDRP_PIPELINE
        [FoldoutGroup("Spot Light Settings (HDRP)")]
        public string[] spotLightMeshSearchStrings;
        [FoldoutGroup("Spot Light Settings (HDRP)")]
        public float spotLightRange = 10.0f;
        [FoldoutGroup("Spot Light Settings (HDRP)")]
        public float spotLightIntensity = 600;
        [FoldoutGroup("Spot Light Settings (HDRP)")]
        public float spotLightRadius = 0.025f;
        [FoldoutGroup("Spot Light Settings (HDRP)")]
        public Color spotLightColor;
        [FoldoutGroup("Spot Light Settings (HDRP)")]
        public LightLayerEnum spotLightLightLayers;
#else
        [FoldoutGroup("Spot Light Settings")]
        public string[] spotLightMeshSearchStrings;

        [FoldoutGroup("Spot Light Settings")]
        public LightmapBakeType spotLightMode = LightmapBakeType.Mixed;
        [FoldoutGroup("Spot Light Settings")]
        public float spotLightRange = 3.0f;
        [FoldoutGroup("Spot Light Settings")]
        public float spotLightIntensity = 4;
        [FoldoutGroup("Spot Light Settings")]
        public Color spotLightColor;
        [FoldoutGroup("Spot Light Settings")]
        public LayerMask spotLightCullingMask;

#endif
        
#if HDRP_PIPELINE
        [FoldoutGroup("Point Light Settings (HDRP)")]
        public string[] pointLightMeshSearchStrings;
        [FoldoutGroup("Point Light Settings (HDRP)")]
        public float pointLightRange = 3.0f;
        [FoldoutGroup("Point Light Settings (HDRP)")]
        public float pointLightIntensity = 4;
        [FoldoutGroup("Point Light Settings (HDRP)")]
        public float pointLightRadius;
        [FoldoutGroup("Point Light Settings (HDRP)")]
        public LightLayerEnum pointLightLightLayers;
        [FoldoutGroup("Point Light Settings (HDRP)")]
        public Color pointLightsurfaceTint = new Color(227, 197, 100, 0);
        [FoldoutGroup("Point Light Settings (HDRP)")]
        public float pointLightcolorTemperature;
        
#else
        [FoldoutGroup("Point Light Settings")]
        public string[] pointLightMeshSearchStrings;
        [FoldoutGroup("Point Light Settings")]
        public LightmapBakeType pointLightMode = LightmapBakeType.Mixed;
        [FoldoutGroup("Point Light Settings")]
        public float pointLightRange = 3.0f;
        [FoldoutGroup("Point Light Settings")]
        public float pointLightIntensity = 4;
        [FoldoutGroup("Point Light Settings")]
        public float pointLightIndirectMultiplier = 2;
        [FoldoutGroup("Point Light Settings")]
        public LightShadows pointLightShadowType = LightShadows.Soft;
        [FoldoutGroup("Point Light Settings")]
        public LightShadowResolution pointLightShadowResolution = LightShadowResolution.VeryHigh;
        [FoldoutGroup("Point Light Settings")]
        public float pointLightBakedShadowRadius = 0.5f;
        [FoldoutGroup("Point Light Settings")]
        public LightRenderMode pointLightRenderMode = LightRenderMode.Auto;
        [FoldoutGroup("Point Light Settings")]
        public Color pointLightColor = new Color(227, 197, 100, 0);
        [FoldoutGroup("Point Light Settings")]
        public LayerMask pointLightCullingMask;
#endif
        
#if HDRP_PIPELINE
            [FoldoutGroup("Area Light Settings (HDRP)")]
            public string[] areaLightMeshSearchStrings;
            [FoldoutGroup("Area Light Settings (HDRP)")]
            public float areaLightRange = 3.0f;
            [FoldoutGroup("Area Light Settings (HDRP)")]
            public float areaLightIntensity = 4;
#else
        [FoldoutGroup("Area Light Settings")]
        public string[] areaLightMeshSearchStrings;
        [FoldoutGroup("Area Light Settings")]
        public float areaLightRange = 3.0f;
        [FoldoutGroup("Area Light Settings")]
        public float areaLightIntensity = 4;
        [FoldoutGroup("Area Light Settings")]
        public Color areaLightColor;
        [FoldoutGroup("Area Light Settings")]
        public LayerMask areaLightcullingMask;
#endif
            
#if HDRP_PIPELINE
        [FoldoutGroup("Directional Light Settings (HDRP)")]
        public string[] directionalLightMeshSearchStrings;
        [FoldoutGroup("Directional Light Settings (HDRP)")]
        public float directionalLightRange = 3.0f;
        [FoldoutGroup("Directional Light Settings (HDRP)")]
        public float directionalLightIntensity = 4;
            
#else
        [FoldoutGroup("Directional Light Settings")]
        public string[] directionalLightMeshSearchStrings;
        [FoldoutGroup("Directional Light Settings")]
        public float directionalLightRange = 3.0f;
        [FoldoutGroup("Directional Light Settings")]
        public float directionalLightIntensity = 4;
        [FoldoutGroup("Directional Light Settings")]
        public Color directionalLightColor;
        [FoldoutGroup("Directional Light Settings")]
        public LayerMask directionalLightcullingMask;
#endif
    }
}
using System;
using DaftAppleGames.Common.GameControllers;
using DaftAppleGames.Common.Utils;
using UnityEngine;
#if UNITY_POST_PROCESSING_STACK_V2
using UnityEngine.Rendering.PostProcessing;
#endif
#if HDPipeline
using UnityEngine.Rendering.HighDefinition;
#endif
#if GPU_INSTANCER
using GPUInstancer;
#endif

namespace DaftAppleGames.Common.Settings
{
    public class PerformanceSettingsManager : BaseSettingsManager, ISettings
    {
        [Header("Performance Defaults")]
        public int defaultTextureResolutionIndex = 0;
        public int defaultAntiAliasingIndex = 0;
        public string defaultQualityPresetName = "Low";
        public float defaultTerrainDetailLevel = 2.0f;
        public bool defaultEnableShadows = true;
        public int defaultAntiAliasingModeIndex = 1;
        
        [Header("Setting Keys")]
        private const string TextureResolutionIndexKey = "TextureResolution";
        private const string AntiAliasingIndexKey = "AntiAliasing";
        private const string QualityPresetNameKey = "QualityPreset";
        private const string TerrainDetailLevelKey = "TerrainDetail";
        private const string EnableShadowsKey = "EnableShadows";
        private const string AntiAliasingModeKey = "AntiAliasingMode";
        
        [Header("Core Game Objects")]
        public Light mainDirectionalLight;

        public int TextureResolutionIndex { get; set; }
        public int AntiAliasingIndex { get; set; }
        public string QualityPresetName { get; set; }
        public float TerrainDetailLevel { get; set; }
        public bool EnableShadows { get; set; }
        public int AntiAliasingModeIndex { get; set; }
        
        /// <summary>
        /// Save settings to PlayerPrefs
        /// </summary>
        public override void SaveSettings()
        {
            SettingsUtils.SaveIntSetting(TextureResolutionIndexKey, TextureResolutionIndex);
            SettingsUtils.SaveIntSetting(AntiAliasingIndexKey, AntiAliasingIndex);
            SettingsUtils.SaveStringSetting(QualityPresetNameKey, QualityPresetName);
            SettingsUtils.SaveFloatSetting(TerrainDetailLevelKey, TerrainDetailLevel);
            SettingsUtils.SaveBoolSetting(EnableShadowsKey, EnableShadows);
            SettingsUtils.SaveIntSetting(AntiAliasingModeKey, AntiAliasingModeIndex);
            
            base.SaveSettings();
        }

        /// <summary>
        /// Load settings from PlayerPrefs
        /// </summary>
        public override void LoadSettings()
        {
            TextureResolutionIndex = SettingsUtils.LoadIntSetting(TextureResolutionIndexKey, defaultTextureResolutionIndex);
            AntiAliasingIndex = SettingsUtils.LoadIntSetting(AntiAliasingIndexKey, defaultAntiAliasingIndex);
            QualityPresetName = SettingsUtils.LoadStringSetting(QualityPresetNameKey, defaultQualityPresetName);
            TerrainDetailLevel = SettingsUtils.LoadFloatSetting(TerrainDetailLevelKey, defaultTerrainDetailLevel);
            EnableShadows = SettingsUtils.LoadBoolSetting(EnableShadowsKey, defaultEnableShadows);
            AntiAliasingModeIndex = SettingsUtils.LoadIntSetting(AntiAliasingModeKey, defaultAntiAliasingModeIndex);
            
            base.LoadSettings();
        }

        /// <summary>
        /// Apply all current settings
        /// </summary>
        public override void ApplySettings()
        {
            ApplyQualityPresets();
            // ApplyTerrainDetailLevel();
            ApplyAntiAliasingMode();
            ApplyAntiAliasing();
            ApplyTextureResolution();
            base.ApplySettings();
        }
        
        /// <summary>
        /// Update and apply changes to shadow enabled
        /// </summary>
        /// <param name="value"></param>
        public void SetEnableShadows(bool value)
        {
            EnableShadows = value;
            ApplyEnableShadows();
        }

        /// <summary>
        /// Updates and applies the Texture Resolution
        /// </summary>
        /// <param name="value"></param>
        public void SetTextureResolution(int value)
        {
            TextureResolutionIndex = value;
            ApplyTextureResolution();
        }

        /// <summary>
        /// Updates and applies Anti Aliasing
        /// </summary>
        /// <param name="value"></param>
        public void SetAntiAliasing(int value)
        {
            AntiAliasingIndex = value;
            ApplyAntiAliasing();
        }

        /// <summary>
        /// Update and apply antialiasing mode
        /// </summary>
        /// <param name="value"></param>
        public void SetAntiAliasingMode(int value)
        {
            AntiAliasingModeIndex = value;
            ApplyAntiAliasingMode();
        }
        
        /// <summary>
        /// Updates and applies Quality Preset
        /// </summary>
        /// <param name="value"></param>
        public void SetQualityPreset(string value)
        {
            QualityPresetName = value;
            ApplyQualityPresets();
        }

        /// <summary>
        /// Sets the Terrain Details
        /// </summary>
        /// <param name="value"></param>
        public void SetTerrainDetailLevel(float value)
        {
            TerrainDetailLevel = value;
            // ApplyTerrainDetailLevel();
        }
        
        /// <summary>
        /// Apply the Texture Resolution
        /// </summary>
        private void ApplyTextureResolution()
        {
            QualitySettings.globalTextureMipmapLimit = TextureResolutionIndex;
            onSettingsAppliedEvent.Invoke();
        }

        /// <summary>
        /// Apply the Anti Aliasing
        /// </summary>
        private void ApplyAntiAliasing()
        {
            switch(AntiAliasingIndex)
            {
                case 0:
                    QualitySettings.antiAliasing = 8;
                    break;
                case 1:
                    QualitySettings.antiAliasing = 4;
                    break;
                case 2:
                    QualitySettings.antiAliasing = 0;
                    break;
            }
            onSettingsAppliedEvent.Invoke();
        }

        /// <summary>
        /// Apply the Anti Aliasing Mode
        /// </summary>
        private void ApplyAntiAliasingMode()
        {
            Camera mainCamera = PlayerCameraManager.Instance.MainCamera;
            
            #if UNITY_POST_PROCESSING_STACK_V2
            if (mainCamera)
            {
                PostProcessLayer postProcess = mainCamera.GetComponent<PostProcessLayer>();
                if (!postProcess)
                {
                    return;
                }

                switch (AntiAliasingModeIndex)
                {
                    case 0:
                        postProcess.antialiasingMode = PostProcessLayer.Antialiasing.None;
                        break;
                    case 1:
                        postProcess.antialiasingMode = PostProcessLayer.Antialiasing.FastApproximateAntialiasing;
                        postProcess.fastApproximateAntialiasing.fastMode = true;
                        break;
                    case 2:
                        postProcess.antialiasingMode = PostProcessLayer.Antialiasing.SubpixelMorphologicalAntialiasing;
                        break;
                    case 3:
                        postProcess.antialiasingMode = PostProcessLayer.Antialiasing.TemporalAntialiasing;
                        break;
                }
                onSettingsAppliedEvent.Invoke();
                Debug.Log($"PerformanceSettings: AntiAliasingMode is now {postProcess.antialiasingMode}");
            }
            #endif
        }
        
        /// <summary>
        /// Apply the Quality Presets
        /// </summary>
        private void ApplyQualityPresets()
        {
            // Lookup quality settings
            string[] qualityNames = QualitySettings.names;
            int qualityIndex = Array.IndexOf(qualityNames, QualityPresetName);

            if (qualityIndex >= 0)
            {
                QualitySettings.SetQualityLevel(qualityIndex, true);
            }
            else
            {
                Debug.LogError($"Quality Setting not found! {QualityPresetName}");
            }
            
            // Override presets with what's selected
            // Quality presets change the anti-aliasing and texture resolution. Update those settings appropriately
            ApplyTextureResolution();
            ApplyAntiAliasing();
            
            onSettingsAppliedEvent.Invoke();
        }

        /// <summary>
        /// Apply the Terrain settings
        /// </summary>
        private void ApplyTerrainDetailLevel()
        {
            #if GPU_INSTANCER
            GPUInstancerDetailManager gpuiDetailManager = GameUtils.FindGpuDetailManager().GetComponent<GPUInstancerDetailManager>();
            if(!gpuiDetailManager)
            {
                return;
            }
            // _mainTerrain.detailObjectDensity= _terrainDetailLevel;
            if (Math.Abs(gpuiDetailManager.terrainSettings.detailDensity - TerrainDetailLevel) > 0.0001f)
            {
                Debug.Log("Updating terrain detail...");
                foreach (GPUInstancerDetailPrototype p in gpuiDetailManager.prototypeList)
                {
                    if (p.detailDensity == gpuiDetailManager.terrainSettings.detailDensity || p.detailDensity > TerrainDetailLevel)
                    {
                        p.detailDensity = TerrainDetailLevel / 10.0f;
                    }
                }
                gpuiDetailManager.terrainSettings.detailDensity = TerrainDetailLevel;
                GPUInstancerAPI.UpdateDetailInstances(gpuiDetailManager, true);
            }
#endif
            onSettingsAppliedEvent.Invoke();
        }

        /// <summary>
        /// Apply the shadow enable settings
        /// </summary>
        private void ApplyEnableShadows()
        {
            if(!mainDirectionalLight)
            {
                mainDirectionalLight = GameUtils.FindMainDirectionalLight();
            }
#if HDPipeline
            HDAdditionalLightData hdLightData = mainDirectionalLight.GetComponent<HDAdditionalLightData>();
            if(EnableShadows)
            {
                hdLightData.SetShadowUpdateMode(ShadowUpdateMode.EveryFrame);
            }
            else
            {
                hdLightData.SetShadowUpdateMode(ShadowUpdateMode.OnEnable);
            }
#else
            if (mainDirectionalLight)
            {
                if (EnableShadows)
                {
                    mainDirectionalLight.shadows = LightShadows.Hard;
                }
                else
                {
                    mainDirectionalLight.shadows = LightShadows.None;
                }
            }
#endif
            onSettingsAppliedEvent.Invoke();      
        }
    }
}
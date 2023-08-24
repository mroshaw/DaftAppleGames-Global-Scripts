using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DaftAppleGames.Common.Settings
{
    public class PerformanceSettingsUiWindow : SettingsUiWindow, ISettingsUiWindow
    {
        [Header("UI Configuration")]
        public TMP_Dropdown textureResDropdown;
        public TMP_Dropdown antiAliasingDropdown;
        public TMP_Dropdown antiAliasingModeDropdown;
        public TMP_Dropdown qualityPresetDropdown;
        public Slider terrainDetailLevelSlider;
        public Toggle enableShadowsToggle;

        [Header("Settings Model")]
        public PerformanceSettingsManager performanceSettingsManager;
        
        /// <summary>
        /// Configure the UI control handlers to call public methods
        /// </summary>
        public override void InitControls()
        {
            // Remove all listeners, to prevent doubling up.
            textureResDropdown.onValueChanged.RemoveAllListeners();
            antiAliasingDropdown.onValueChanged.RemoveAllListeners();
            antiAliasingModeDropdown.onValueChanged.RemoveAllListeners();
            qualityPresetDropdown.onValueChanged.RemoveAllListeners();
            terrainDetailLevelSlider.onValueChanged.RemoveAllListeners();
            enableShadowsToggle.onValueChanged.RemoveAllListeners();

            // Configure the Audio setting sliders
            textureResDropdown.onValueChanged.AddListener(UpdateTextureResolution);
            antiAliasingDropdown.onValueChanged.AddListener(UpdateAntiAliasing);
            antiAliasingModeDropdown.onValueChanged.AddListener(UpdateAntiAliasingMode);
            qualityPresetDropdown.onValueChanged.AddListener(UpdateQualityPreset);
            terrainDetailLevelSlider.onValueChanged.AddListener(UpdateTerrainDetailLevel);
            enableShadowsToggle.onValueChanged.AddListener(UpdateEnableShadows);
            
            // Init dynamic drop downs
            InitQualitySettings();
        }

        /// <summary>
        /// Populate Quality Drop Down from Quality Settings
        /// </summary>
        private void InitQualitySettings()
        {
            qualityPresetDropdown.ClearOptions();
            List<string> qualityOptions = new List<string>(QualitySettings.names);
            qualityPresetDropdown.AddOptions(qualityOptions);
        }
        
        /// <summary>
        /// Initialise the controls with current settings
        /// </summary>
        public override void RefreshControlState()
        {
            base.RefreshControlState();
            textureResDropdown.SetValueWithoutNotify(performanceSettingsManager.TextureResolutionIndex);
            antiAliasingDropdown.SetValueWithoutNotify(performanceSettingsManager.AntiAliasingIndex);
            antiAliasingModeDropdown.SetValueWithoutNotify(performanceSettingsManager.AntiAliasingModeIndex);
            terrainDetailLevelSlider.SetValueWithoutNotify(performanceSettingsManager.TerrainDetailLevel);
            enableShadowsToggle.SetIsOnWithoutNotify(performanceSettingsManager.EnableShadows);
        
            int qualityIndex = qualityPresetDropdown.options.FindIndex(i => i.text == performanceSettingsManager.QualityPresetName);
            qualityPresetDropdown.SetValueWithoutNotify(qualityIndex);
        }

        /// <summary>
        /// Update the Enable Shadows setting
        /// </summary>
        /// <param name="enableShadows"></param>
        public void UpdateEnableShadows(bool enableShadows)
        {
            performanceSettingsManager.SetEnableShadows(enableShadows);
        }

        /// <summary>
        /// UI controller method to manage "Master Volume" UI changes
        /// </summary>
        /// <param name="textureResIndex"></param>
        public void UpdateTextureResolution(int textureResIndex)
        {
            performanceSettingsManager.SetTextureResolution(textureResIndex);
        }

        /// <summary>
        /// Handle Anti Aliasing value change
        /// </summary>
        /// <param name="antiAliasingIndex"></param>
        public void UpdateAntiAliasing(int antiAliasingIndex)
        {
            performanceSettingsManager.SetAntiAliasing(antiAliasingIndex);
        }

        /// <summary>
        /// Handle Anti Aliasing mode value change
        /// </summary>
        /// <param name="antiAliasingModeIndex"></param>
        public void UpdateAntiAliasingMode(int antiAliasingModeIndex)
        {
            performanceSettingsManager.SetAntiAliasingMode(antiAliasingModeIndex);
        }
        
        /// <summary>
        /// Handle Quality Preset value changed
        /// </summary>
        /// <param name="qualityPresetIndex"></param>
        public void UpdateQualityPreset(int qualityPresetIndex)
        {
            string qualityName = qualityPresetDropdown.options[qualityPresetIndex].text;
            performanceSettingsManager.SetQualityPreset(qualityName);
        }

        /// <summary>
        /// Handle Terrain Detail value changed
        /// </summary>
        /// <param name="terrainDetailLevel"></param>
        public void UpdateTerrainDetailLevel(float terrainDetailLevel)
        {
            performanceSettingsManager.SetTerrainDetailLevel(terrainDetailLevel);
        }
    }
}
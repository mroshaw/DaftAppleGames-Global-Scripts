using UnityEditor;
using UnityEngine;

namespace DaftAppleGames.Editor.AutoEditor
{
    public class LodGroupAutoEditor : BaseAutoEditor
    {
        [Header("LOD Group Editor Settings")]
        public float[] lodWeights = new float[] { 0.6f, 0.3f, 0.1f };
        public LODFadeMode fadeMode = LODFadeMode.CrossFade;
        public float cullRatio = 0.01f;

        [MenuItem("Window/Scene Tools/LOD Group Fixer")]
        public static void ShowWindow()
        {
            GetWindow(typeof(LodGroupAutoEditor));
        }

        /// <summary>
        /// Override base class to load specific Editor settings
        /// </summary>      
        public override void LoadSettings()
        {
            base.LoadSettings();
            LodGroupAutoEditorSettings lodGroupAutoEditorSettings = autoEditorSettings as LodGroupAutoEditorSettings;

            // Update editor specific config settings
            lodWeights = lodGroupAutoEditorSettings.lodWeights;
            fadeMode = lodGroupAutoEditorSettings.fadeMode;
            cullRatio = lodGroupAutoEditorSettings.cullRatio;
        }

        public override void RunEditorConfiguration()
        {
            LODGroup[] allLodGroups;

            outputArea = "Processing LOD Groups...\n";

            if (rootGameObjects.Count != 0)
            {
                allLodGroups = rootGameObjects[0].GetComponentsInChildren<LODGroup>(true);
            }
            else
            {
                allLodGroups = FindObjectsOfType<LODGroup>();
            }   

            outputArea += $"Found {allLodGroups.Length} LOD Groups...\n";

            int totalProcessed = 0;

            // Iterate and update
            foreach (LODGroup group in allLodGroups)
            {
                // outputArea += $"LOD Group processing: {group.gameObject.name}\n";
                ConfigureLodGroup(group);
                totalProcessed++;
                progressPercentage = totalProcessed / allLodGroups.Length * 100;
            }

            outputArea += $"LOD Group processed: {totalProcessed}";
        }

        private void ConfigureLodGroup(LODGroup group)
        {
            group.fadeMode = fadeMode;
            int numberOfLods = group.lodCount;
            LOD[] lods = group.GetLODs();

            if(numberOfLods == 0)
            {
                return;
            }

            float weightSum = 0;
            for (int k = 0; k < lods.Length; k++)
            {

                if (k >= lodWeights.Length)
                {
                    weightSum += lodWeights[lodWeights.Length - 1];
                }
                else
                {
                    weightSum += lodWeights[k];
                }
            }

            float maxLength = 1 - cullRatio;
            float curLodPos = 1;
            for (int j = 0; j < lods.Length; j++)
            {

                float weight = j < lodWeights.Length ? lodWeights[j] : lodWeights[lodWeights.Length - 1];

                float lengthRatio = weightSum != 0 ? weight / weightSum : 1;

                float lodLength = maxLength * lengthRatio;
                curLodPos = curLodPos - lodLength;

                lods[j].screenRelativeTransitionHeight = curLodPos;
            }

            group.SetLODs(lods);

            // Recalculate bounds
            group.RecalculateBounds();
        }
    }
}

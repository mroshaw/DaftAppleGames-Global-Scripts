using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace DaftAppleGames.Common.UI
{
    public class HudText : MonoBehaviour
    {
        [Header("Settings")]
        public float textDuration = 3.0f;
        public float fadeDuration = 0.5f;
        public TMP_Text fadeText;

        public Color startColor = new Color(0, 0, 0);
        public Color endColor = Color.white;

        /// <summary>
        /// Hide the text
        /// </summary>
        private void Start()
        {
            fadeText.color = startColor;
        }

        /// <summary>
        /// Fade in and fade out given text
        /// </summary>
        /// <param name="text"></param>
        public void ShowText(string text)
        {
            Debug.Log("In HudText.ShowText!");
            fadeText.text = text;
            StartCoroutine(FadeText());
        }

        /// <summary>
        /// Fade in, wait, and fade out the text
        /// </summary>
        /// <returns></returns>
        private IEnumerator FadeText()
        {
            // Fade in
            float time = 0;
            while (time < fadeDuration)
            {
                fadeText.color = Color.Lerp(startColor, endColor, time / fadeDuration);
                time += Time.deltaTime;
                yield return null;
            }
            fadeText.color = endColor;
         
            // Wait
            yield return new WaitForSeconds(textDuration);

            // Fade out
            time = 0;
            while (time < fadeDuration)
            {
                fadeText.color = Color.Lerp(endColor, startColor, time / fadeDuration);
                time += Time.deltaTime;
                yield return null;
            }
            fadeText.color = startColor;         
        }
    }
}

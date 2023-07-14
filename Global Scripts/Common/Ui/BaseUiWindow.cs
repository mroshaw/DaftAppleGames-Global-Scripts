using System;
using DaftAppleGames.Common.Characters;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace DaftAppleGames.Common.Ui
{
    /// <summary>
    /// Class to underpine all UI window components
    /// </summary>
    public class BaseUiWindow : MonoBehaviour
    {
        [BoxGroup("Window UI Settings")]
        public GameObject uiPanel;
        [BoxGroup("Window UI Settings")]
        public bool startWithUiOpen = false;
        [BoxGroup("Window UI Settings")]
        public GameObject startSelectedGameObject;
        [BoxGroup("Debug")]
        public bool isUiOpen = false;
        [FoldoutGroup("Show Events")]
        public UnityEvent onUiShowEvent;
        [FoldoutGroup("Hide Events")]
        public UnityEvent onUiHideEvent;
        
        private PausePlayerHelper _pausePlayer;
        
        /// <summary>
        /// Un-Register from the Ui controller
        /// </summary>
        private void OnDestroy()
        {
            if (UiController.Instance)
            {
                UiController.Instance.UnRegisterUiWindow(this);
            }
        }

        /// <summary>
        /// Init the UI
        /// </summary>
        public virtual void Start()
        {
            // Register with controller
            if (UiController.Instance)
            {
                UiController.Instance.RegisterUiWindow(this);
            }

            // Start with UI in specified state
            SetUiState(startWithUiOpen);
            _pausePlayer = GetComponent<PausePlayerHelper>();
        }

        /// <summary>
        /// Display the options UI
        /// </summary>
        public virtual void ShowUi()
        {
            // If PausePlayer component is present, use it.
            if (_pausePlayer)
            {
                _pausePlayer.PausePlayer();
            }

            // Enable the UI panel and set Event content
            uiPanel.SetActive(true);
            isUiOpen = true;
            if (startSelectedGameObject)
            {
                EventSystem.current.SetSelectedGameObject(startSelectedGameObject);
            }
            onUiShowEvent.Invoke();
        }

        /// <summary>
        /// Hide the options UI
        /// </summary>
        public virtual void HideUi()
        {
            // If PausePlayer component is present, use it.
            if (_pausePlayer)
            {
                _pausePlayer.UnpausePlayer();
            }
            
            // Disable the UI panel
            uiPanel.SetActive(false);
            isUiOpen = false;
            onUiHideEvent.Invoke();
        }

        /// <summary>
        /// Sets the appropriate UI state
        /// </summary>
        /// <param name="state"></param>
        private void SetUiState(bool state)
        {
            uiPanel.SetActive(state);
        }
    }
}
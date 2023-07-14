using System.Collections;
using DaftAppleGames.Common.Ui;
using UnityEngine;
using UnityEngine.Events;

namespace DaftAppleGames.Common.Tutorials
{
    /// <summary>
    /// MonoBehaviour class to manage display and tracking of tutorials
    /// </summary>
    public class Tutorial : MonoBehaviour
    {
        // Public serializable properties
        [Header("General Settings")]
        public TutorialContent tutorial;
        public float delay;
        
        [SerializeField]
        [Header("Tracking")]
        private bool _isDone = false;

        [Header("Events")]
        public UnityEvent onTutorialOpenEvent;
        public UnityEvent onTutorialCloseEvent;
        
        public bool IsDone
        {
            set => _isDone = value;
            get => _isDone;
        }
        
        // Private fields
        private InfoPanel _infoPanel;

        /// <summary>
        /// Initialise the component
        /// </summary>
        private void Start()
        {
            _infoPanel = GetComponentInChildren<InfoPanel>();
        }

        /// <summary>
        /// Displays the specified tutorial.
        /// </summary>
        /// <param name="force"></param>
        public void ShowTutorial(bool force)
        {
            StartCoroutine(ShowTutorialAsync(force));
            onTutorialOpenEvent.Invoke();
        }

        /// <summary>
        /// Public method to show tutorial is complete
        /// </summary>
        public void CloseTutorial()
        {
            onTutorialCloseEvent.Invoke();
        }
        
        /// <summary>
        /// Async coroutine wrapper for ShowTutorialAfterDelay
        /// </summary>
        /// <param name="force"></param>
        /// <returns></returns>
        private IEnumerator ShowTutorialAsync(bool force)
        {
            yield return new WaitForSeconds(delay);
            // Only show tutorial if it's not been done, or if we choose to force it
            if (!IsDone || force)
            {
                // Populate and show the InfoPanel
                _infoPanel.headingText.text = tutorial.heading;
                _infoPanel.contentText.text = tutorial.content;
                if (tutorial.image != null)
                {
                    _infoPanel.image.sprite = tutorial.image;
                }
                _infoPanel.ShowUi();
                IsDone = true;
            }            
        }
    }
}

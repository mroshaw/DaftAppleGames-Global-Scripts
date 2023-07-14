#if ASMDEF
#if BOOK
using DaftAppleGames.Common.GameControllers;
using echo17.EndlessBook;
#if INVECTOR_SHOOTER
using Invector.vCamera;
using Invector.vCharacterController;
#endif
using UnityEngine;
using UnityEngine.Events;

namespace DaftAppleGames.Common.Books
{
    /// <summary>
    /// Class to manage readable game objects like books and diaries
    /// </summary>
    public class Readable : MonoBehaviour
    {
        [Header("Settings")]
        public GameObject actionTextGameObject;
        public string playerTag;
        public BookController bookController;
        public GameObject renderGameObject;
        
        [Header("Action Settings")]
        public KeyCode actionKey = KeyCode.E;

        [Header("Events")]
        public UnityEvent OnEnterTriggerArea;
        public UnityEvent OnExitTriggerArea;
        public UnityEvent OnShowBookEvent;
        public UnityEvent OnHideBookEvent;

        private GameObject _bookControllerGameObject;

        private StateChangedDelegate onStateCompleted;

        /// <summary>
        /// Configure component on wake, before other components start
        /// </summary>
        private void Awake()
        {
            _bookControllerGameObject = bookController.gameObject;
            _bookControllerGameObject.SetActive(false);
            actionTextGameObject.SetActive(false);
            onStateCompleted = HideBookGameObjectDelegate;
        }
        
        /// <summary>
        /// Brings the interactive book up to the camera
        /// </summary>
        public void ShowBook()
        {
            actionTextGameObject.SetActive(false);

            // Enable the renderer components
            if (renderGameObject)
            {
                renderGameObject.SetActive(true);
            }
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            _bookControllerGameObject.SetActive(true);
            bookController.MoveBookToCamera();
            OnShowBookEvent.Invoke();
        }

        /// <summary>
        /// Hides the interactive book
        /// </summary>
        public void HideBook()
        {
            bookController.ResetBook(onStateCompleted);
            OnHideBookEvent.Invoke();
        }

        /// <summary>
        /// Hides the book
        /// </summary>
        private void HideBookGameObjectDelegate(EndlessBook.StateEnum fromState, EndlessBook.StateEnum toState, int pageNumber)
        {
            Cursor.lockState = CursorLockMode.Locked;

            // Hide the renderer components
            if (renderGameObject)
            {
                renderGameObject.SetActive(false);
            }
            
            _bookControllerGameObject.SetActive(false);
        }

        /// <summary>
        /// Handle action text when player walks into action trigger
        /// </summary>
        /// <param name="other"></param>
        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(playerTag))
            {
                actionTextGameObject.SetActive(true);
                OnEnterTriggerArea.Invoke();
            }
        }

        /// <summary>
        /// Handle action text when player walks leaves action trigger
        /// </summary>
        public void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag(playerTag))
            {
                actionTextGameObject.SetActive(false);
                OnExitTriggerArea.Invoke();
            }
        }

        /// <summary>
        /// Handle action key press while in action trigger
        /// </summary>
        /// <param name="other"></param>
        public void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag(playerTag))
            {
                if(Input.GetButtonDown("DoAction"))
                {
//                    if(_isShown)
//                    {
//                        HideBook();
//                    }
//                    else
//                    {
                        ShowBook();
//                    }
                }
            }
        }
    }
}
#endif
#endif
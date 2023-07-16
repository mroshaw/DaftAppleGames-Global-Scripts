using DaftAppleGames.Common.GameControllers;
#if PIXELCRUSHERS
#if INVECTOR_SHOOTER
using PixelCrushers.InvectorSupport;
using Invector.vCamera;
#endif
#endif
using UnityEngine;

namespace DaftAppleGames.Common.Characters 
{
    /// <summary>
    /// Helper component to call the PixelCrushers pause and unpause player functions
    /// </summary>
    public class PausePlayerHelper : MonoBehaviour
    {
        #if INVECTOR_SHOOTER
        private vThirdPersonCamera _vCamera;
        #endif
        /// <summary>
        /// Init the component
        /// </summary>
        private void Start()
        {
            RefreshvCamera();
        }

        /// <summary>
        /// Find the Invector Camera, if not found already
        /// </summary>
        private void RefreshvCamera()
        {
#if INVECTOR_SHOOTER
            if (!_vCamera)
            {
                _vCamera = PlayerCameraManager.Instance.InvectorMainCamera;
            }
#endif
        }
        
        /// <summary>
        /// Pause Invector player
        /// </summary>
        public void PausePlayer()
        {
#if PIXELCRUSHERS
#if INVECTOR_SHOOTER
            RefreshvCamera();
            if (_vCamera)
            {
                _vCamera.FreezeCamera();
            }
            Debug.Log("Pause Player");
            InvectorPlayerUtility.PausePlayer();
#endif
#endif
           
        }

        /// <summary>
        /// Unpause Invector player
        /// </summary>
        public void UnpausePlayer()
        {
#if PIXELCRUSHERS
#if INVECTOR_SHOOTER
            RefreshvCamera();
            if (_vCamera)
            {
                _vCamera.UnFreezeCamera();
            }
            Debug.Log("Un-Pause Player");
            InvectorPlayerUtility.UnpausePlayer();
#endif
#endif
        }
    }
}

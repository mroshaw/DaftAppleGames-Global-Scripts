using System.Collections;
using System.Collections.Generic;
using DaftAppleGames.Common.GameControllers;
using UnityEngine;

namespace DaftAppleGames
{
    public class CameraHelper : MonoBehaviour
    {
        public void EnableAudioListener()
        {
            PlayerCameraManager.Instance.MainCamera.GetComponent<AudioListener>().enabled = true;
        }

        public void DisableAudioListener()
        {
            PlayerCameraManager.Instance.MainCamera.GetComponent<AudioListener>().enabled = false;
        }
    }
}

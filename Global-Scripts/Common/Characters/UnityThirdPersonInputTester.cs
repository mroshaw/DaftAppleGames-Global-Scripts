#if INVECTOR_SHOOTER
using System;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.InputSystem;

namespace DaftAppleGames.Common.Characters
{
    public class UnityThirdPersonInputTester : MonoBehaviour
    {
        /// <summary>
        /// Respond to OnMove message
        /// Moves the player character
        /// </summary>
        /// <param name="value"></param>
        public void OnMove(InputValue value)
        {
            float x = value.Get<Vector2>().x;
            float y = value.Get<Vector2>().y;
            
            Debug.Log($"Move: ({x}, {y})");
        }

        /// <summary>
        /// Respond to the OnLook message
        /// </summary>
        /// <param name="value"></param>
        public void OnLook(InputValue value)
        {
            float x = value.Get<Vector2>().x;
            float y =  value.Get<Vector2>().y;
            
            Debug.Log($"Look: ({x}, {y})");
        }

        public void OnJump(InputValue value)
        {
            Debug.Log("Jump Pressed");
        }

        public void OnCrouch(InputValue value)
        {
            Debug.Log("Crouch Pressed");
        }

        public void OnStrafe(InputValue value)
        {
            Debug.Log("Strafe Pressed");
        }
        
        /// <summary>
        /// Respond toToggle Walk
        /// </summary>
        /// <param name="value"></param>
        public void OnToggleWalk(InputValue value)
        {
            Debug.Log($"Walk Toggle Pressed");
        }
    }
}
#endif
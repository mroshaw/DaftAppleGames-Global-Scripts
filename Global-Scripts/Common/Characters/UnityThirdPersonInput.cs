#if INVECTOR_SHOOTER
using System.Collections;
using Invector.vCamera;
using Invector.vCharacterController;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace DaftAppleGames.Common.Characters
{
    public class UnityThirdPersonInput : MonoBehaviour, vIAnimatorMoveReceiver
    {
        #region Events, Public, Private attribs
        [FoldoutGroup("Unity Events")]
        public delegate void OnUpdateEvent();
        [FoldoutGroup("Unity Events")]
        public event OnUpdateEvent onUpdate;
        [FoldoutGroup("Unity Events")]
        public event OnUpdateEvent onLateUpdate;
        [FoldoutGroup("Unity Events")]
        public event OnUpdateEvent onFixedUpdate;
        [FoldoutGroup("Unity Events")]
        public event OnUpdateEvent onAnimatorMove;
        
        [BoxGroup("Settings")]
        public bool unlockCursorOnStart = false;
        [BoxGroup("Settings")]
        public bool showCursorOnStart = false;
        // public KeyCode toggleWalk = KeyCode.CapsLock;

        [BoxGroup("Debug Movement")]
        public bool lockInput;

        [BoxGroup("Camera Settings")]
        public bool lockCameraInput;
        [BoxGroup("Camera Settings")]
        public bool invertCameraInputVertical, invertCameraInputHorizontal;

        [FoldoutGroup("Events")]
        public UnityEvent OnLockCamera;
        [FoldoutGroup("Events")]
        public UnityEvent OnUnlockCamera;
        [FoldoutGroup("Events")]
        public UnityEvent onEnableAnimatorMove = new UnityEvent();
        [FoldoutGroup("Events")]
        public UnityEvent onDisableDisableAnimatorMove = new UnityEvent();

        [HideInInspector]
        public vThirdPersonCamera tpCamera;         // access tpCamera info
        [HideInInspector]
        public bool ignoreTpCamera;                         // controls whether update the cameraStates of not                
        [HideInInspector]
        public string customCameraState;                    // generic string to change the CameraState
        [HideInInspector]
        public string customlookAtPoint;                    // generic string to change the CameraPoint of the Fixed Point Mode
        [HideInInspector]
        public bool changeCameraState;                      // generic bool to change the CameraState
        [HideInInspector]
        public bool smoothCameraState;                      // generic bool to know if the state will change with or without lerp
        [HideInInspector]
        public vThirdPersonController cc;                   // access the ThirdPersonController component
        [HideInInspector]
        public vHUDController hud;                          // acess vHUDController component
        protected bool updateIK = false;
        protected bool isInit;
        [BoxGroup("Debug Movement")]
        public bool lockMoveInput;

        protected Camera _cameraMain;
        protected bool withoutMainCamera;
        internal bool lockUpdateMoveDirection;                // lock the method UpdateMoveDirection
        private PlayerInput _playerInput;
        
        public Camera cameraMain
        {
            get
            {
                if (!_cameraMain && !withoutMainCamera)
                {
                    if (!Camera.main)
                    {
                        Debug.Log("Missing a Camera with the tag MainCamera, please add one.");
                        withoutMainCamera = true;
                    }
                    else
                    {
                        _cameraMain = Camera.main;
                        cc.rotateTarget = _cameraMain.transform;
                    }
                }
                return _cameraMain;
            }
            set
            {
                _cameraMain = value;
            }
        }

        public Animator animator
        {
            get
            {
                if (cc == null)
                {
                    cc = GetComponent<vThirdPersonController>();
                }

                if (cc.animator == null)
                {
                    return GetComponent<Animator>();
                }

                return cc.animator;
            }
        }
        #endregion
        #region Initialize Character, Camera & HUD when LoadScene

        protected virtual void Start()
        {
            cc = GetComponent<vThirdPersonController>();

            if (cc != null)
            {
                cc.Init();
            }

            StartCoroutine(CharacterInit());
            ShowCursor(showCursorOnStart);
            LockCursor(unlockCursorOnStart);
            EnableOnAnimatorMove();
        }

        protected virtual IEnumerator CharacterInit()
        {
            FindCamera();
            yield return new WaitForEndOfFrame();
            FindHUD();
        }

        public virtual void FindHUD()
        {
            if (hud == null && vHUDController.instance != null)
            {
                hud = vHUDController.instance;
                hud.Init(cc);
            }
        }

        public virtual void FindCamera()
        {
            var tpCameras = FindObjectsOfType<vThirdPersonCamera>();

            if (tpCameras.Length > 1)
            {
                tpCamera = System.Array.Find(tpCameras, tp => !tp.isInit);

                if (tpCamera == null)
                {
                    tpCamera = tpCameras[0];
                }

                if (tpCamera != null)
                {
                    for (int i = 0; i < tpCameras.Length; i++)
                    {
                        if (tpCamera != tpCameras[i])
                        {
                            Destroy(tpCameras[i].gameObject);
                        }
                    }
                }
            }
            else if (tpCameras.Length == 1)
            {
                tpCamera = tpCameras[0];
            }

            if (tpCamera && tpCamera.mainTarget != transform)
            {
                tpCamera.SetMainTarget(this.transform);
            }
        }

        #endregion
        #region Unity Events
        protected virtual void LateUpdate()
        {
            if (cc == null)
            {
                return;
            }

            if (!updateIK)
            {
                return;
            }

            if (onLateUpdate != null)
            {
                onLateUpdate.Invoke();
            }
            
            UpdateCameraStates();               // update camera states                        
            updateIK = false;
        }

        protected virtual void FixedUpdate()
        {
            if (onFixedUpdate != null)
            {
                onFixedUpdate.Invoke();
            }

            Physics.SyncTransforms();
            cc.UpdateMotor();                                                   // handle the ThirdPersonMotor methods            
            cc.ControlLocomotionType();                                         // handle the controller locomotion type and movespeed   
            ControlRotation();
            cc.UpdateAnimator();                                                // handle the ThirdPersonAnimator methods
            updateIK = true;
        }

        protected virtual void Update()
        {
            if (cc == null || Time.timeScale == 0)
            {
                return;
            }

            if (onUpdate != null)
            {
                onUpdate.Invoke();
            }
            
            UpdateHUD();                        // update hud graphics            
        }

        public virtual void OnAnimatorMoveEvent()
        {
            if (cc == null)
            {
                return;
            }

            cc.ControlAnimatorRootMotion();
            if (onAnimatorMove != null)
            {
                onAnimatorMove.Invoke();
            }
        }
        #endregion
        #region Generic Methods
        // you can call this methods anywhere in the inspector or third party assets to have better control of the controller or cutscenes

        /// <summary>
        /// Lock all Basic  Input from the Player
        /// </summary>
        /// <param name="value"></param>
        public virtual void SetLockBasicInput(bool value)
        {
            lockInput = value;
            if (value)
            {
                cc.input = Vector2.zero;
                cc.isSprinting = false;
                cc.animator.SetFloat("InputHorizontal", 0, 0.25f, Time.deltaTime);
                cc.animator.SetFloat("InputVertical", 0, 0.25f, Time.deltaTime);
                cc.animator.SetFloat("InputMagnitude", 0, 0.25f, Time.deltaTime);
            }
        }

        /// <summary>
        /// Lock all Inputs 
        /// </summary>
        /// <param name="value"></param>
        public virtual void SetLockAllInput(bool value)
        {
            SetLockBasicInput(value);
        }

        /// <summary>
        /// Show/Hide Cursor
        /// </summary>
        /// <param name="value"></param>
        public virtual void ShowCursor(bool value)
        {
            Cursor.visible = value;
        }

        /// <summary>
        /// Lock/Unlock the cursor to the center of screen
        /// </summary>
        /// <param name="value"></param>
        public virtual void LockCursor(bool value)
        {
            if (!value)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
            }
        }

        /// <summary>
        /// Lock the Camera Input
        /// </summary>
        /// <param name="value"></param>
        public virtual void SetLockCameraInput(bool value)
        {
            lockCameraInput = value;

            if (lockCameraInput)
            {
                OnLockCamera.Invoke();
            }
            else
            {
                OnUnlockCamera.Invoke();
            }
        }

        /// <summary>
        /// If you're using the MoveCharacter method with a custom targetDirection, check this true to align the character with your custom targetDirection
        /// </summary>
        /// <param name="value"></param>
        public virtual void SetLockUpdateMoveDirection(bool value)
        {
            lockUpdateMoveDirection = value;
        }

        /// <summary>
        /// Limits the character to walk only, useful for cutscenes and 'indoor' areas
        /// </summary>
        /// <param name="value"></param>
        public virtual void SetWalkByDefault(bool value)
        {
            cc.freeSpeed.walkByDefault = value;
            cc.strafeSpeed.walkByDefault = value;
        }

        /// <summary>
        /// Set the character to Strafe Locomotion
        /// </summary>
        /// <param name="value"></param>
        public virtual void SetStrafeLocomotion(bool value)
        {
            cc.lockInStrafe = value;
            cc.isStrafing = value;
        }

        /// <summary>
        /// OnAnimatorMove Event Sender 
        /// </summary>
        internal virtual vAnimatorMoveSender animatorMoveSender { get; set; }

        /// <summary>
        /// Use Animator Move Event Sender <seealso cref="vAnimatorMoveSender"/>
        /// </summary>
        protected bool _useAnimatorMove { get; set; }

        /// <summary>
        /// Check if OnAnimatorMove is Enabled
        /// </summary>
        public virtual bool UseAnimatorMove
        {
            get
            {
                return _useAnimatorMove;
            }
            set
            {

                if (_useAnimatorMove != value)
                {
                    if (value)
                    {
                        animatorMoveSender = gameObject.AddComponent<vAnimatorMoveSender>();
                        onEnableAnimatorMove?.Invoke();
                    }
                    else
                    {
                        if (animatorMoveSender)
                        {
                            Destroy(animatorMoveSender);
                        }

                        onEnableAnimatorMove?.Invoke();
                    }
                }
                _useAnimatorMove = value;
            }
        }

        /// <summary>
        /// Enable OnAnimatorMove event
        /// </summary>
        public virtual void EnableOnAnimatorMove()
        {
            UseAnimatorMove = true;
        }

        /// <summary>
        /// Disable OnAnimatorMove event
        /// </summary>
        public virtual void DisableOnAnimatorMove()
        {
            UseAnimatorMove = false;
        }

        #endregion
        #region Basic Locomotion Inputs
        /// <summary>
        /// Respond to OnMove message
        /// Moves the player character
        /// </summary>
        /// <param name="value"></param>
        public void OnMove(InputValue value)
        {
            if (lockInput || cc.ragdolled)
            {
                return;
            }
            
            if (!lockMoveInput)
            {
                // cc.input.x = value.Get<Vector2>().x;
                // cc.input.z = value.Get<Vector2>().y;
                cc.ControlKeepDirection();
            }
        }

        /// <summary>
        /// Respond toToggle Walk
        /// </summary>
        /// <param name="value"></param>
        public void OnToggleWalk(InputValue value)
        {
            cc.alwaysWalkByDefault = !cc.alwaysWalkByDefault;
        }
        
        /// <summary>
        /// Respond to OnStrafe
        /// </summary>
        /// <param name="value"></param>
        public void OnStrafe(InputValue value)
        {
            cc.Strafe();
        }
        
        /// <summary>
        /// Respond to OnSprint
        /// </summary>
        /// <param name="value"></param>
        public void OnSprint(InputValue value)
        {
            cc.Sprint(cc.useContinuousSprint);
        }

        /// <summary>
        /// Respond to OnCrouch
        /// </summary>
        /// <param name="value"></param>
        public void OnCrouch(InputValue value)
        {
            cc.AutoCrouch();
            cc.Crouch();
        }
        
        /// <summary>
        /// Respond to OnJump
        /// </summary>
        /// <param name="value"></param>
        public void OnJump(InputValue value)
        {
            if (JumpConditions())
            {
                cc.Jump(true);
            }
        }

        /// <summary>
        /// Respond to OnRoll
        /// </summary>
        /// <param name="value"></param>
        public void OnRoll(InputValue value)
        {
            if (RollConditions())
            {
                cc.Roll();
            }
        }
        #endregion       

        #region Helper Methods
        /// <summary>
        /// Conditions to trigger the Roll animation & behavior
        /// </summary>
        /// <returns></returns>
        protected virtual bool RollConditions()
        {
            return (!cc.isRolling || cc.canRollAgain) && cc.isGrounded && cc.input != Vector3.zero && !cc.customAction && cc.currentStamina > cc.rollStamina && !cc.isJumping && !cc.isSliding;
        }
        
        /// <summary>
        /// Conditions to trigger the Jump animation & behavior
        /// </summary>
        /// <returns></returns>
        protected virtual bool JumpConditions()
        {
            return !cc.inJumpStarted && !cc.customAction && !cc.isCrouching && cc.isGrounded && cc.GroundAngle() < cc.slopeLimit && cc.currentStamina >= cc.jumpStamina && !cc.isJumping && !cc.isRolling;
        }
        
        protected virtual bool rotateToLockTargetConditions => tpCamera && tpCamera.lockTarget && cc.isStrafing && !cc.isRolling && !cc.isJumping && !cc.customAction;
        public virtual void ControlRotation()
        {
            if (cameraMain && !lockUpdateMoveDirection)
            {
                if (!cc.keepDirection)
                {
                    cc.UpdateMoveDirection(cameraMain.transform);
                }
            }

            if (rotateToLockTargetConditions)
            {
                cc.RotateToPosition(tpCamera.lockTarget.position);          // rotate the character to a specific target
            }
            else
            {
                cc.ControlRotationType();                                   // handle the controller rotation type (strafe or free)
            }
        }
        #endregion
        #region Camera Methods

        /// <summary>
        /// Respond to the OnLook message
        /// </summary>
        /// <param name="value"></param>
        public void OnLook(InputValue value)
        {
            if (!cameraMain)
            {
                return;
            }

            if (tpCamera == null)
            {
                return;
            }

            float x = lockCameraInput ? 0f : value.Get<Vector2>().x;
            float y = lockCameraInput ? 0f : value.Get<Vector2>().y;
            
            if (invertCameraInputHorizontal)
            {
                x *= -1;
            }

            if (invertCameraInputVertical)
            {
                y *= -1;
            }
            
            // tpCamera.RotateCamera(x, y);
        }

        /// <summary>
        /// Respond to OnZoom
        /// </summary>
        /// <param name="value"></param>
        public void OnZoom(InputValue value)
        {
            if (!lockCameraInput)
            {
                tpCamera.Zoom(value.Get<float>());
            }
        }
        #endregion
        #region Camera Helper Methods
       public virtual void UpdateCameraStates()
        {
            // CAMERA STATE - you can change the CameraState here, the bool means if you want lerp of not, make sure to use the same CameraState String that you named on TPCameraListData
            if (ignoreTpCamera)
            {
                return;
            }

            if (tpCamera == null)
            {
                tpCamera = FindObjectOfType<vThirdPersonCamera>();
                if (tpCamera == null)
                {
                    return;
                }

                if (tpCamera)
                {
                    tpCamera.SetMainTarget(this.transform);
                    tpCamera.Init();
                }
            }

            if (changeCameraState)
            {
                tpCamera.ChangeState(customCameraState, customlookAtPoint, smoothCameraState);
            }
            else if (cc.isCrouching)
            {
                tpCamera.ChangeState("Crouch", true);
            }
            else if (cc.isStrafing)
            {
                tpCamera.ChangeState("Strafing", true);
            }
            else
            {
                tpCamera.ChangeState("Default", true);
            }
        }

        public virtual void ChangeCameraState(string cameraState, bool useLerp = true)
        {
            if (useLerp)
            {
                ChangeCameraStateWithLerp(cameraState);
            }
            else
            {
                ChangeCameraStateNoLerp(cameraState);
            }
        }

        public virtual void ResetCameraAngleSmooth()
        {
            if (tpCamera)
            {
                tpCamera.ResetAngle();
            }
        }

        public virtual void ResetCameraAngleWithoutSmooth()
        {
            if (tpCamera)
            {
                tpCamera.ResetAngleWithoutSmooth();
            }
        }

        public virtual void ChangeCameraStateWithLerp(string cameraState)
        {
            changeCameraState = true;
            customCameraState = cameraState;
            smoothCameraState = true;
        }

        public virtual void ChangeCameraStateNoLerp(string cameraState)
        {
            changeCameraState = true;
            customCameraState = cameraState;
            smoothCameraState = false;
        }

        public virtual void ResetCameraState()
        {
            changeCameraState = false;
            customCameraState = string.Empty;
        }

        #endregion
        #region HUD       

        public virtual void UpdateHUD()
        {
            if (hud == null)
            {
                if (vHUDController.instance != null)
                {
                    hud = vHUDController.instance;
                    hud.Init(cc);
                }
                else
                {
                    return;
                }
            }

            hud.UpdateHUD(cc);
        }

        #endregion
    }

    #region AnimatorMoveReceiver
    /// <summary>
    /// Interface to receive events from <seealso cref="vAnimatorMoveSender"/>
    /// </summary>
    public interface vIAnimatorMoveReceiver
    {
        /// <summary>
        /// Check if Component is Enabled
        /// </summary>
        bool enabled { get; set; }
        /// <summary>
        /// Method Called from <seealso cref="vAnimatorMoveSender"/>
        /// </summary>
        void OnAnimatorMoveEvent();
    }

    /// <summary>
    /// OnAnimatorMove Event Sender 
    /// </summary>
    class vAnimatorMoveSender : MonoBehaviour
    {
        private void Awake()
        {
            ///Hide in Inpector
            this.hideFlags = HideFlags.HideInInspector;
            vIAnimatorMoveReceiver[] animatorMoves = GetComponents<vIAnimatorMoveReceiver>();
            for (int i = 0; i < animatorMoves.Length; i++)
            {
                var receiver = animatorMoves[i];
                animatorMoveEvent += () =>
                {
                    if (receiver.enabled)
                    {
                        receiver.OnAnimatorMoveEvent();
                    }
                };
            }
        }

        /// <summary>
        /// AnimatorMove event called using  default unity OnAnimatorMove
        /// </summary>
        public System.Action animatorMoveEvent;

        private void OnAnimatorMove()
        {
            animatorMoveEvent?.Invoke();
        }
    }
    #endregion
}
#endif
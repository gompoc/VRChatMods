using System;
using System.Collections;
using HarmonyLib;
using MelonLoader;
using ModJsonGenerator;
using UnityEngine;
using UnityEngine.XR;
using Main = StandaloneThirdPerson.Main;

[assembly: MelonGame("VRChat", "VRChat")]
[assembly: MelonInfo(typeof(Main), "StandaloneThirdPerson", "1.3.1", "gompo & ljoonal", "https://github.com/gompoc/VRChatMods/releases/")]
[assembly: ModJsonInfo(
        242, 
    "A simple standalone third person mod\n" +
    "- Keybind configurable via uix/config file. Valid values can be found here: https://docs.unity3d.com/ScriptReference/KeyCode.html\n" +
    "- Third person camera fov and nearclipplane value can also be configured through uix/config file\n" +
    "- Rear camera can be move over to the left or right of your avatar using keybinds that are also configurable through uix/config file\n" +
    "- Has a \"freecam\". Keybind needs to be set first to use. You can use arrow keys to look up/down/left/right and I/J/K/L to move the camera", 
    new []{"Third person", "freecam", "freeview", "camera"}, 
    null, 
    null, 
    "#2ad9f7"
    )
]

namespace StandaloneThirdPerson
{
    internal partial class Main : MelonMod
    {
        private static CameraMode currentMode = CameraMode.Normal;
        private static CameraBehindMode cameraBehindMode = CameraBehindMode.Center;

        private static Camera thirdPersonCamera;
        private static Camera vrcCamera;
        private static bool initialised;

        internal static bool Allowed;

        public override void OnApplicationStart() => MelonCoroutines.Start(WaitForUIInit());

        private static IEnumerator WaitForUIInit()
        {
            while (VRCUiManager.prop_VRCUiManager_0 == null)
                yield return new WaitForEndOfFrame();
            if (XRDevice.isPresent)
                yield break;
            OnUIInit();
        }

        private static void OnUIInit()
        {
            ModSettings.RegisterSettings();
            ModSettings.LoadSettings();
            
            vrcCamera = GameObject.Find("Camera (eye)")?.GetComponent<Camera>();

            if (vrcCamera == null)
            {
                MelonLogger.Error("Couldn't find camera... mod won't run...");
                return;
            }

            var originalCameraTransform = vrcCamera.transform;
            thirdPersonCamera = new GameObject("Standalone ThirdPerson Camera").AddComponent<Camera>();
            thirdPersonCamera.fieldOfView = ModSettings.FOV;
            thirdPersonCamera.nearClipPlane = ModSettings.NearClipPlane;
            thirdPersonCamera.enabled = false;
            thirdPersonCamera.transform.parent = originalCameraTransform.parent;

            GameObject.Find("UserInterface/QuickMenu/MicControls").AddComponent<QMEnableDisableListener>();

            initialised = true;
        }

        private void RepositionCamera(bool isBehind, CameraBehindMode cameraBehindMode)
        {
            var vrcCameraTransform = vrcCamera.transform;
            var thirdPersonCameraTransform = thirdPersonCamera.transform;
            thirdPersonCameraTransform.parent = vrcCameraTransform;
            thirdPersonCameraTransform.position = vrcCameraTransform.position + (isBehind ? -vrcCameraTransform.forward : vrcCameraTransform.forward);
            thirdPersonCameraTransform.LookAt(vrcCameraTransform);
            if (isBehind)
            {
                if (cameraBehindMode == CameraBehindMode.RightShoulder)
                    thirdPersonCameraTransform.position += vrcCameraTransform.right * 0.5f;
                if (cameraBehindMode == CameraBehindMode.LeftShoulder)
                    thirdPersonCameraTransform.position -= vrcCameraTransform.right * 0.5f;
            }

            thirdPersonCameraTransform.position += thirdPersonCameraTransform.forward * 0.25f; // Reverse + = In  && - = Out
        }

        private static void FreeformCameraUpdate()
        {
            float h = 0;
            if (Input.GetKey(KeyCode.UpArrow)) h -= 1f;
            if (Input.GetKey(KeyCode.DownArrow)) h += 1f;
            float v = 0;
            if (Input.GetKey(KeyCode.LeftArrow)) v -= 1f;
            if (Input.GetKey(KeyCode.RightArrow)) v += 1f;
            thirdPersonCamera.transform.eulerAngles += new Vector3(h, v, 0);

            Vector3 movement = new();
            if (Input.GetKey(KeyCode.U)) movement += thirdPersonCamera.transform.up;
            if (Input.GetKey(KeyCode.O)) movement -= thirdPersonCamera.transform.up;
            if (Input.GetKey(KeyCode.L)) movement += thirdPersonCamera.transform.right;
            if (Input.GetKey(KeyCode.J)) movement -= thirdPersonCamera.transform.right;
            if (Input.GetKey(KeyCode.I)) movement += thirdPersonCamera.transform.forward;
            if (Input.GetKey(KeyCode.K)) movement -= thirdPersonCamera.transform.forward;

            thirdPersonCamera.transform.position += movement * Time.deltaTime * 2;
        }

        public static void UpdateCameraSettings()
        {
            if (thirdPersonCamera == null) return;
            thirdPersonCamera.fieldOfView = ModSettings.FOV;
            thirdPersonCamera.nearClipPlane = ModSettings.NearClipPlane;
            if (!ModSettings.Enabled)
                thirdPersonCamera.enabled = false;
        }

        public static void UpdateInputCheckerDel()
        {
            if (ModSettings.SecondaryKeyBind == KeyCode.None)
                keyBindInputDelegate = () => Input.GetKeyDown(ModSettings.KeyBind);
            else
                keyBindInputDelegate = () => Input.GetKey(ModSettings.SecondaryKeyBind) && Input.GetKeyDown(ModSettings.KeyBind);
            
            if (ModSettings.FreeformSecondaryKeyBind == KeyCode.None)
                freeformKeyBindInputDelegate = () => Input.GetKeyDown(ModSettings.FreeformKeyBind);
            else
                freeformKeyBindInputDelegate = () => Input.GetKey(ModSettings.FreeformSecondaryKeyBind) && Input.GetKeyDown(ModSettings.FreeformKeyBind);
        }
        private static ToggledDelegate keyBindInputDelegate;
        private static ToggledDelegate freeformKeyBindInputDelegate;
        delegate bool ToggledDelegate();

        public override void OnUpdate()
        {
            if (!initialised || !ModSettings.Enabled || !Allowed || Utils.IsBigMenuOpen() ||
                QMEnableDisableListener.Enabled)
                return;

            if (keyBindInputDelegate())
            {
                currentMode++;
                if (currentMode > CameraMode.InFront) currentMode = CameraMode.Normal;
                if (currentMode != CameraMode.Normal)
                {
                    RepositionCamera(currentMode == CameraMode.Behind, cameraBehindMode);
                    thirdPersonCamera.enabled = true;
                }
                else
                {
                    thirdPersonCamera.enabled = false;
                }
            }
            else if (ModSettings.FreeformEnabled && freeformKeyBindInputDelegate())
            {
                if (currentMode == CameraMode.Freeform)
                {
                    currentMode = CameraMode.Normal;
                    thirdPersonCamera.enabled = false;
                }
                else
                {
                    currentMode = CameraMode.Freeform;
                    thirdPersonCamera.transform.parent = null;
                    thirdPersonCamera.enabled = true;
                }
            }


            if (currentMode != CameraMode.Normal)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    currentMode = CameraMode.Normal;
                    thirdPersonCamera.enabled = false;
                }

                thirdPersonCamera.transform.position += thirdPersonCamera.transform.forward * Input.GetAxis("Mouse ScrollWheel");
                if (currentMode == CameraMode.Freeform)
                {
                    FreeformCameraUpdate();
                }
                else if (currentMode == CameraMode.Behind && ModSettings.RearCameraChangedEnabled)
                {
                    if (Input.GetKeyDown(ModSettings.MoveRearCameraLeftKeyBind))
                    {
                        cameraBehindMode--;
                        if (cameraBehindMode <= CameraBehindMode.LeftShoulder)
                            cameraBehindMode = CameraBehindMode.LeftShoulder;
                        RepositionCamera(true, cameraBehindMode);
                    }

                    if (Input.GetKeyDown(ModSettings.MoveRearCameraRightKeyBind))
                    {
                        cameraBehindMode++;
                        if (cameraBehindMode > CameraBehindMode.RightShoulder)
                            cameraBehindMode = CameraBehindMode.RightShoulder;
                        RepositionCamera(true, cameraBehindMode);
                    }
                }
            }
        }

        public override void OnPreferencesLoaded()
        {
            ModSettings.LoadSettings();
        }

        public override void OnPreferencesSaved()
        {
            ModSettings.LoadSettings();
        }
        
        [HarmonyPatch(typeof(NetworkManager), "OnJoinedRoom")]
        internal class OnJoinedRoomPatch
        {
            private static void Prefix()
            {
                currentMode = CameraMode.Normal;
                Allowed = false;
                MelonCoroutines.Start(Utils.CheckWorld());
            }
        }
    }
}
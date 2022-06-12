using System;
using MelonLoader;
using UnityEngine;

namespace StandaloneThirdPerson
{
    public static class ModSettings
    {
        private const string CATEGORY_NAME = "StandaloneThirdPerson";

        private static MelonPreferences_Entry<string> keyBind,
            freeformSecondaryKeyBind,
            secondaryKeyBind,
            freeformKeyBind,
            moveRearCameraLeftKeyBind,
            moveRearCameraRightKeyBind;

        private static MelonPreferences_Entry<float> fov, nearClipPlane;
        private static MelonPreferences_Entry<bool> enabled, rearCameraChangerEnabled, freeformEnabled;
        public static bool RearCameraChangedEnabled = true;


        public static KeyCode KeyBind { get; private set; } = KeyCode.T;
        public static KeyCode SecondaryKeyBind { get; private set; } = KeyCode.None;
        public static KeyCode FreeformKeyBind { get; private set; } = KeyCode.None;
        public static KeyCode FreeformSecondaryKeyBind { get; private set; } = KeyCode.None;
        public static KeyCode MoveRearCameraLeftKeyBind { get; private set; } = KeyCode.Q;
        public static KeyCode MoveRearCameraRightKeyBind { get; private set; } = KeyCode.E;
        public static float FOV { get; private set; } = 80;
        public static float NearClipPlane { get; private set; } = 0.01f;
        public static bool Enabled { get; private set; } = true;
        public static bool FreeformEnabled { get; private set; } = true;

        private static bool registered = false;


        public static void RegisterSettings()
        {
            var category = MelonPreferences.CreateCategory(CATEGORY_NAME, CATEGORY_NAME);
            keyBind = category.CreateEntry("Keybind", KeyBind.ToString(), "Toggle Third Person Keybind");
            secondaryKeyBind = category.CreateEntry("Secondary Keybind", SecondaryKeyBind.ToString(), "Toggle Third Person Secondary Keybind", "(Set to None to disable)");
            freeformKeyBind = category.CreateEntry("Freeform Keybind", FreeformKeyBind.ToString(), "Freeform Keybind");
            freeformSecondaryKeyBind = category.CreateEntry("Freeform Secondary Keybind", FreeformSecondaryKeyBind.ToString(), "Freeform Secondary Keybind");
            fov = category.CreateEntry("Camera FOV", FOV, "Camera FOV");
            nearClipPlane = category.CreateEntry("Camera NearClipPlane Value", NearClipPlane, "Camera NearClipPlane Value");
            enabled = category.CreateEntry("Mod Enabled", Enabled, "Mod Enabled");
            freeformEnabled = category.CreateEntry("Freeform Enabled", FreeformEnabled, "Freeform Enabled");
            rearCameraChangerEnabled = category.CreateEntry("Rear Camera Changer Enabled", RearCameraChangedEnabled, "Rear Camera Changer Enabled");
            moveRearCameraLeftKeyBind = category.CreateEntry("Move Rear Camera Left KeyBind", MoveRearCameraLeftKeyBind.ToString(), "Move Rear Camera Left KeyBind");
            moveRearCameraRightKeyBind = category.CreateEntry("Move Rear Camera Right KeyBind", MoveRearCameraRightKeyBind.ToString(), "Move Rear Camera Right KeyBind");
            registered = true;
        }

        public static void LoadSettings()
        {
            if (!registered)
                return;
            
            KeyBind = keyBind.TryParseKeyCodePref();
            SecondaryKeyBind = secondaryKeyBind.TryParseKeyCodePref(true);
            FreeformKeyBind = freeformKeyBind.TryParseKeyCodePref(true);
            FreeformSecondaryKeyBind = freeformSecondaryKeyBind.TryParseKeyCodePref(true);
            MoveRearCameraLeftKeyBind = moveRearCameraLeftKeyBind.TryParseKeyCodePref();
            MoveRearCameraRightKeyBind = moveRearCameraRightKeyBind.TryParseKeyCodePref();
            NearClipPlane = nearClipPlane.Value;
            FOV = fov.Value;
            Enabled = enabled.Value;
            FreeformEnabled = freeformEnabled.Value;
            RearCameraChangedEnabled = rearCameraChangerEnabled.Value;
            Main.UpdateCameraSettings();
            Main.UpdateInputCheckerDel();
        }

        private static KeyCode TryParseKeyCodePref(this MelonPreferences_Entry<string> pref, bool canBeNone = false)
        {
            if (pref is null)
            {
                MelonLogger.Error("TryParseKeyCodePref was passed a null pref, this shouldn't have happened, please send your log into the #log-scanner channel in the vrcmg and ping gompo");
            }
            if (pref.Value is null)
            {
                MelonLogger.Error("TryParseKeyCodePref was passed a pref with a null value, this shouldn't have happened, please send your log into the #log-scanner channel in the vrcmg and ping gompo");
            }
            try
            {
                if (!canBeNone && pref.Value.Equals("None"))
                    throw new ArgumentException();
                return ParseKeyCode(pref.Value);
            }
            catch (ArgumentException)
            {
                MelonLogger.Error($"Failed to parse keybind defaulting back to: {pref.DefaultValue}");
                pref.Value = pref.DefaultValue;
                return ParseKeyCode(pref.Value);
            }
        }

        private static KeyCode ParseKeyCode(string value)
        {
            return (KeyCode) Enum.Parse(typeof(KeyCode), value, true);
        }
    }
}
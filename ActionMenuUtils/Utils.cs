using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using MelonLoader;
using UnhollowerRuntimeLib.XrefScans;
using VRC.Animation;
using VRC.Core;
using VRC.SDKBase;

namespace ActionMenuUtils
{
    internal static class Utils
    {
        //Gracefully taken from Advanced Invites https://github.com/Psychloor/AdvancedInvites/blob/master/AdvancedInvites/Utilities.cs#L356
        private static bool XRefScanFor(this MethodBase methodBase, string searchTerm)
        {
            return XrefScanner.XrefScan(methodBase).Any(
                xref => xref.Type == XrefType.Global && xref.ReadAsObject()?.ToString().IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private static GoHomeDelegate GetGoHomeDelegate
        {
            get
            {
                if (goHomeDelegate != null) return goHomeDelegate;
                MethodInfo goHomeMethod = typeof(VRCFlowManager).GetMethods(BindingFlags.Public | BindingFlags.Instance).First(
                    m => m.GetParameters().Length == 0 && m.ReturnType == typeof(void) && m.XRefScanFor("Going to Home Location: "));

                goHomeDelegate = (GoHomeDelegate)Delegate.CreateDelegate(
                    typeof(GoHomeDelegate),
                    VRCFlowManager.prop_VRCFlowManager_0,
                    goHomeMethod);
                return goHomeDelegate;
            }
        }

        private static void GoHome() => GetGoHomeDelegate();
        private static GoHomeDelegate goHomeDelegate;
        private delegate void GoHomeDelegate();

        private static RespawnDelegate GetRespawnDelegate
        {
            get
            {
                if (respawnDelegate != null) return respawnDelegate;
                MethodInfo respawnMethod = typeof(VRCPlayer).GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).Single(
                    m => m.GetParameters().Length == 0 && m.ReturnType == typeof(void) && m.XRefScanFor("Respawned while not in a room."));

                respawnDelegate = (RespawnDelegate)Delegate.CreateDelegate(
                    typeof(RespawnDelegate),
                    VRCPlayer.field_Internal_Static_VRCPlayer_0,
                    respawnMethod);
                return respawnDelegate;
            }
        }
        
        private static RespawnDelegate respawnDelegate;
        private delegate void RespawnDelegate();
        
        public static void Respawn()
        {
            GetRespawnDelegate();
            VRCPlayer.field_Internal_Static_VRCPlayer_0.GetComponent<VRCMotionState>().Reset();
        }

        public static void RejoinInstance()
        {
            var instance = RoomManager.field_Internal_Static_ApiWorldInstance_0;
            Networking.GoToRoom($"{instance.world.id}:{instance.instanceId}");
        }

        public static void Home() => GetGoHomeDelegate();
        // {
        //     if (ModSettings.forceGoHome)
        //         GoHome();
        //     else
        //         GameObject.Find("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/Buttons_QuickActions/Button_GoHome").GetComponent<Button>().onClick.Invoke();
        // }

        public static void ResetAvatar()
        {
            if (ModSettings.enableCustomAvatarReset)
            {
                ApiAvatar avatar = null;
                ApiAvatar fallbackAvatar = null;

                if (IsValidAvatarId(ModSettings.customAvatarId))
                {
                    avatar = API.Fetch<ApiAvatar>(ModSettings.customAvatarId);
                }
                else
                {
                    SwitchToRobot(true);
                    return;
                }

                if (IsValidAvatarId(ModSettings.fallbackAvatarId))
                {
                    fallbackAvatar = API.Fetch<ApiAvatar>(ModSettings.fallbackAvatarId);
                }

                if (avatar is null)
                {
                    SwitchToRobot();
                    return;
                }

                SwitchAvatar(avatar, "AvatarMenu", fallbackAvatar);
            }
            else
            {
                SwitchToRobot();
            }
        }

        private static void SwitchToRobot(bool invalidId = false)
        {
            if (ModSettings.enableCustomAvatarReset)
            {
                MelonLogger.Warning(invalidId
                    ? "Couldn't switch to selected custom avatar. Make sure to select one first using the uix button on the left of the avatar page."
                    : "Couldn't switch to selected custom avatar. This likely means that the avatar you had selected before is no longer available.");
            }
            SwitchAvatar(API.Fetch<ApiAvatar>("avtr_c38a1615-5bf5-42b4-84eb-a8b6c37cbd11"), "fallbackAvatar");
        }
        
        private static bool IsValidAvatarId(string id)
        {
            return !string.IsNullOrEmpty(id) && AvatarIdValidationRegex.IsMatch(id);
        }

        private static void SwitchAvatar(ApiAvatar avatar, string thingy, ApiAvatar fallback = null)
        {
            ObjectPublicAbstractSealedApObApStApApUnique.Method_Public_Static_Void_ApiAvatar_String_ApiAvatar_0(avatar, thingy, fallback);
        }
        
        private static readonly Regex AvatarIdValidationRegex = new ("^avtr_[0-9|a-f]{8}-[0-9|a-f]{4}-[0-9|a-f]{4}-[0-9|a-f]{4}-[0-9|a-f]{12}$", RegexOptions.Compiled | RegexOptions.Singleline);
    }
}
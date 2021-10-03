using System;
using System.IO;
using System.Reflection;
using ActionMenuApi.Api;
using MelonLoader;
using ModJsonGenerator;
using UIExpansionKit.API;
using UnhollowerRuntimeLib;
using UnityEngine;
using VRC;
using Main = ActionMenuUtils.Main;

[assembly: MelonGame("VRChat", "VRChat")]
[assembly: MelonInfo(typeof(Main), "ActionMenuUtils", "2.0.0", "gompo", "https://github.com/gompoc/VRChatMods/releases/")]
[assembly: VerifyLoaderVersion(0, 4, 3, true)]
[assembly: ModJsonInfo(
        140, 
        "Lets you respawn using the action menu\n" +
        "Lets you go home for when respawning wont save you such as in broken worlds with no floors\n" +
        "Additionally lets you reset avatar or rejoin instance", 
        new []{"action menu", "respawn", "go home", "reset avatar"}, 
        new []{"[ActionMenuApi](https://api.vrcmg.com/v0/mods/201/ActionMenuApi.dll)", "[UIExpansionKit](https://api.vrcmg.com/v0/mods/55/UIExpansionKit.dll)"}, 
        "- Mod now depends on ActionMenuApi & UIExpansionKit, this just makes maintaining it easier for me when/if something breaks\n"+
        "- New setting added that you can enable so you can now select which avatar you want to reset into. You can select avatar by going to the AvatarMenu and clicking the UIExpansionKit button on the left",
        "#2ad9f7"
        )
]

namespace ActionMenuUtils
{
    public partial class Main : MelonMod
    {
        private static AssetBundle iconsAssetBundle;
        private static Texture2D respawnIcon;
        private static Texture2D helpIcon;
        private static Texture2D goHomeIcon;
        private static Texture2D resetAvatarIcon;
        private static Texture2D rejoinInstanceIcon;
        
        public override void OnApplicationStart()
        {
            try
            {
                if (string.IsNullOrEmpty(ID)) return;
                //Adapted from knah's JoinNotifier mod found here: https://github.com/knah/VRCMods/blob/master/JoinNotifier/JoinNotifierMod.cs 
                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ActionMenuUtils.icons"))
                {
                    using var tempStream = new MemoryStream((int)stream.Length);
                    stream.CopyTo(tempStream);

                    iconsAssetBundle = AssetBundle.LoadFromMemory_Internal(tempStream.ToArray(), 0);
                    iconsAssetBundle.hideFlags |= HideFlags.DontUnloadUnusedAsset;
                }

                respawnIcon = iconsAssetBundle.LoadAsset_Internal("Assets/Resources/Refresh.png", Il2CppType.Of<Texture2D>()).Cast<Texture2D>();
                respawnIcon.hideFlags |= HideFlags.DontUnloadUnusedAsset;
                helpIcon = iconsAssetBundle.LoadAsset_Internal("Assets/Resources/Help.png", Il2CppType.Of<Texture2D>()).Cast<Texture2D>();
                helpIcon.hideFlags |= HideFlags.DontUnloadUnusedAsset;
                goHomeIcon = iconsAssetBundle.LoadAsset_Internal("Assets/Resources/Home.png", Il2CppType.Of<Texture2D>()).Cast<Texture2D>();
                goHomeIcon.hideFlags |= HideFlags.DontUnloadUnusedAsset;
                resetAvatarIcon = iconsAssetBundle.LoadAsset_Internal("Assets/Resources/Avatar.png", Il2CppType.Of<Texture2D>()).Cast<Texture2D>();
                resetAvatarIcon.hideFlags |= HideFlags.DontUnloadUnusedAsset;
                rejoinInstanceIcon = iconsAssetBundle.LoadAsset_Internal("Assets/Resources/Pin.png", Il2CppType.Of<Texture2D>()).Cast<Texture2D>();
                rejoinInstanceIcon.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            }
            catch (Exception e) {
                MelonLogger.Warning("Consider checking for newer version as mod possibly no longer working, Exception occured OnAppStart(): " + e.Message);
            }
            ModSettings.RegisterSettings();
            ModSettings.Apply();
            SetupAMAPIButtons();
            SetupUIXButtons();
        }

        private static void SetupUIXButtons()
        {
            ExpansionKitApi.GetExpandedMenu(ExpandedMenu.AvatarMenu).AddSimpleButton("Set as reset avatar for ActionMenuUtils", 
                () =>
                {
                    var avatarId = GameObject.Find("UserInterface/MenuContent/Screens/Avatar/AvatarPreviewBase/MainRoot/MainModel").GetComponent<SimpleAvatarPedestal>().field_Internal_ApiAvatar_0.id;
                    var fallbackAvatarId =GameObject.Find("UserInterface/MenuContent/Screens/Avatar/AvatarPreviewBase/FallbackRoot/FallbackModel").GetComponent<SimpleAvatarPedestal>().field_Internal_ApiAvatar_0.id;
                    ModSettings.customAvatarId = avatarId;
                    ModSettings.fallbackAvatarId = fallbackAvatarId;
                    ModSettings.Save();
#if DEBUG
                    MelonLogger.Msg($"{avatarId},{fallbackAvatarId}");
#endif

                },
                g =>
                {
                    UIXAvatarMenuButton = g;
                    UIXAvatarMenuButton.active = ModSettings.enableCustomAvatarReset;
                });
            
        }

        public static GameObject UIXAvatarMenuButton;
        
        private static void SetupAMAPIButtons()
        {
            VRCActionMenuPage.AddSubMenu(ActionMenuPage.Options, "SOS",
                () =>
                {
                    //Respawn
                    if (ModSettings.confirmRespawn)
                        CustomSubMenu.AddSubMenu("Respawn", 
                            () => CustomSubMenu.AddButton("Confirm Respawn", Utils.Respawn, respawnIcon),
                            respawnIcon
                        );
                    else
                        CustomSubMenu.AddButton("Respawn", Utils.Respawn, respawnIcon);

                    //Reset Avatar
                    if (ModSettings.confirmAvatarReset)
                        CustomSubMenu.AddSubMenu("Reset Avatar",
                            () => CustomSubMenu.AddButton("Confirm Reset Avatar", Utils.ResetAvatar, resetAvatarIcon), 
                            resetAvatarIcon
                        );
                    else
                        CustomSubMenu.AddButton("Reset Avatar", Utils.ResetAvatar, resetAvatarIcon);
                   
                    //Instance Rejoin
                    if (ModSettings.confirmInstanceRejoin)
                        CustomSubMenu.AddSubMenu("Rejoin Instance", 
                            () => CustomSubMenu.AddButton("Confirm Instance Rejoin", Utils.RejoinInstance, rejoinInstanceIcon), 
                            rejoinInstanceIcon
                        );
                    else
                        CustomSubMenu.AddButton("Rejoin Instance", Utils.RejoinInstance, rejoinInstanceIcon);
                    
                    //Go Home
                    if (ModSettings.confirmGoHome)
                        CustomSubMenu.AddSubMenu("Go Home", 
                            () => CustomSubMenu.AddButton("Confirm Go Home",Utils.Home, goHomeIcon), 
                            goHomeIcon
                        );
                    else
                        CustomSubMenu.AddButton("Go Home",Utils.Home, goHomeIcon);

                }, helpIcon
            );
        }

        public override void OnPreferencesLoaded() => ModSettings.Apply();
        public override void OnPreferencesSaved() => ModSettings.Apply();
        
        private static string ID = "gompo";
    }
}
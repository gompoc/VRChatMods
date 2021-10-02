using MelonLoader;
using ModJsonGenerator;
using UIExpansionKit.API;
using WorldPredownload.UI;

[assembly: MelonInfo(typeof(WorldPredownload.WorldPredownload), "WorldPredownload", "1.6.2", "gompo", "https://github.com/gompocp/VRChatMods/releases/")]
[assembly: MelonGame("VRChat", "VRChat")]
[assembly: VerifyLoaderVersion(0, 4, 3, true)]
[assembly: ModJsonInfo(
        141,
        "-Lets you predownload worlds from the world info page, friend info page and in the more options tab for invites notifications\n" +
        "-Download Status can be found on hud and as well on the bottom of the quickmenu.\n" +
        "-Switching world while downloading will cancel current download.\n" +
        "-Mod now checks to see if world has been downloaded previously on world info page and user info page.\n" +
        "-Mod is useful if you have particularly bad internet and don't fancy hanging around in the loading screen for a decade.\n" +
        "-Options to follow predownloads configurable via UIX too",
        new []{"world", "download", "preload", "predownload"},
        new []{"[UIExpansionKit](https://api.vrcmg.com/v0/mods/55/UIExpansionKit.dll)"},
        null,
        "#2ad9f7"
    )
]

namespace WorldPredownload
{
    internal partial class WorldPredownload : MelonMod
    {
        private static MelonMod instance;
        
        public new static HarmonyLib.Harmony HarmonyInstance => instance.HarmonyInstance;

        public override void OnApplicationStart()
        {
            instance = this;
            ModSettings.RegisterSettings();
            ModSettings.LoadSettings();
            SocialMenuSetup.Patch();
            WorldInfoSetup.Patch();
            NotificationMoreActions.Patch();
            ExpansionKitApi.OnUiManagerInit += UiManagerInit;
        }

        private void UiManagerInit()
        {
            if (string.IsNullOrEmpty(ID)) return;
            InviteButton.Setup();
            FriendButton.Setup();
            WorldButton.Setup();
            WorldDownloadStatus.Setup();
            HudIcon.Setup();
        }

        public override void OnPreferencesLoaded()
        {
            ModSettings.LoadSettings();
        }

        public override void OnPreferencesSaved()
        {
            ModSettings.LoadSettings();
        }
        
        private static readonly string ID = "gompo";
    }
}
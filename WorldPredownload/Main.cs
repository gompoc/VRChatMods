using MelonLoader;
using UIExpansionKit.API;
using WorldPredownload.UI;


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
            //NotificationMoreActions.Patch();
            ExpansionKitApi.OnUiManagerInit += UiManagerInit;
        }

        private void UiManagerInit()
        {
            if (string.IsNullOrEmpty(ID)) return;
            InviteButton.Setup();
            FriendButton.Setup();
            WorldButton.Setup();
            //WorldDownloadStatus.Setup();
            HudIcon.Setup();
            PortalButton.Setup();
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

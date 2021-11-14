using MelonLoader;

namespace ActionMenuUtils
{
    public static class ModSettings
    {
        private static string categoryName = "ActionMenuRespawn"; //Old Name
        private static string categoryDisplayName = "ActionMenuUtils"; //New Name
        public static bool confirmRespawn { get; private set; } = false;
        public static bool confirmGoHome { get; private set; } = false;
        public static bool confirmAvatarReset { get; private set; } = false;
        public static bool confirmInstanceRejoin { get; private set; } = true;
        //public static bool forceGoHome { get; private set; } = false;

        public static bool enableCustomAvatarReset { get; private set; } = false;

        public static string customAvatarId;
        public static string fallbackAvatarId;
        

        private static MelonPreferences_Entry<bool> ConfirmRespawn,
            ConfirmGoHome,
            ConfirmAvatarReset,
            ConfirmInstanceRejoin,
            //ForceGoHome,
            EnableCustomAvatarReset;

        private static MelonPreferences_Entry<string> CustomAvatarId, FallbackAvatarId;

        public static void RegisterSettings()
        {
            var category = MelonPreferences.CreateCategory(categoryName, categoryDisplayName);
            ConfirmRespawn = category.CreateEntry("ConfirmRespawn", confirmRespawn, "Add a confirmation for respawn");
            ConfirmGoHome = category.CreateEntry( "ConfirmGoHome", confirmGoHome, "Add a confirmation for go home");
            //ForceGoHome = category.CreateEntry("ForceGoHome", forceGoHome, "Skip the go home popup");
            ConfirmAvatarReset = category.CreateEntry("ConfirmAvatarReset", confirmAvatarReset, "Add a confirmation for avatar reset");
            ConfirmInstanceRejoin = category.CreateEntry("ConfirmInstanceReJoin", confirmInstanceRejoin, "Add a confirmation for rejoin instance");
            EnableCustomAvatarReset = category.CreateEntry("EnableCustomAvatarReset", enableCustomAvatarReset, "Use custom avatar when resetting");
            CustomAvatarId = category.CreateEntry("CustomAvatarId", customAvatarId, "", true) as MelonPreferences_Entry<string>;
            FallbackAvatarId = category.CreateEntry("FallbackAvatarId", fallbackAvatarId, "", true) as MelonPreferences_Entry<string>;
        }

        public static void Apply()
        {
            confirmRespawn = ConfirmRespawn.Value;
            confirmGoHome = ConfirmGoHome.Value;
            //forceGoHome = ForceGoHome.Value;
            confirmAvatarReset = ConfirmAvatarReset.Value;
            confirmInstanceRejoin = ConfirmInstanceRejoin.Value;
            enableCustomAvatarReset = EnableCustomAvatarReset.Value;
            customAvatarId = CustomAvatarId.Value;
            fallbackAvatarId = FallbackAvatarId.Value;
            
            if(Main.UIXAvatarMenuButton is not null) 
                Main.UIXAvatarMenuButton.active = enableCustomAvatarReset;
        }

        public static void Save()
        {
            CustomAvatarId.Value = customAvatarId;
            FallbackAvatarId.Value = fallbackAvatarId;
            MelonPreferences.Save();
        }
    }
}
using System;
using System.Collections;
using ActionMenuApi.Helpers;
using ActionMenuApi.Managers;
using MelonLoader;
using ModJsonGenerator;

#pragma warning disable 1591

[assembly: MelonInfo(typeof(ActionMenuApi.ActionMenuApi), "ActionMenuApi", "1.0.0", "gompo", "https://github.com/gompoc/VRChatMods/releases")]
[assembly: MelonGame("VRChat", "VRChat")]

[assembly: ModJsonInfo(
    201, 
    "This mod doesn't do anything on it's own. \n" +
    "It provides an easy way for modders to add integration with the action menu.\n" +
    "It supports the use of the:\n" +
    "- Radial Puppet\n" +
    "- Four Axis Puppet\n" +
    "- Button\n" +
    "- Toggle Button\n" +
    "- Sub Menus\n" +
    "\n" +
    "Additionally allows mods to add their menus to a dedicated section on the action menu to prevent clutter.\n" +
    "Example mod and documentation can be found on github",
    new []{"action menu", "api", "radial menu"}, 
    null,
    "Fix crap for ui update", 
    "#2ad9f7"
    )
]

namespace ActionMenuApi
{
    public partial class ActionMenuApi : MelonMod
    {
        
        public override void OnApplicationStart()
        {
            ResourcesManager.LoadTextures();
            MelonCoroutines.Start(WaitForActionMenuInit());
            try
            {
                Patches.PatchAll(HarmonyInstance);
            }
            catch (Exception e)
            {
                MelonLogger.Error($"Patching failed with exception: {e.Message}");
            }
        }

        private IEnumerator WaitForActionMenuInit()
        {
            while (ActionMenuDriver.prop_ActionMenuDriver_0 == null) //VRCUIManager Init is too early 
                yield return null;
            if (string.IsNullOrEmpty(ID)) yield break;
            ResourcesManager.InitLockGameObject();
            RadialPuppetManager.Setup();
            FourAxisPuppetManager.Setup();
        }
        
        public override void OnUpdate()
        {
            RadialPuppetManager.OnUpdate();
            FourAxisPuppetManager.OnUpdate();
        }

        private static string ID = "gompo";
    }
}

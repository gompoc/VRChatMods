using System.Reflection;
using ActionMenuUtils;
using MelonLoader;
using ModJsonGenerator;



[assembly: MelonGame("VRChat", "VRChat")]
[assembly: MelonInfo(typeof(ActionMenuUtils.Main), ModConstants.NAME, ModConstants.VERSION, ModConstants.AUTHOR, ModConstants.DOWNLOAD_LINK)]
[assembly: ModJsonInfo(
        140, 
        "Lets you respawn, go home, rejoin instance and reset you avatar to the default robot or a custom one all using the action menu", 
        new []{"action menu", "respawn", "go home", "reset avatar"}, 
        new []{"[ActionMenuApi](https://api.vrcmg.com/v0/mods/201/ActionMenuApi.dll)", "[UIExpansionKit](https://api.vrcmg.com/v0/mods/55/UIExpansionKit.dll)"}, 
        "Fixed issues with respawn and go home buttons", 
        "#2ad9f7"
    )
]

[assembly:AssemblyVersion(ModConstants.VERSION)]
[assembly:AssemblyFileVersion(ModConstants.VERSION)]
[assembly:AssemblyTitle(ModConstants.NAME)]
[assembly:AssemblyDescription(ModConstants.NAME)]
[assembly:AssemblyCopyright("Created by " + ModConstants.AUTHOR)]

namespace ActionMenuUtils;
public static class ModConstants
{
    public const string VERSION = "2.0.4";
    public const string NAME = "ActionMenuUtils";
    public const string AUTHOR = "gompo";
    public const string DOWNLOAD_LINK = "https://github.com/gompoc/VRChatMods/releases/";
}
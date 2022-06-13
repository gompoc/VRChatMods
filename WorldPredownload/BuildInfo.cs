using System;
using System.Reflection;
using MelonLoader;
using ModJsonGenerator;
using WorldPredownload;


[assembly: MelonGame("VRChat", "VRChat")]
[assembly: MelonInfo(typeof(WorldPredownload.WorldPredownload), ModConstants.NAME, ModConstants.VERSION, ModConstants.AUTHOR, ModConstants.DOWNLOAD_LINK)]
[assembly: ModJsonInfo(
        141,
        "-Lets you predownload worlds from the world info page and friend info page \n" +
        "-Download Status can be found on hud and as well on the bottom of the quickmenu.\n" +
        "-Switching world while downloading will cancel current download.\n" +
        "-Mod now checks to see if world has been downloaded previously on world info page and user info page.\n" +
        "-Mod is useful if you have particularly bad internet and don't fancy hanging around in the loading screen for a decade.\n" +
        "-Options to follow predownloads configurable via UIX too",
        new []{"world", "download", "preload", "predownload"},
        new []{"[UIExpansionKit](https://api.vrcmg.com/v0/mods/55/UIExpansionKit.dll)"},
        "Compatibility fix for update and add ability to download worlds via portals (credit to yobson)",
        "#2ad9f7"
    )
]

[assembly:AssemblyVersion(ModConstants.VERSION)]
[assembly:AssemblyFileVersion(ModConstants.VERSION)]
[assembly:AssemblyTitle(ModConstants.NAME)]
[assembly:AssemblyDescription(ModConstants.NAME)]
[assembly:AssemblyCopyright("Created by " + ModConstants.AUTHOR)]

namespace WorldPredownload;
public static class ModConstants
{
    public const string VERSION = "1.7.0";
    public const string NAME = "WorldPredownload";
    public const string AUTHOR = "gompo, yobson";
    public const string DOWNLOAD_LINK = "https://github.com/gompoc/VRChatMods/releases/";
}
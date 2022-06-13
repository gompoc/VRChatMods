using System;
using System.Reflection;
using MelonLoader;
using ModJsonGenerator;
using UpdateChecker;
using Main = UpdateChecker.Main;

[assembly: MelonGame("VRChat", "VRChat")]
[assembly: MelonInfo(typeof(Main), ModConstants.NAME, ModConstants.VERSION, ModConstants.AUTHOR, ModConstants.DOWNLOAD_LINK)]
[assembly: MelonColor(ConsoleColor.Magenta)]
[assembly: ModJsonInfo(
        249, 
        "- Simple mod for people who dislike auto updaters\n" +
        "- Checks for mod updates and lets you know in the console\n" +
        "- Only works for mods that follow the semver format",
        null,
        null,
        "Updated to api/v1 (Credit to DubyaDude)",
        "#2ad9f7"
    )
]
[assembly:AssemblyVersion(ModConstants.VERSION)]
[assembly:AssemblyFileVersion(ModConstants.VERSION)]
[assembly:AssemblyTitle(ModConstants.NAME)]
[assembly:AssemblyDescription(ModConstants.NAME)]
[assembly:AssemblyCopyright("Created by " + ModConstants.AUTHOR)]

namespace UpdateChecker;
public static class ModConstants
{
    public const string VERSION = "1.0.1";
    public const string NAME = "UpdateChecker";
    public const string AUTHOR = "gompo, DubyaDude";
    public const string DOWNLOAD_LINK = "https://github.com/gompoc/VRChatMods/releases/";
}
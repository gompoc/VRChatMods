using System.Reflection;
using ActionMenuApi;
using MelonLoader;
using ModJsonGenerator;



[assembly: MelonGame("VRChat", "VRChat")]
[assembly: MelonInfo(typeof(ActionMenuApi.ActionMenuApi), ModConstants.NAME, ModConstants.VERSION, ModConstants.AUTHOR, ModConstants.DOWNLOAD_LINK)]
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
        "TODO", 
        "#2ad9f7"
    )
]
[assembly:AssemblyVersion(ModConstants.VERSION)]
[assembly:AssemblyFileVersion(ModConstants.VERSION)]
[assembly:AssemblyTitle(ModConstants.NAME)]
[assembly:AssemblyDescription(ModConstants.NAME)]
[assembly:AssemblyCopyright("Created by " + ModConstants.AUTHOR)]

namespace ActionMenuApi;
public static class ModConstants
{
    public const string VERSION = "1.0.0";
    public const string NAME = "ActionMenuApi";
    public const string AUTHOR = "gompo";
    public const string DOWNLOAD_LINK = "https://github.com/gompoc/VRChatMods/releases/";
}
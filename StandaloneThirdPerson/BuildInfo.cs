using System;
using System.Reflection;
using MelonLoader;
using ModJsonGenerator;
using StandaloneThirdPerson;


[assembly: MelonGame("VRChat", "VRChat")]
[assembly: MelonInfo(typeof(StandaloneThirdPerson.Main), ModConstants.NAME, ModConstants.VERSION, ModConstants.AUTHOR, ModConstants.DOWNLOAD_LINK)]
[assembly: ModJsonInfo(
        242, 
        "A simple standalone third person mod\n" +
        "- Keybind configurable via uix/config file. Valid values can be found here: https://docs.unity3d.com/ScriptReference/KeyCode.html\n" +
        "- Third person camera fov and nearclipplane value can also be configured through uix/config file\n" +
        "- Rear camera can be move over to the left or right of your avatar using keybinds that are also configurable through uix/config file\n" +
        "- Has a \"freecam\". Keybind needs to be set first to use. You can use arrow keys to look up/down/left/right and I/J/K/L to move the camera", 
        new []{"Third person", "freecam", "freeview", "camera"}, 
        null, 
        "Fix random null ref", 
        "#2ad9f7"
    )
]

[assembly:AssemblyVersion(ModConstants.VERSION)]
[assembly:AssemblyFileVersion(ModConstants.VERSION)]
[assembly:AssemblyTitle(ModConstants.NAME)]
[assembly:AssemblyDescription(ModConstants.NAME)]
[assembly:AssemblyCopyright("Created by " + ModConstants.AUTHOR)]

namespace StandaloneThirdPerson;
public static class ModConstants
{
    public const string VERSION = "1.3.4";
    public const string NAME = "StandaloneThirdPerson";
    public const string AUTHOR = "gompo, ljoonal";
    public const string DOWNLOAD_LINK = "https://github.com/gompoc/VRChatMods/releases/";
}
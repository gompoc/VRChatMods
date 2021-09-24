using System.IO;
using System.Reflection;
using ActionMenuApi;
using ActionMenuApi.Api;
using ActionMenuApi.Pedals;
using MelonLoader;
using UnhollowerRuntimeLib;
using UnityEngine;

[assembly: MelonInfo(typeof(ActionMenuTestMod.ActionMenuTestMod), "ActionMenuTestMod", "1.0.0", "gompo")]
[assembly: MelonGame("VRChat", "VRChat")]
[assembly: VerifyLoaderVersion(0, 4, 3, true)]

namespace ActionMenuTestMod
{
    // Icons from https://uxwing.com/
    public partial class ActionMenuTestMod : MelonMod
    {

        private float testFloatValue = 50;
        private float testFloatValue2 = 50;
        private bool testBool = false;
        private bool testBool2 = false;
        private bool riskyFunctionsAllowed = false;
        private Vector2 testVector = new ();
        private Vector2 testVector2 = new ();
        private static float x = 0;
        private static float y = 0;
        private static float z = 0;
        private static AssetBundle iconsAssetBundle = null;
        private static Texture2D toggleIcon;
        private static Texture2D radialIcon;
        private static Texture2D subMenuIcon;
        private static Texture2D buttonIcon;
        
        public override void OnApplicationStart()
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ActionMenuTestMod.customicons"))
            using (var tempStream = new MemoryStream((int) stream.Length))
            {
                stream.CopyTo(tempStream);
                iconsAssetBundle = AssetBundle.LoadFromMemory_Internal(tempStream.ToArray(), 0);
                iconsAssetBundle.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            }

            radialIcon = iconsAssetBundle.LoadAsset_Internal("Assets/Resources/Icons/sound-full.png", Il2CppType.Of<Texture2D>()).Cast<Texture2D>();
            radialIcon.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            toggleIcon = iconsAssetBundle.LoadAsset_Internal("Assets/Resources/Icons/zero.png", Il2CppType.Of<Texture2D>()).Cast<Texture2D>();
            toggleIcon.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            subMenuIcon = iconsAssetBundle.LoadAsset_Internal("Assets/Resources/Icons/file-transfer.png", Il2CppType.Of<Texture2D>()).Cast<Texture2D>();
            subMenuIcon.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            buttonIcon = iconsAssetBundle.LoadAsset_Internal("Assets/Resources/Icons/cloud-data-download.png", Il2CppType.Of<Texture2D>()).Cast<Texture2D>();
            buttonIcon.hideFlags |= HideFlags.DontUnloadUnusedAsset;

            VRCActionMenuPage.AddButton(ActionMenuPage.Main, "Button",() => MelonLogger.Msg("Pressed Button"), buttonIcon);

            PedalSubMenu subMenu = VRCActionMenuPage.AddSubMenu(ActionMenuPage.Config, "Toggle", () => { }, null, true);
            subMenu.locked = true;
            
            AMUtils.AddToModsFolder(
                "Test Stuff",
                delegate
                {
                    CustomSubMenu.AddToggle("Risky Functions", !riskyFunctionsAllowed, (b) =>
                    {
                        riskyFunctionsAllowed = !b;
                        AMUtils.RefreshActionMenu(); //Refresh menu to update the locked state of the pedal
                    });
                    //No properties here are saved because I'm lazy af
                    CustomSubMenu.AddToggle("Enable Hax", false, b => { }, buttonIcon,riskyFunctionsAllowed);
                    CustomSubMenu.AddRadialPuppet("Volume", f => { }, 0, buttonIcon, riskyFunctionsAllowed);
                    CustomSubMenu.AddRestrictedRadialPuppet("Volume Restricted", f => { }, 0, buttonIcon, riskyFunctionsAllowed);
                    CustomSubMenu.AddSubMenu("Whatever", () => { }, buttonIcon, riskyFunctionsAllowed);
                    CustomSubMenu.AddButton("Risky Function", () =>
                    {
                        MelonLogger.Msg("Locked Pedal Func ran");
                    }, buttonIcon, riskyFunctionsAllowed);
                    CustomSubMenu.AddFourAxisPuppet("Move", vector2 => { }, toggleIcon, riskyFunctionsAllowed);
                },
                subMenuIcon
            );
            
            AMUtils.AddToModsFolder(
                "New Cube Stuff",
                delegate
                {
                    CustomSubMenu.AddFourAxisPuppet("Reposition cube X/Y", (v) => RePositionCubeXY(v), buttonIcon);
                    CustomSubMenu.AddFourAxisPuppet("Reposition cube Z/Y", RePositionCubeZY, toggleIcon);
                    CustomSubMenu.AddFourAxisPuppet("Reposition cube X/Z", RePositionCubeXZ, toggleIcon);
                    CustomSubMenu.AddRadialPuppet("X",RotateCubeX, x,radialIcon); //Rotation a bit borked
                    CustomSubMenu.AddToggle("Test Toggle", testBool2, (b) => testBool2 = b);
                    CustomSubMenu.AddRadialPuppet("Y",RotateCubeY, y,radialIcon);
                    CustomSubMenu.AddRadialPuppet("Z",RotateCubeZ, z,radialIcon);
                    CustomSubMenu.AddButton("Spawn Cube", CreateCube, buttonIcon);
                    CustomSubMenu.AddButton("Tp Cube To Player",() => _controllingGameObject.transform.localPosition = VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.localPosition, buttonIcon);
                },
                subMenuIcon,
                false
            );

            //Purely for backwards compatibility testing don't use yourself
            AMAPI.AddModFolder(
                "Old Cube Stuff",
                delegate
                {
                    AMAPI.AddFourAxisPedalToSubMenu("Reposition cube X/Y", (v) => RePositionCubeXY(v), toggleIcon);
                    AMAPI.AddFourAxisPedalToSubMenu("Reposition cube Z/Y", RePositionCubeZY, toggleIcon);
                    AMAPI.AddFourAxisPedalToSubMenu("Reposition cube X/Z", RePositionCubeXZ, toggleIcon);
                    AMAPI.AddRadialPedalToSubMenu("X",RotateCubeX, x,radialIcon); //Rotation a bit borked
                    AMAPI.AddTogglePedalToSubMenu("Test Toggle", testBool2, (b) => testBool2 = b);
                    AMAPI.AddRadialPedalToSubMenu("Y",RotateCubeY, y,radialIcon);
                    AMAPI.AddRadialPedalToSubMenu("Z",RotateCubeZ, z,radialIcon);
                    AMAPI.AddButtonPedalToSubMenu("Spawn Cube", CreateCube, buttonIcon);
                    AMAPI.AddButtonPedalToSubMenu("Tp Cube To Player",() => _controllingGameObject.transform.localPosition = VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.localPosition, buttonIcon);
                },
                subMenuIcon
            );
            for (int i = 0; i < 2; i++) //Set to a high number if you want to test the page functionality 
            {
                AMAPI.AddModFolder($"Example Mod {i+2}", () => {}, subMenuIcon); 
            }
        }

        public override void OnUpdate()
        {
            if (Input.GetKeyUp(KeyCode.P))
            {
                AMUtils.RefreshActionMenu();
            }

            if (Input.GetKeyUp(KeyCode.I))
            {
                AMUtils.ResetMenu();
            }
        }

        private static void CreateCube()
        {
            _controllingGameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            _controllingGameObject.GetComponent<Collider>().enabled = false;
            var eulerAngles = _controllingGameObject.transform.eulerAngles;
            x = eulerAngles.x*360;
            y = eulerAngles.y*360;
            z = eulerAngles.z*360;
        }


        private static void RePositionCubeXY(Vector3 v)
        {
            _controllingGameObject.transform.localPosition += v/25;
        }
        private static void RePositionCubeZY(Vector2 v)
        {
            _controllingGameObject.transform.localPosition += new Vector3(0, v.y/25, v.x/25);
        }
        private static void RePositionCubeXZ(Vector2 v)
        {
            _controllingGameObject.transform.localPosition += new Vector3(v.x/25, 0, v.y/25);
        }
        
        private static void RotateCubeX(float rotation)
        {
            //This is the incorrect way of rotating the gameobject and it breaks from 90-270 as quaternions and euler angle conversions are a bit fucky
            Vector3 old = _controllingGameObject.transform.eulerAngles;
            _controllingGameObject.transform.eulerAngles = new Vector3((rotation)*360, old.y, old.z);
            x = rotation;
        }
        
        private static void RotateCubeY(float rotation)
        {
            Vector3 old = _controllingGameObject.transform.eulerAngles;
            //MelonLogger.Msg($"Old Angles: {old.ToString()}");
            _controllingGameObject.transform.eulerAngles = new Vector3(old.x, (rotation)*360, old.z);
            y = rotation;
        }
        private static void RotateCubeZ(float rotation)
        {
            Vector3 old = _controllingGameObject.transform.eulerAngles;
            //MelonLogger.Msg($"Old Angles: {old.ToString()}");
            _controllingGameObject.transform.eulerAngles = new Vector3(old.x, old.y, (rotation)*360);
            z = rotation;
        }
        
        private static GameObject _controllingGameObject;
    }
}
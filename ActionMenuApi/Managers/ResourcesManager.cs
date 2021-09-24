using System.IO;
using System.Reflection;
using ActionMenuApi.Helpers;
using MelonLoader;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.UI;

namespace ActionMenuApi.Managers
{
    internal static class ResourcesManager
    {
        private static GameObject lockPrefab;
        private static Texture2D pageOne;
        private static Texture2D pageTwo;
        private static Texture2D pageThree;
        private static Texture2D pageFour;
        private static Texture2D pageFive;
        private static Texture2D pageSix;
        private static Texture2D pageSeven;
        private static Texture2D locked;
        private static Texture2D modsSectionIcon;

        public static void LoadTextures()
        {
            AssetBundle iconsAssetBundle;
            using (var stream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("ActionMenuApi.actionmenuapi.icons"))
            using (var tempStream = new MemoryStream((int) stream.Length))
            {
                stream.CopyTo(tempStream);

                iconsAssetBundle = AssetBundle.LoadFromMemory_Internal(tempStream.ToArray(), 0);
                iconsAssetBundle.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            }

            modsSectionIcon = iconsAssetBundle
                .LoadAsset_Internal("Assets/ActionMenuApi/vrcmg.png", Il2CppType.Of<Texture2D>()).Cast<Texture2D>();
            modsSectionIcon.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            pageOne = iconsAssetBundle.LoadAsset_Internal("Assets/ActionMenuApi/1.png", Il2CppType.Of<Texture2D>())
                .Cast<Texture2D>();
            pageOne.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            pageTwo = iconsAssetBundle.LoadAsset_Internal("Assets/ActionMenuApi/2.png", Il2CppType.Of<Texture2D>())
                .Cast<Texture2D>();
            pageTwo.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            pageThree = iconsAssetBundle.LoadAsset_Internal("Assets/ActionMenuApi/3.png", Il2CppType.Of<Texture2D>())
                .Cast<Texture2D>();
            pageThree.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            pageFour = iconsAssetBundle.LoadAsset_Internal("Assets/ActionMenuApi/4.png", Il2CppType.Of<Texture2D>())
                .Cast<Texture2D>();
            pageFour.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            pageFive = iconsAssetBundle.LoadAsset_Internal("Assets/ActionMenuApi/5.png", Il2CppType.Of<Texture2D>())
                .Cast<Texture2D>();
            pageFive.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            pageSix = iconsAssetBundle.LoadAsset_Internal("Assets/ActionMenuApi/6.png", Il2CppType.Of<Texture2D>())
                .Cast<Texture2D>();
            pageSix.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            pageSeven = iconsAssetBundle.LoadAsset_Internal("Assets/ActionMenuApi/7.png", Il2CppType.Of<Texture2D>())
                .Cast<Texture2D>();
            pageSeven.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            locked = iconsAssetBundle.LoadAsset_Internal("Assets/ActionMenuApi/locked.png", Il2CppType.Of<Texture2D>())
                .Cast<Texture2D>();
            locked.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            MelonLogger.Msg("Loaded textures");
        }

        public static void InitLockGameObject()
        {
            lockPrefab = Object.Instantiate(ActionMenuDriver.prop_ActionMenuDriver_0.GetRightOpener().GetActionMenu()
                .GetPedalOptionPrefab().GetComponent<PedalOption>().GetActionButton().gameObject.GetChild("Inner")
                .GetChild("Folder Icon"));
            Object.DontDestroyOnLoad(lockPrefab);
            lockPrefab.active = false;
            lockPrefab.gameObject.name = Constants.LOCKED_PEDAL_OVERLAY_GAMEOBJECT_NAME;
            lockPrefab.GetComponent<RawImage>().texture = locked;
            MelonLogger.Msg("Created lock gameobject");
        }

        public static Texture2D GetPageIcon(int pageIndex)
        {
            switch (pageIndex)
            {
                case 1:
                    return pageOne;
                case 2:
                    return pageTwo;
                case 3:
                    return pageThree;
                case 4:
                    return pageFour;
                case 5:
                    return pageFive;
                case 6:
                    return pageSix;
                case 7:
                    return pageSeven;
                default:
                    return null;
            }
        }

        public static void AddLockChildIcon(GameObject parent)
        {
            var lockedGameObject = Object.Instantiate(lockPrefab, parent.transform, false);
            lockedGameObject.SetActive(true);
            lockedGameObject.transform.localPosition = new Vector3(50, -25, 0);
            lockedGameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }

        public static Texture2D GetModsSectionIcon()
        {
            return modsSectionIcon;
        }
    }
}
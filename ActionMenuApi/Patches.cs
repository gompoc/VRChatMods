using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using ActionMenuApi.Helpers;
using ActionMenuApi.Managers;
using ActionMenuApi.Pedals;
using HarmonyLib;
using MelonLoader;
using UnhollowerBaseLib;

namespace ActionMenuApi
{
    [SuppressMessage("ReSharper", "Unity.IncorrectMonoBehaviourInstantiation")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal static class Patches
    {
        public static List<PedalStruct>
            configPagePre = new(),
            configPagePost = new(),
            emojisPagePre = new(),
            emojisPagePost = new(),
            expressionPagePre = new(),
            expressionPagePost = new(),
            sdk2ExpressionPagePre = new(),
            sdk2ExpressionPagePost = new(),
            mainPagePre = new(),
            mainPagePost = new(),
            menuOpacityPagePre = new(),
            menuOpacityPagePost = new(),
            menuSizePagePre = new(),
            menuSizePagePost = new(),
            nameplatesPagePre = new(),
            nameplatesPagePost = new(),
            nameplatesOpacityPagePre = new(),
            nameplatesOpacityPagePost = new(),
            nameplatesVisibilityPagePre = new(),
            nameplatesVisibilityPagePost = new(),
            nameplatesSizePagePre = new(),
            nameplatesSizePagePost = new(),
            optionsPagePre = new(),
            optionsPagePost = new();

        private static readonly List<string> openConfigPageKeyWords = new(new[] {"Menu Size", "Menu Opacity"});
        private static readonly List<string> openMainPageKeyWords = new(new[] {"Options", "Emojis"});
        private static readonly List<string> openMenuOpacityPageKeyWords = new(new[] {"{0}%"});
        private static readonly List<string> openEmojisPageKeyWords = new(new[] {" ", "_"});
        private static readonly List<string> openExpressionMenuKeyWords = new(new[] {"Reset Avatar"});
        private static readonly List<string> openOptionsPageKeyWords = new(new[] {"Config"});
        private static readonly List<string> openSDK2ExpressionPageKeyWords = new(new[] {"EMOTE{0}"});

        private static readonly List<string> openNameplatesOpacityPageKeyWords =
            new(new[] {"100%", "80%", "60%", "40%", "20%", "0%"});

        private static readonly List<string> openNameplatesPageKeyWords = new(new[] {"Visibility", "Size", "Opacity"});

        private static readonly List<string> openNameplatesVisibilityPageKeyWords =
            new(new[] {"Nameplates Shown", "Icons Only", "Nameplates Hidden"});

        private static readonly List<string> openNameplatesSizePageKeyWords =
            new(new[] {"Large", "Medium", "Normal", "Small", "Tiny"});

        private static readonly List<string> openMenuSizePageKeyWords =
            new(new[]
            {
                "XXXXXXXXX"
            }); // No strings found :( Unusable for now. Scanning for methods doesnt help either as there are other functions that yield similar results

        private static HarmonyLib.Harmony Harmony;

        public static void PatchAll(HarmonyLib.Harmony harmonyInstance)
        {
            Harmony = harmonyInstance;
            PatchMethod(openMenuOpacityPageKeyWords, "OpenMenuOpacityPagePre", "OpenMenuOpacityPagePost");
            PatchMethod(openExpressionMenuKeyWords, "OpenExpressionMenuPre", "OpenExpressionMenuPost");
            PatchMethod(openConfigPageKeyWords, "OpenConfigPagePre", "OpenConfigPagePost");
            PatchMethod(openMainPageKeyWords, "OpenMainPagePre", "OpenMainPagePost");
            PatchMethod(openEmojisPageKeyWords, "OpenEmojisPagePre", "OpenEmojisPagePost");
            PatchMethod(openNameplatesOpacityPageKeyWords, "OpenNameplatesOpacityPre", "OpenNameplatesOpacityPost");
            PatchMethod(openNameplatesPageKeyWords, "OpenNameplatesPagePre", "OpenNameplatesPagePost");
            PatchMethod(openNameplatesVisibilityPageKeyWords, "OpenNameplatesVisibilityPre", "OpenNameplatesVisibilityPost");
            PatchMethod(openSDK2ExpressionPageKeyWords, "OpenSDK2ExpressionPre", "OpenSDK2ExpressionPost");
            PatchMethod(openOptionsPageKeyWords, "OpenOptionsPre", "OpenOptionsPost");

            //Method_Private_Void_PDM_11
            //Special Child
            /*harmonyInstance.Patch(
                typeof(ActionMenu).GetMethods().Single(
                    m => Utilities.checkXref(m, openNameplatesSizePageKeyWords)
                         && m.CheckStringsCount(5)
                ),
                new HarmonyMethod(typeof(Patches).GetMethod("OpenNameplatesSizePre")),
                new HarmonyMethod(typeof(Patches).GetMethod("OpenNameplatesSizePost"))
            );*/
            MelonLogger.Msg("Patches Applied");
        }

        public static void OpenConfigPagePre(ActionMenu __instance)
        {
            Utilities.AddPedalsInList(configPagePre, __instance);
        }

        public static void OpenConfigPagePost(ActionMenu __instance)
        {
            Utilities.AddPedalsInList(configPagePost, __instance);
        }

        public static void OpenMainPagePre(ActionMenu __instance)
        {
            Utilities.AddPedalsInList(mainPagePre, __instance);
        }

        public static void OpenMainPagePost(ActionMenu __instance)
        {
            if (ModsFolderManager.mods.Count > 0) ModsFolderManager.AddMainPageButton();
            Utilities.AddPedalsInList(mainPagePost, __instance);
        }

        public static void OpenMenuOpacityPagePre(ActionMenu __instance)
        {
            Utilities.AddPedalsInList(menuOpacityPagePre, __instance);
        }

        public static void OpenMenuOpacityPagePost(ActionMenu __instance)
        {
            Utilities.AddPedalsInList(menuOpacityPagePost, __instance);
        }

        public static void OpenEmojisPagePre(ActionMenu __instance)
        {
            Utilities.AddPedalsInList(emojisPagePre, __instance);
        }

        public static void OpenEmojisPagePost(ActionMenu __instance)
        {
            Utilities.AddPedalsInList(emojisPagePost, __instance);
        }

        public static void OpenExpressionMenuPre(ActionMenu __instance)
        {
            Utilities.AddPedalsInList(expressionPagePre, __instance);
        }

        public static void OpenExpressionMenuPost(ActionMenu __instance)
        {
            Utilities.AddPedalsInList(expressionPagePost, __instance);
        }

        public static void OpenNameplatesOpacityPre(ActionMenu __instance)
        {
            Utilities.AddPedalsInList(nameplatesOpacityPagePre, __instance);
        }

        public static void OpenNameplatesOpacityPost(ActionMenu __instance)
        {
            Utilities.AddPedalsInList(nameplatesOpacityPagePost, __instance);
        }

        public static void OpenNameplatesPagePre(ActionMenu __instance)
        {
            Utilities.AddPedalsInList(nameplatesPagePre, __instance);
        }

        public static void OpenNameplatesPagePost(ActionMenu __instance)
        {
            Utilities.AddPedalsInList(nameplatesPagePost, __instance);
        }

        public static void OpenNameplatesVisibilityPre(ActionMenu __instance)
        {
            Utilities.AddPedalsInList(nameplatesVisibilityPagePre, __instance);
        }

        public static void OpenNameplatesVisibilityPost(ActionMenu __instance)
        {
            Utilities.AddPedalsInList(nameplatesVisibilityPagePost, __instance);
        }

        public static void OpenNameplatesSizePre(ActionMenu __instance)
        {
            Utilities.AddPedalsInList(nameplatesSizePagePre, __instance);
        }

        public static void OpenNameplatesSizePost(ActionMenu __instance)
        {
            Utilities.AddPedalsInList(nameplatesSizePagePost, __instance);
        }

        public static void OpenOptionsPre(ActionMenu __instance)
        {
            Utilities.AddPedalsInList(optionsPagePre, __instance);
        }

        public static void OpenOptionsPost(ActionMenu __instance)
        {
            Utilities.AddPedalsInList(optionsPagePost, __instance);
        }

        public static void OpenSDK2ExpressionPre(ActionMenu __instance)
        {
            Utilities.AddPedalsInList(sdk2ExpressionPagePre, __instance);
        }

        public static void OpenSDK2ExpressionPost(ActionMenu __instance)
        {
            Utilities.AddPedalsInList(sdk2ExpressionPagePost, __instance);
        }

        public static void OpenMenuSizePre(ActionMenu __instance)
        {
            Utilities.AddPedalsInList(menuSizePagePre, __instance);
        }

        public static void OpenMenuSizePost(ActionMenu __instance)
        {
            Utilities.AddPedalsInList(menuSizePagePost, __instance);
        }

        private static MethodInfo FindAMMethod(List<string> keywords)
        {
            return typeof(ActionMenu).GetMethods()
                .First(m => m.Name.StartsWith("Method") && Utilities.CheckXref(m, keywords));
        }

        private static void PatchMethod(List<string> keywords, string preName, string postName)
        {
            try
            {
                Harmony.Patch(
                    FindAMMethod(keywords),
                    new HarmonyMethod(typeof(Patches).GetMethod(preName)),
                    new HarmonyMethod(typeof(Patches).GetMethod(postName))
                );
            }
            catch (Exception e)
            {
                MelonLogger.Warning($"Failed to Patch Method: {e}");
            }
        }
    }
}
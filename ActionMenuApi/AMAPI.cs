using System;
using ActionMenuApi.Api;
using ActionMenuApi.Pedals;
using ActionMenuApi.Types;
using UnityEngine;
// ReSharper disable HeuristicUnreachableCode
#pragma warning disable 1591

namespace ActionMenuApi
{
    [Obsolete(
        "This class is only here for compatibility reasons! Please use ActionMenuApi.Api.CustomSubMenu/VRCActionMenuPage/AMUtils for new updated methods to integrate with the action menu",
        false)]
    public static class AMAPI
    {
        [Obsolete("This method is only here for compatibility reasons! Please use VRCActionMenuPage.AddButton()",
            false)]
        public static void AddButtonPedalToMenu(ActionMenuPageType pageType, string text, Action triggerEvent,
            Texture2D icon = null, Insertion insertion = Insertion.Post)
        {
            VRCActionMenuPage.AddPedalToList((ActionMenuPage) pageType, new PedalButton(text, icon, triggerEvent),
                insertion);
        }

        [Obsolete("This method is only here for compatibility reasons! Please use CustomSubMenu.AddButton()", false)]
        public static PedalOption AddButtonPedalToSubMenu(string text, Action triggerEvent, Texture2D icon = null)
        {
            return CustomSubMenu.AddButton(text, triggerEvent, icon);
        }

        [Obsolete("This method is only here for compatibility reasons! Please use VRCActionMenuPage.AddRadialPuppet()",
            false)]
        public static void AddRadialPedalToMenu(ActionMenuPageType pageType, string text, Action<float> onUpdate,
            float startingValue = 0, Texture2D icon = null, Insertion insertion = Insertion.Post)
        {
            VRCActionMenuPage.AddPedalToList((ActionMenuPage) pageType,
                new PedalRadial(text, startingValue, icon, onUpdate), insertion);
        }

        [Obsolete("This method is only here for compatibility reasons! Please use CustomSubMenu.AddRadialPuppet()",
            false)]
        public static PedalOption AddRadialPedalToSubMenu(string text, Action<float> onUpdate, float startingValue = 0,
            Texture2D icon = null)
        {
            return CustomSubMenu.AddRadialPuppet(text, onUpdate, startingValue, icon);
        }

        [Obsolete(
            "This method is only here for compatibility reasons! Please use VRCActionMenuPage.AddFourAxisPuppet()",
            false)]
        public static void AddFourAxisPedalToMenu(ActionMenuPageType pageType, string text, Action<Vector2> onUpdate,
            Texture2D icon = null, Insertion insertion = Insertion.Post, string topButtonText = "Up",
            string rightButtonText = "Right", string downButtonText = "Down", string leftButtonText = "Left")
        {
            VRCActionMenuPage.AddPedalToList((ActionMenuPage) pageType,
                new PedalFourAxis(text, icon, onUpdate, topButtonText, rightButtonText, downButtonText, leftButtonText),
                insertion);
        }

        [Obsolete("This method is only here for compatibility reasons! Please use CustomSubMenu.AddFourAxisPuppet()",
            false)]
        public static PedalOption AddFourAxisPedalToSubMenu(string text, Action<Vector2> onUpdate,
            Texture2D icon = null, string topButtonText = "Up",
            string rightButtonText = "Right", string downButtonText = "Down", string leftButtonText = "Left")
        {
            return CustomSubMenu.AddFourAxisPuppet(text, onUpdate, icon, false, topButtonText, rightButtonText,
                downButtonText, leftButtonText);
        }

        [Obsolete("This method is only here for compatibility reasons! Please use VRCActionMenuPage.AddSubMenu()",
            false)]
        public static void AddSubMenuToMenu(ActionMenuPageType pageType, string text, Action openFunc,
            Texture2D icon = null, Action closeFunc = null, Insertion insertion = Insertion.Post)
        {
            VRCActionMenuPage.AddPedalToList((ActionMenuPage) pageType,
                new PedalSubMenu(openFunc, text, icon, closeFunc), insertion);
        }

        [Obsolete("This method is only here for compatibility reasons! Please use CustomSubMenu.AddSubMenu()", false)]
        public static PedalOption AddSubMenuToSubMenu(string text, Action openFunc, Texture2D icon = null,
            Action closeFunc = null)
        {
            return CustomSubMenu.AddSubMenu(text, openFunc, icon, false, closeFunc);
        }

        [Obsolete("This method is only here for compatibility reasons! Please use VRCActionMenuPage.AddToggle()",
            false)]
        public static void AddTogglePedalToMenu(ActionMenuPageType pageType, string text, bool startingState,
            Action<bool> onToggle, Texture2D icon = null, Insertion insertion = Insertion.Post)
        {
            VRCActionMenuPage.AddPedalToList((ActionMenuPage) pageType,
                new PedalToggle(text, onToggle, startingState, icon), insertion);
        }

        [Obsolete("This method is only here for compatibility reasons! Please use CustomSubMenu.AddToggle()", false)]
        public static PedalOption AddTogglePedalToSubMenu(string text, bool startingState, Action<bool> onToggle,
            Texture2D icon = null)
        {
            return CustomSubMenu.AddToggle(text, startingState, onToggle, icon);
        }

        [Obsolete("This method is only here for compatibility reasons! Please use AMUtils.AddToModsFolder()", false)]
        public static void AddModFolder(string text, Action openFunc, Texture2D icon = null)
        {
            AMUtils.AddToModsFolder(text, openFunc, icon);
        }
    }
}
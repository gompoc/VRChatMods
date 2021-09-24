using System;
using ActionMenuApi.Pedals;
using ActionMenuApi.Types;
using MelonLoader;
using UnityEngine;

namespace ActionMenuApi.Api
{
    /// <summary>
    /// VRC Page specific things
    /// </summary>
    public static class VRCActionMenuPage
    {
        /// <summary>
        ///     Add a button pedal to a specific ActionMenu page
        /// </summary>
        /// <param name="pageType">The page to add the button to</param>
        /// <param name="text">Button text</param>
        /// <param name="triggerEvent">Button click action</param>
        /// <param name="icon">(optional) The Button Icon</param>
        /// <param name="locked">(optional)The starting state for the lockable pedal, true = locked, false = unlocked</param>
        /// <param name="insertion">
        ///     (optional) Determines whether or not the button is added before or after VRChat's buttons for
        ///     the target page
        /// </param>
        public static PedalButton AddButton(ActionMenuPage pageType, string text, Action triggerEvent,
            Texture2D icon = null, bool locked = false, Insertion insertion = Insertion.Post)
        {
            var pedal = new PedalButton(text, icon, triggerEvent, locked);
            AddPedalToList(pageType, pedal, insertion);
            return pedal;
        }

        /// <summary>
        ///     Add a radial puppet button pedal to a specific ActionMenu page
        /// </summary>
        /// <param name="pageType">The page to add the button to</param>
        /// <param name="text">Button text</param>
        /// <param name="onUpdate">Calls action with a float between 0 - 1 depending on the current value of the radial puppet</param>
        /// <param name="startingValue">(optional) Starting value for radial puppet 0-1</param>
        /// <param name="icon">(optional) The Button Icon</param>
        /// <param name="locked">(optional)The starting state for the lockable pedal, true = locked, false = unlocked</param>
        /// <param name="insertion">
        ///     (optional) Determines whether or not the button is added before or after VRChat's buttons for
        ///     the target page
        /// </param>
        public static PedalRadial AddRadialPuppet(ActionMenuPage pageType, string text, Action<float> onUpdate,
            float startingValue = 0, Texture2D icon = null, bool locked = false, Insertion insertion = Insertion.Post)
        {
            var pedal = new PedalRadial(text, startingValue, icon, onUpdate, locked);
            AddPedalToList(pageType, pedal, insertion);
            return pedal;
        }

        /// <summary>
        ///     Add a restricted radial puppet button pedal to a specific ActionMenu page. Restricted meaning that you can't rotate
        ///     past 100 to get to 0 and vice versa
        /// </summary>
        /// <param name="pageType">The page to add the button to</param>
        /// <param name="text">Button text</param>
        /// <param name="onUpdate">Calls action with a float between 0 - 1 depending on the current value of the radial puppet</param>
        /// <param name="startingValue">(optional) Starting value for radial puppet 0-1</param>
        /// <param name="icon">(optional) The Button Icon</param>
        /// <param name="locked">(optional)The starting state for the lockable pedal, true = locked, false = unlocked</param>
        /// <param name="insertion">
        ///     (optional) Determines whether or not the button is added before or after VRChat's buttons for
        ///     the target page
        /// </param>
        public static PedalRadial AddRestrictedRadialPuppet(ActionMenuPage pageType, string text,
            Action<float> onUpdate, float startingValue = 0, Texture2D icon = null, bool locked = false,
            Insertion insertion = Insertion.Post)
        {
            var pedal = new PedalRadial(text, startingValue, icon, onUpdate, locked, true);
            AddPedalToList(pageType, pedal, insertion);
            return pedal;
        }

        /// <summary>
        ///     Add a four axis puppet button pedal to a specific ActionMenu page
        /// </summary>
        /// <param name="pageType">The page to add the button to</param>
        /// <param name="text">Button text</param>
        /// <param name="onUpdate">
        ///     Calls action with a Vector2 with x and y being between -1 and 1 depending on the current value
        ///     of the four axis puppet
        /// </param>
        /// <param name="icon">(optional) The Button Icon</param>
        /// <param name="insertion">
        ///     (optional) Determines whether or not the button is added before or after VRChat's buttons for
        ///     the target page
        /// </param>
        /// <param name="locked">(optional)The starting state for the lockable pedal, true = locked, false = unlocked</param>
        /// <param name="topButtonText">(optional) Top Button Button text On Four Axis Puppet</param>
        /// <param name="rightButtonText">(optional) Right Button Button text On Four Axis Puppet</param>
        /// <param name="downButtonText">(optional) Bottom Button Button text On Four Axis Puppet</param>
        /// <param name="leftButtonText">(optional) Left Button Button text On Four Axis Puppet</param>
        public static PedalFourAxis AddFourAxisPuppet(ActionMenuPage pageType, string text, Action<Vector2> onUpdate,
            Texture2D icon = null, bool locked = false, string topButtonText = "Up",
            string rightButtonText = "Right", string downButtonText = "Down", string leftButtonText = "Left",
            Insertion insertion = Insertion.Post)
        {
            var pedal = new PedalFourAxis(text, icon, onUpdate, topButtonText, rightButtonText, downButtonText,
                leftButtonText, locked);
            AddPedalToList(pageType, pedal, insertion);
            return pedal;
        }

        /// <summary>
        ///     Add a submenu to an ActionMenu page
        /// </summary>
        /// <param name="pageType">The page to add the button to</param>
        /// <param name="text">Button text</param>
        /// <param name="openFunc">
        ///     Function called when page opened. Add your methods calls to other AMAPI methods such
        ///     AddRadialPedalToSubMenu to add buttons to the submenu it creates when clicked
        /// </param>
        /// <param name="icon">(optional) The Button Icon</param>
        /// <param name="locked">(optional)The starting state for the lockable pedal, true = locked, false = unlocked</param>
        /// <param name="closeFunc">(optional) Function called when page closes</param>
        /// <param name="insertion">
        ///     (optional) Determines whether or not the button is added before or after VRChat's buttons for
        ///     the target page
        /// </param>
        public static PedalSubMenu AddSubMenu(ActionMenuPage pageType, string text, Action openFunc,
            Texture2D icon = null, bool locked = false, Action closeFunc = null, Insertion insertion = Insertion.Post)
        {
            var pedal = new PedalSubMenu(openFunc, text, icon, closeFunc, locked);
            AddPedalToList(pageType, pedal, insertion);
            return pedal;
        }

        /// <summary>
        ///     Add a toggle button pedal to an ActionMenu page
        /// </summary>
        /// <param name="pageType">The page to add the button to</param>
        /// <param name="text">Button text</param>
        /// <param name="startingState">Starting value of the toggle pedal everytime the pedal is created</param>
        /// <param name="onToggle">Calls action with a bool depending on the current value of the toggle</param>
        /// <param name="icon">(optional) The Button Icon</param>
        /// <param name="locked">(optional)The starting state for the lockable pedal, true = locked, false = unlocked</param>
        /// <param name="insertion">
        ///     (optional) Determines whether or not the button is added before or after VRChat's buttons for
        ///     the target page
        /// </param>
        public static PedalToggle AddToggle(ActionMenuPage pageType, string text, bool startingState,
            Action<bool> onToggle, Texture2D icon = null, bool locked = false, Insertion insertion = Insertion.Post)
        {
            var pedal = new PedalToggle(text, onToggle, startingState, icon, locked);
            AddPedalToList(pageType, pedal, insertion);
            return pedal;
        }

        internal static void AddPedalToList(ActionMenuPage pageType, PedalStruct customPedal, Insertion insertion)
        {
            Logger.Log(
                $"Adding to page: {pageType.ToString()}, Text: {customPedal.text}, Locked: {customPedal.locked}");
            switch (pageType)
            {
                case ActionMenuPage.SDK2Expression:
                    if (insertion == Insertion.Pre) Patches.sdk2ExpressionPagePre.Add(customPedal);
                    else if (insertion == Insertion.Post) Patches.sdk2ExpressionPagePost.Add(customPedal);
                    return;
                case ActionMenuPage.Config:
                    if (insertion == Insertion.Pre) Patches.configPagePre.Add(customPedal);
                    else if (insertion == Insertion.Post) Patches.configPagePost.Add(customPedal);
                    return;
                case ActionMenuPage.Emojis:
                    if (insertion == Insertion.Pre) Patches.emojisPagePre.Add(customPedal);
                    else if (insertion == Insertion.Post) Patches.emojisPagePost.Add(customPedal);
                    return;
                case ActionMenuPage.Expression:
                    if (insertion == Insertion.Pre) Patches.expressionPagePre.Add(customPedal);
                    else if (insertion == Insertion.Post) Patches.expressionPagePost.Add(customPedal);
                    return;
                case ActionMenuPage.Main:
                    if (insertion == Insertion.Pre) Patches.mainPagePre.Add(customPedal);
                    else if (insertion == Insertion.Post) Patches.mainPagePost.Add(customPedal);
                    return;
                case ActionMenuPage.MenuOpacity:
                    if (insertion == Insertion.Pre) Patches.menuOpacityPagePre.Add(customPedal);
                    else if (insertion == Insertion.Post) Patches.menuOpacityPagePost.Add(customPedal);
                    return;
                case ActionMenuPage.MenuSize:
                    MelonLogger.Warning(
                        "Adding to the MenuSize page hasn't been implemented yet. Please use a different page");
                    return;
                    if (insertion == Insertion.Pre) Patches.menuSizePagePre.Add(customPedal);
                    else if (insertion == Insertion.Post) Patches.menuSizePagePost.Add(customPedal);
                    return;
                case ActionMenuPage.Nameplates:
                    if (insertion == Insertion.Pre) Patches.nameplatesPagePre.Add(customPedal);
                    else if (insertion == Insertion.Post) Patches.nameplatesPagePost.Add(customPedal);
                    return;
                case ActionMenuPage.NameplatesOpacity:
                    if (insertion == Insertion.Pre) Patches.nameplatesOpacityPagePre.Add(customPedal);
                    else if (insertion == Insertion.Post) Patches.nameplatesOpacityPagePost.Add(customPedal);
                    return;
                case ActionMenuPage.NameplatesSize:
                    MelonLogger.Warning(
                        "Adding to the Nameplates size page isn't supported currently. Please use a different page");
                    return;
                    if (insertion == Insertion.Pre) Patches.nameplatesSizePagePre.Add(customPedal);
                    else if (insertion == Insertion.Post) Patches.nameplatesSizePagePost.Add(customPedal);
                    return;
                case ActionMenuPage.NameplatesVisibilty:
                    if (insertion == Insertion.Pre) Patches.nameplatesVisibilityPagePre.Add(customPedal);
                    else if (insertion == Insertion.Post) Patches.nameplatesVisibilityPagePost.Add(customPedal);
                    return;
                case ActionMenuPage.Options:
                    if (insertion == Insertion.Pre) Patches.optionsPagePre.Add(customPedal);
                    else if (insertion == Insertion.Post) Patches.optionsPagePost.Add(customPedal);
                    return;
            }
        }
    }
}
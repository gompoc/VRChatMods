using System;
using ActionMenuApi.Helpers;
using ActionMenuApi.Managers;
using MelonLoader;
using UnityEngine;
using VRC.SDK3.Avatars.ScriptableObjects;

// ReSharper disable HeuristicUnreachableCode

namespace ActionMenuApi.Api
{
    /// <summary>
    ///     Class for adding buttons,toggles,radial puppets inside of a custom submenu
    /// </summary>
    public static class CustomSubMenu
    {
        /// <summary>
        ///     Add a lockable button pedal to a custom submenu
        /// </summary>
        /// <param name="text">Button text</param>
        /// <param name="triggerEvent">Button click action</param>
        /// <param name="icon">(optional) The Button Icon</param>
        /// <param name="locked">(optional)The starting state for the lockable pedal, true = locked, false = unlocked</param>
        /// <returns>
        ///     PedalOption Instance (Note: 1. can be null if both action menus are open 2. The gameobject that it is
        ///     attached to is destroyed when you change page on the action menu)
        /// </returns>
        public static PedalOption AddButton(string text, Action triggerEvent, Texture2D icon = null,
            bool locked = false)
        {
            var actionMenuOpener = Utilities.GetActionMenuOpener();
            if (actionMenuOpener == null) return null;
            var pedalOption = actionMenuOpener.GetActionMenu().AddOption();
            pedalOption.SetText(text);
            pedalOption.SetForegroundIcon(icon);
            if (!locked) pedalOption.SetPedalAction(triggerEvent);
            else pedalOption.Lock();
            return pedalOption;
        }


        /// <summary>
        ///     Add a lockable radial puppet button pedal to a custom submenu
        /// </summary>
        /// <param name="text">Button text</param>
        /// <param name="onUpdate">Calls action with a float between 0 - 1 depending on the current value of the radial puppet</param>
        /// <param name="startingValue">(optional) Starting value for radial puppet 0-1</param>
        /// <param name="icon">(optional) The Button Icon</param>
        /// <param name="locked">(optional)The starting state for the lockable pedal, true = locked, false = unlocked</param>
        /// <returns>
        ///     PedalOption Instance (Note: the gameobject that it is attached to is destroyed when you change page on the
        ///     action menu
        /// </returns>
        public static PedalOption AddRadialPuppet(string text, Action<float> onUpdate, float startingValue = 0,
            Texture2D icon = null, bool locked = false)
        {
            var actionMenuOpener = Utilities.GetActionMenuOpener();
            if (actionMenuOpener == null) return null;
            var pedalOption = actionMenuOpener.GetActionMenu().AddOption();
            pedalOption.SetText(text);
            pedalOption.SetBackgroundIcon(icon);
            pedalOption.SetButtonPercentText($"{Math.Round(startingValue * 100)}%");
            pedalOption.SetPedalTypeIcon(Utilities.GetExpressionsIcons().typeRadial);
            if (!locked)
                pedalOption.SetPedalAction(
                    delegate
                    {
                        var combinedAction = (Action<float>) Delegate.Combine(new Action<float>(delegate(float f)
                        {
                            startingValue = f;
                            pedalOption.SetButtonPercentText($"{Math.Round(startingValue * 100)}%");
                        }), onUpdate);
                        RadialPuppetManager.OpenRadialMenu(startingValue, combinedAction, text, pedalOption);
                    }
                );
            else pedalOption.Lock();
            return pedalOption;
        }

        /// <summary>
        ///     Add a lockable and restricted radial puppet button pedal to a custom submenu. Restricted meaning that you can't
        ///     rotate past 100 to get to 0 and vice versa
        /// </summary>
        /// <param name="text">Button text</param>
        /// <param name="onUpdate">Calls action with a float between 0 - 1 depending on the current value of the radial puppet</param>
        /// <param name="startingValue">(optional) Starting value for radial puppet 0-1</param>
        /// <param name="icon">(optional) The Button Icon</param>
        /// <param name="locked">(optional)The starting state for the lockable pedal, true = locked, false = unlocked</param>
        /// <returns>
        ///     PedalOption Instance (Note: the gameobject that it is attached to is destroyed when you change page on the
        ///     action menu
        /// </returns>
        public static PedalOption AddRestrictedRadialPuppet(string text, Action<float> onUpdate,
            float startingValue = 0, Texture2D icon = null, bool locked = false)
        {
            var actionMenuOpener = Utilities.GetActionMenuOpener();
            if (actionMenuOpener == null) return null;
            var pedalOption = actionMenuOpener.GetActionMenu().AddOption();
            pedalOption.SetText(text);
            pedalOption.SetBackgroundIcon(icon);
            pedalOption.SetButtonPercentText($"{Math.Round(startingValue * 100)}%");
            pedalOption.SetPedalTypeIcon(Utilities.GetExpressionsIcons().typeRadial);
            if (!locked)
                pedalOption.SetPedalAction(
                    delegate
                    {
                        var combinedAction = (Action<float>) Delegate.Combine(new Action<float>(delegate(float f)
                        {
                            startingValue = f;
                            pedalOption.SetButtonPercentText($"{Math.Round(startingValue * 100)}%");
                        }), onUpdate);
                        RadialPuppetManager.OpenRadialMenu(startingValue, combinedAction, text, pedalOption, true);
                    }
                );
            else pedalOption.Lock();
            return pedalOption;
        }

        /// <summary>
        ///     Add a lockable four axis puppet button pedal to a custom submenu
        /// </summary>
        /// <param name="text">Button text</param>
        /// <param name="onUpdate">
        ///     Calls action with a Vector2 with x and y being between -1 and 1 depending on the current value
        ///     of the four axis puppet
        /// </param>
        /// <param name="icon">(optional) The Button Icon</param>
        /// <param name="locked">(optional)The starting state for the lockable pedal, true = locked, false = unlocked</param>
        /// <param name="topButtonText">(optional) Top Button Button text On Four Axis Puppet</param>
        /// <param name="rightButtonText">(optional) Right Button Button text On Four Axis Puppet</param>
        /// <param name="downButtonText">(optional) Bottom Button Button text On Four Axis Puppet</param>
        /// <param name="leftButtonText">(optional) Left Button Button text On Four Axis Puppet</param>
        /// <returns>
        ///     PedalOption Instance (Note: the gameobject that it is attached to is destroyed when you change page on the
        ///     action menu
        /// </returns>
        public static PedalOption AddFourAxisPuppet(string text, Action<Vector2> onUpdate, Texture2D icon = null,
            bool locked = false, string topButtonText = "Up",
            string rightButtonText = "Right", string downButtonText = "Down", string leftButtonText = "Left")
        {
            var actionMenuOpener = Utilities.GetActionMenuOpener();
            if (actionMenuOpener == null) return null;
            var pedalOption = actionMenuOpener.GetActionMenu().AddOption();
            pedalOption.SetText(text);
            pedalOption.SetBackgroundIcon(icon);
            pedalOption.SetPedalTypeIcon(Utilities.GetExpressionsIcons().typeAxis);
            if (!locked)
                pedalOption.SetPedalAction(
                    delegate
                    {
                        FourAxisPuppetManager.OpenFourAxisMenu(text, onUpdate, pedalOption);
                        FourAxisPuppetManager.current.GetButtonUp().SetButtonText(topButtonText);
                        FourAxisPuppetManager.current.GetButtonRight().SetButtonText(rightButtonText);
                        FourAxisPuppetManager.current.GetButtonDown().SetButtonText(downButtonText);
                        FourAxisPuppetManager.current.GetButtonLeft().SetButtonText(leftButtonText);
                    }
                );
            else pedalOption.Lock();
            return pedalOption;
        }

        /// <summary>
        ///     Add a lockable submenu button pedal to a custom submenu
        /// </summary>
        /// <param name="text">Button text</param>
        /// <param name="openFunc">
        ///     Function called when page opened. Add your methods calls to other AMAPI methods such
        ///     AddRadialPedalToSubMenu to add buttons to the submenu it creates when clicked
        /// </param>
        /// <param name="icon">(optional) The Button Icon</param>
        /// <param name="locked">(optional)The starting state for the lockable pedal, true = locked, false = unlocked</param>
        /// <param name="closeFunc">(optional) Function called when page closes</param>
        /// <returns>
        ///     PedalOption Instance (Note: the gameobject that it is attached to is destroyed when you change page on the
        ///     action menu
        /// </returns>
        public static PedalOption AddSubMenu(string text, Action openFunc, Texture2D icon = null, bool locked = false,
            Action closeFunc = null)
        {
            var actionMenuOpener = Utilities.GetActionMenuOpener();
            if (actionMenuOpener == null) return null;
            var pedalOption = actionMenuOpener.GetActionMenu().AddOption();
            pedalOption.SetText(text);
            pedalOption.SetForegroundIcon(icon);
            //pedalOption.SetPedalTypeIcon(Utilities.GetExpressionsIcons().typeFolder);
            if (!locked)
                pedalOption.SetPedalAction(
                    delegate { actionMenuOpener.GetActionMenu().PushPage(openFunc, closeFunc, icon, text); }
                );
            else pedalOption.Lock();
            return pedalOption;
        }

        /// <summary>
        ///     Add a lockable toggle button pedal to a custom submenu
        /// </summary>
        /// <param name="text">Button text</param>
        /// <param name="startingState">Starting value of the toggle pedal everytime the pedal is created</param>
        /// <param name="onToggle">Calls action with a bool depending on the current value of the toggle</param>
        /// <param name="icon">(optional) The Button Icon</param>
        /// <param name="locked">(optional)The starting state for the lockable pedal, true = locked, false = unlocked</param>
        /// <returns>
        ///     PedalOption Instance (Note: the gameobject that it is attached to is destroyed when you change page on the
        ///     action menu
        /// </returns>
        public static PedalOption AddToggle(string text, bool startingState, Action<bool> onToggle,
            Texture2D icon = null, bool locked = false)
        {
            var actionMenuOpener = Utilities.GetActionMenuOpener();
            if (actionMenuOpener == null) return null;
            var pedalOption = actionMenuOpener.GetActionMenu().AddOption();
            pedalOption.SetText(text);
            pedalOption.SetBackgroundIcon(icon);
            if (startingState) pedalOption.SetPedalTypeIcon(Utilities.GetExpressionsIcons().typeToggleOn);
            else pedalOption.SetPedalTypeIcon(Utilities.GetExpressionsIcons().typeToggleOff);
            if (!locked)
                pedalOption.SetPedalAction(
                    delegate
                    {
                        startingState = !startingState;
                        if (startingState)
                            pedalOption.SetPedalTypeIcon(Utilities.GetExpressionsIcons().typeToggleOn);
                        else
                            pedalOption.SetPedalTypeIcon(Utilities.GetExpressionsIcons().typeToggleOff);
                        onToggle.Invoke(startingState);
                    }
                );
            else pedalOption.Lock();
            return pedalOption;
        }
    }
}
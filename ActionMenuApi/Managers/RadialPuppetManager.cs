using System;
using ActionMenuApi.Helpers;
using ActionMenuApi.Types;
using MelonLoader;
using UnhollowerBaseLib;
using UnityEngine;
using UnityEngine.XR;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace ActionMenuApi.Managers
{
    internal static class RadialPuppetManager
    {
        private static RadialPuppetMenu radialPuppetMenuRight;
        private static RadialPuppetMenu radialPuppetMenuLeft;
        private static RadialPuppetMenu current;
        private static ActionMenuHand hand;
        private static bool open;
        private static bool restricted;
        private static float currentValue;

        public static float radialPuppetValue { get; set; }
        public static Action<float> onUpdate { get; set; }

        public static void Setup()
        {
            radialPuppetMenuLeft = Utilities
                .CloneGameObject("UserInterface/ActionMenu/Container/MenuL/ActionMenu/RadialPuppetMenu",
                    "UserInterface/ActionMenu/Container/MenuL/ActionMenu").GetComponent<RadialPuppetMenu>();
            radialPuppetMenuRight = Utilities
                .CloneGameObject("UserInterface/ActionMenu/Container/MenuR/ActionMenu/RadialPuppetMenu",
                    "UserInterface/ActionMenu/Container/MenuR/ActionMenu").GetComponent<RadialPuppetMenu>();
        }

        public static void OnUpdate()
        {
            //Probably a better more efficient way to do all this
            if (current != null && current.gameObject.gameObject.active)
            {
                if (XRDevice.isPresent)
                {
                    if (hand == ActionMenuHand.Right)
                    {
                        if (Input.GetAxis(Constants.RIGHT_TRIGGER) >= 0.4f)
                        {
                            CloseRadialMenu();
                            return;
                        }
                    }
                    else if (hand == ActionMenuHand.Left)
                    {
                        if (Input.GetAxis(Constants.LEFT_TRIGGER) >= 0.4f)
                        {
                            CloseRadialMenu();
                            return;
                        }
                    }
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    CloseRadialMenu();
                    return;
                }

                UpdateMathStuff();
                CallUpdateAction();
            }
        }

        public static void OpenRadialMenu(float startingValue, Action<float> onUpdate, string title, PedalOption pedalOption, bool restricted = false)
        {
            if (open) return;
            switch (Utilities.GetActionMenuHand())
            {
                case ActionMenuHand.Invalid:
                    return;
                case ActionMenuHand.Left:
                    current = radialPuppetMenuLeft;
                    hand = ActionMenuHand.Left;
                    open = true;
                    break;
                case ActionMenuHand.Right:
                    current = radialPuppetMenuRight;
                    hand = ActionMenuHand.Right;
                    open = true;
                    break;
                   
            }

            RadialPuppetManager.restricted = restricted;
            Input.ResetInputAxes();
            InputManager.ResetMousePos();
            current.gameObject.SetActive(true);
            current.GetFill().SetFillAngle(startingValue * 360); //Please dont break
            RadialPuppetManager.onUpdate = onUpdate;
            currentValue = startingValue;
            
            current.GetTitle().text = title;
            current.GetCenterText().text = $"{Mathf.Round(startingValue * 100f)}%";
            current.GetFill().UpdateGeometry();
            current.transform.localPosition = pedalOption.GetActionButton().transform.localPosition; //new Vector3(-256f, 0, 0); 
            var angleOriginal = Utilities.ConvertFromEuler(startingValue * 360);
            var eulerAngle = Utilities.ConvertFromDegToEuler(angleOriginal);
            var actionMenu = Utilities.GetActionMenuOpener().GetActionMenu();
            actionMenu.DisableInput();
            actionMenu.SetMainMenuOpacity(0.5f);
            current.UpdateArrow(angleOriginal, eulerAngle);
        }

        public static void CloseRadialMenu()
        {
            if (current == null) return;
            CallUpdateAction();
            current.gameObject.SetActive(false);
            current = null;
            open = false;
            hand = ActionMenuHand.Invalid;
            var actionMenu = Utilities.GetActionMenuOpener().GetActionMenu();
            actionMenu.EnableInput();
            actionMenu.SetMainMenuOpacity();
        }

        private static void CallUpdateAction()
        {
            try
            {
                onUpdate?.Invoke(current.GetFill().GetFillAngle() / 360f);
            }
            catch (Exception e)
            {
                MelonLogger.Error($"Exception caught in onUpdate action passed to Radial Puppet: {e}");
            }
        }

        private static void UpdateMathStuff()
        {
            var mousePos = hand == ActionMenuHand.Left ? InputManager.LeftInput : InputManager.RightInput;
            radialPuppetMenuRight.GetCursor().transform.localPosition = mousePos * 4;

            if (Vector2.Distance(mousePos, Vector2.zero) > 12)
            {
                var angleOriginal = Mathf.Round(Mathf.Atan2(mousePos.y, mousePos.x) * Constants.RAD_TO_DEG);
                var eulerAngle = Utilities.ConvertFromDegToEuler(angleOriginal);
                var normalisedAngle = eulerAngle / 360f;
                if (Math.Abs(normalisedAngle - currentValue) < 0.0001f) return;
                if (!restricted)
                {
                    current.SetAngle(eulerAngle);
                    current.UpdateArrow(angleOriginal, eulerAngle);
                }
                else
                {
                    if (currentValue > normalisedAngle)
                    {
                        if (currentValue - normalisedAngle < 0.5f)
                        {
                            current.SetAngle(eulerAngle);
                            current.UpdateArrow(angleOriginal, eulerAngle);
                            currentValue = normalisedAngle;
                        }
                        else
                        {
                            current.SetAngle(360);
                            current.UpdateArrow(90, 360);
                            currentValue = 1f;
                        }
                    }
                    else
                    {
                        if (normalisedAngle - currentValue < 0.5f)
                        {
                            current.SetAngle(eulerAngle);
                            current.UpdateArrow(angleOriginal, eulerAngle);
                            currentValue = normalisedAngle;
                        }
                        else
                        {
                            current.SetAngle(0);
                            current.UpdateArrow(90, 0);
                            currentValue = 0;
                        }
                    }
                }
            }
        }
    }
}
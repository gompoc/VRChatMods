using System;
using ActionMenuApi.Helpers;
using ActionMenuApi.Types;
using MelonLoader;
using UnityEngine;
using UnityEngine.XR;

namespace ActionMenuApi.Managers
{
    internal static class FourAxisPuppetManager
    {
        private static AxisPuppetMenu fourAxisPuppetMenuRight;
        private static AxisPuppetMenu fourAxisPuppetMenuLeft;
        private static ActionMenuHand hand;
        private static bool open;
        public static AxisPuppetMenu current { get; private set; }

        public static Vector2 fourAxisPuppetValue { get; set; }

        public static Action<Vector2> onUpdate { get; set; }

        public static void Setup()
        {
            fourAxisPuppetMenuLeft = Utilities
                .CloneGameObject("UserInterface/ActionMenu/MenuL/ActionMenu/AxisPuppetMenu",
                    "UserInterface/ActionMenu/MenuL/ActionMenu").GetComponent<AxisPuppetMenu>();
            fourAxisPuppetMenuRight = Utilities
                .CloneGameObject("UserInterface/ActionMenu/MenuR/ActionMenu/AxisPuppetMenu",
                    "UserInterface/ActionMenu/MenuR/ActionMenu").GetComponent<AxisPuppetMenu>();
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
                            CloseFourAxisMenu();
                            return;
                        }
                    }
                    else if (hand == ActionMenuHand.Left)
                    {
                        if (Input.GetAxis(Constants.LEFT_TRIGGER) >= 0.4f)
                        {
                            CloseFourAxisMenu();
                            return;
                        }
                    }
                }
                else if (Input.GetMouseButton(0))
                {
                    CloseFourAxisMenu();
                    return;
                }

                fourAxisPuppetValue = (hand == ActionMenuHand.Left
                    ? Utilities.GetCursorPosLeft()
                    : Utilities.GetCursorPosRight()) / 16;
                var x = fourAxisPuppetValue.x;
                var y = fourAxisPuppetValue.y;
                if (x >= 0)
                {
                    current.GetFillLeft().SetAlpha(0);
                    current.GetFillRight().SetAlpha(x);
                }
                else
                {
                    current.GetFillLeft().SetAlpha(Math.Abs(x));
                    current.GetFillRight().SetAlpha(0);
                }

                if (y >= 0)
                {
                    current.GetFillDown().SetAlpha(0);
                    current.GetFillUp().SetAlpha(y);
                }
                else
                {
                    current.GetFillDown().SetAlpha(Math.Abs(y));
                    current.GetFillUp().SetAlpha(0);
                }

                UpdateMathStuff();
                CallUpdateAction();
            }
        }

        public static void OpenFourAxisMenu(string title, Action<Vector2> update, PedalOption pedalOption)
        {
            if (open) return;
            switch (Utilities.GetActionMenuHand())
            {
                case ActionMenuHand.Invalid:
                    return;
                case ActionMenuHand.Left:
                    current = fourAxisPuppetMenuLeft;
                    hand = ActionMenuHand.Left;
                    open = true;
                    break;
                case ActionMenuHand.Right:
                    current = fourAxisPuppetMenuRight;
                    hand = ActionMenuHand.Right;
                    open = true;
                    break;
            }

            Input.ResetInputAxes();
            onUpdate = update;
            current.gameObject.SetActive(true);
            current.GetTitle().text = title;
            current.transform.localPosition = pedalOption.GetActionButton().transform.localPosition;
        }

        private static void CallUpdateAction()
        {
            try
            {
                onUpdate?.Invoke(fourAxisPuppetValue);
            }
            catch (Exception e)
            {
                MelonLogger.Error($"Exception caught in onUpdate action passed to Four Axis Puppet: {e}");
            }
        }

        public static void CloseFourAxisMenu()
        {
            if (current == null) return;
            CallUpdateAction();
            current.gameObject.SetActive(false);
            current = null;
            open = false;
            hand = ActionMenuHand.Invalid;
        }

        private static void UpdateMathStuff()
        {
            var mousePos = hand == ActionMenuHand.Left
                ? Utilities.GetCursorPosLeft()
                : Utilities.GetCursorPosRight();
            current.GetCursor().transform.localPosition = mousePos * 4;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ActionMenuApi.Pedals;
using ActionMenuApi.Types;
using HarmonyLib;
using MelonLoader;
using UnhollowerRuntimeLib.XrefScans;
using UnityEngine;
using UnityEngine.XR;
using Object = UnityEngine.Object;
using Vector2 = System.Numerics.Vector2;

namespace ActionMenuApi.Helpers
{
    internal static class Utilities
    {
        private static RefreshAMDelegate refreshAMDelegate;

        private static RefreshAMDelegate GetRefreshAMDelegate
        {
            get
            {
                //Build 1121 menu.Method_Private_Void_PDM_11)
                if (refreshAMDelegate != null) return refreshAMDelegate;
                var refreshAMMethod = typeof(ActionMenu).GetMethods().First(
                    m =>
                        m.Name.StartsWith("Method_Private_Void_PDM_")
                        && !m.HasStringLiterals()
                        && m.SameClassMethodCallCount(1) 
                        && m.HasMethodCallWithName("ThrowArgumentOutOfRangeException")
                        && !m.HasMethodWithDeclaringType(typeof(ActionMenuDriver))
                );
                refreshAMDelegate = (RefreshAMDelegate) Delegate.CreateDelegate(
                    typeof(RefreshAMDelegate),
                    null,
                    refreshAMMethod);
                return refreshAMDelegate;
            }
        }

        public static bool CheckXref(MethodBase m, params string[] keywords)
        {
            try
            {
                foreach (var keyword in keywords)
                    if (!XrefScanner.XrefScan(m).Any(
                        instance => instance.Type == XrefType.Global && instance.ReadAsObject() != null && instance
                            .ReadAsObject().ToString()
                            .Equals(keyword, StringComparison.OrdinalIgnoreCase)))
                        return false;

                return true;
            }
            catch
            {
            }

            return false;
        }

        public static bool CheckXref(MethodBase m, List<string> keywords)
        {
            try
            {
                foreach (var keyword in keywords)
                    if (!XrefScanner.XrefScan(m).Any(
                        instance => instance.Type == XrefType.Global && instance.ReadAsObject() != null && instance
                            .ReadAsObject().ToString()
                            .Equals(keyword, StringComparison.OrdinalIgnoreCase)))
                        return false;

                return true;
            }
            catch
            {
            }

            return false;
        }

        public static void AddPedalsInList(List<PedalStruct> list, ActionMenu instance)
        {
            //if (!LoaderIntegrityCheck.passed && new System.Random().Next(3) == 1) return;
            foreach (var pedalStruct in list)
            {
                if (!pedalStruct.shouldAdd) continue;
                var pedalOption = instance.AddOption();
                pedalOption.SetText(pedalStruct.text);
                if (!pedalStruct.locked) pedalOption.SetPedalAction(pedalStruct.triggerEvent);
                else pedalOption.Lock();
                //Additional setup for pedals
                switch (pedalStruct.Type)
                {
                    /*case PedalType.SubMenu:
                        pedalOption.SetPedalTypeIcon(GetExpressionsIcons().typeFolder);
                        break;*/
                    case PedalType.RadialPuppet:
                        var pedalRadial = (PedalRadial) pedalStruct;
                        pedalOption.SetPedalTypeIcon(GetExpressionsIcons().typeRadial);
                        pedalOption.SetButtonPercentText($"{Math.Round(pedalRadial.currentValue)}%");
                        pedalRadial.pedal = pedalOption;
                        pedalOption.SetBackgroundIcon(pedalStruct.icon);
                        break;
                    case PedalType.Toggle:
                        var pedalToggle = (PedalToggle) pedalStruct;
                        if (pedalToggle.toggled)
                            pedalOption.SetPedalTypeIcon(GetExpressionsIcons().typeToggleOn);
                        else
                            pedalOption.SetPedalTypeIcon(GetExpressionsIcons().typeToggleOff);
                        pedalToggle.pedal = pedalOption;
                        pedalOption.SetBackgroundIcon(pedalStruct.icon);
                        break;
                    case PedalType.FourAxisPuppet:
                        pedalOption.SetPedalTypeIcon(GetExpressionsIcons().typeAxis);
                        pedalOption.SetBackgroundIcon(pedalStruct.icon);
                        break;
                    default:
                        pedalOption.SetForegroundIcon(pedalStruct.icon);
                        break;
                }
            }
        }

        public static float ConvertFromDegToEuler(float angle)
        {
            //TODO: Rewrite/Remove Unnecessary Addition/Subtraction
            if (angle >= 0 && angle <= 90) return 90 - angle;
            if (angle > 90 && angle <= 180) return 360 - (angle - 90);
            if (angle <= -90 && angle >= -180) return 270 - (angle + 180);
            if (angle <= 0 && angle >= -90) return 180 - (angle + 180) + 90;
            return 0;
        }

        public static float ConvertFromEuler(float angle)
        {
            //TODO: Rewrite/Remove Unnecessary Addition/Subtraction
            if (angle >= 90 && angle <= 270) return (angle - 90) * -1;
            if (angle <= 360 && angle > 270) return 180 - (angle - 270);
            if (angle < 90 && angle >= 0) return 90 - angle;
            return 0;
        }

        public static GameObject CloneGameObject(string pathToGameObject, string pathToParent)
        {
            return Object
                .Instantiate(GameObject.Find(pathToGameObject).transform, GameObject.Find(pathToParent).transform)
                .gameObject;
        }

        public static ActionMenuDriver.ExpressionIcons GetExpressionsIcons()
        {
            return ActionMenuDriver.prop_ActionMenuDriver_0.field_Public_ExpressionIcons_0;
        }

        // Didnt know what to name this function
        public static ActionMenuHand GetActionMenuHand()
        {
            if (!ActionMenuDriver.prop_ActionMenuDriver_0.GetLeftOpener().isOpen() &&
                ActionMenuDriver.prop_ActionMenuDriver_0.GetRightOpener().isOpen())
                return ActionMenuHand.Right;

            if (ActionMenuDriver.prop_ActionMenuDriver_0.GetLeftOpener().isOpen() &&
                !ActionMenuDriver.prop_ActionMenuDriver_0.GetRightOpener().isOpen())
                return ActionMenuHand.Left;

            return ActionMenuHand.Invalid;
        }

        public static ActionMenuOpener GetActionMenuOpener()
        {
            if (!ActionMenuDriver.prop_ActionMenuDriver_0.GetLeftOpener().isOpen() &&
                ActionMenuDriver.prop_ActionMenuDriver_0.GetRightOpener().isOpen())
                return ActionMenuDriver.prop_ActionMenuDriver_0.GetRightOpener();

            if (ActionMenuDriver.prop_ActionMenuDriver_0.GetLeftOpener().isOpen() &&
                !ActionMenuDriver.prop_ActionMenuDriver_0.GetRightOpener().isOpen())
                return ActionMenuDriver.prop_ActionMenuDriver_0.GetLeftOpener();

            return null;
            /*
            else if (ActionMenuDriver._instance.openerL.isOpen() && ActionMenuDriver._instance.openerR.isOpen())
            {
                return null; //Which one to return ¯\_(ツ)_/¯ Mystery till I figure something smart out
            }
            */
        }

        public static void ScanMethod(MethodInfo m)
        {
            MelonLogger.Msg($"Scanning: {m.FullDescription()}");
            foreach (var instance in XrefScanner.XrefScan(m))
                try
                {
                    if (instance.Type == XrefType.Global && instance.ReadAsObject() != null)
                        try
                        {
                            MelonLogger.Msg($"   Found String: {instance.ReadAsObject().ToString()}");
                        }
                        catch
                        {
                        }
                    else if (instance.Type == XrefType.Method && instance.TryResolve() != null)
                        try
                        {
                            MelonLogger.Msg($"   Found Method: {instance.TryResolve().FullDescription()}");
                        }
                        catch
                        {
                        }
                }
                catch
                {
                }

            foreach (var instance in XrefScanner.UsedBy(m))
                try
                {
                    if (instance.Type == XrefType.Method && instance.TryResolve() != null)
                        try
                        {
                            MelonLogger.Msg($"   Found Used By Method: {instance.TryResolve().FullDescription()}");
                        }
                        catch
                        {
                        }
                }
                catch
                {
                }
        }

        public static void RefreshAM()
        {
            if (ActionMenuDriver.prop_ActionMenuDriver_0 == null)
            {
                Logger.LogWarning("Refresh called before driver init");
                return;
            }

            var leftOpener = ActionMenuDriver.prop_ActionMenuDriver_0.GetLeftOpener();
            GetRefreshAMDelegate(leftOpener.GetActionMenu());
            var rightOpener = ActionMenuDriver.prop_ActionMenuDriver_0.GetRightOpener();
            GetRefreshAMDelegate(rightOpener.GetActionMenu());
        }

        public static void ResetMenu()
        {
            if (ActionMenuDriver.prop_ActionMenuDriver_0 == null)
            {
                Logger.LogWarning("Reset called before driver init");
                return;
            }

            var leftOpener = ActionMenuDriver.prop_ActionMenuDriver_0.GetLeftOpener();
            if (leftOpener.isOpen())
            {
                leftOpener.GetActionMenu().Reset();
                //leftOpener.GetActionMenu().ResetMenu();
            }
            var rightOpener = ActionMenuDriver.prop_ActionMenuDriver_0.GetRightOpener();
            if (rightOpener.isOpen())
            {
                rightOpener.GetActionMenu().Reset();
                //rightOpener.GetActionMenu().ResetMenu();
            }
        }
        
        public static (double x1, double y1, double x2, double y2) GetIntersection(float x, float y, float r)
        {
            var tmp = Math.Pow(y / x, 2);
            var c4 = -Math.Pow(r, 2)*-4;
            var x1 = Math.Sqrt(c4 + c4*tmp) / (2+2*tmp);
            var x2 = -x1;
            return (x1, x1 * (y / x), x2, x2 * (y / x));
        }

        private delegate void RefreshAMDelegate(ActionMenu actionMenu);
    }
}
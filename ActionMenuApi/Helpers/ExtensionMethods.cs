using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ActionMenuApi.Managers;
using MelonLoader;
using TMPro;
using UnhollowerRuntimeLib;
using UnhollowerRuntimeLib.XrefScans;
using UnityEngine;
using ActionMenuPage = ActionMenu.Page; //Will this change?, ¯\_(ツ)_/¯x2
using Object = UnityEngine.Object;

namespace ActionMenuApi.Helpers
{
    internal static class ExtensionMethods
    {
        private static AddOptionDelegate addOptionDelegate;

        private static PushPageDelegate pushPageDelegate;

        private static PropertyInfo actionButtonPercentProperty;

        private static PropertyInfo radialPuppetCursorProperty;

        private static PropertyInfo axisPuppetCursorProperty;

        private static PropertyInfo radialPuppetArrowProperty;

        private static PropertyInfo axisPuppetFillUpProperty;

        private static PropertyInfo axisPuppetFillRightProperty;

        private static PropertyInfo axisPuppetFillDownProperty;

        private static PropertyInfo axisPuppetFillLeftProperty;

        private static PropertyInfo actionButtonTextProperty;

        private static PropertyInfo axisPuppetButtonUpProperty;

        private static PropertyInfo axisPuppetButtonRightProperty;

        private static PropertyInfo axisPuppetButtonDownProperty;

        private static PropertyInfo axisPuppetButtonLeftProperty;

        private static PropertyInfo pedalOptionPrefabProperty;

        private static ClosePuppetMenusDelegate closePuppetMenusDelegate;

        private static DestroyPageDelegate destroyPageDelegate;

        private static Func<RadialPuppetMenu, GameObject> getRadialCursorGameObjectDelegate;

        private static Func<AxisPuppetMenu, GameObject> getAxisCursorGameObjectDelegate;

        private static Func<RadialPuppetMenu, GameObject> getRadialArrowGameObjectDelegate;

        private static Func<AxisPuppetMenu, PedalGraphic> getAxisFillUpDelegate;

        private static Func<AxisPuppetMenu, PedalGraphic> getAxisFillRightDelegate;

        private static Func<AxisPuppetMenu, PedalGraphic> getAxisFillDownDelegate;

        private static Func<AxisPuppetMenu, PedalGraphic> getAxisFillLeftDelegate;

        private static Action<ActionButton, string> setActionButtonText;

        private static Func<AxisPuppetMenu, ActionButton> getAxisPuppetButtonUpDelegate;

        private static Func<AxisPuppetMenu, ActionButton> getAxisPuppetButtonRightDelegate;

        private static Func<AxisPuppetMenu, ActionButton> getAxisPuppetButtonDownDelegate;

        private static Func<AxisPuppetMenu, ActionButton> getAxisPuppetButtonLeftDelegate;

        private static AddOptionDelegate GetAddOptionDelegate
        {
            get
            {
                //Build 1088 menu.Method_Private_PedalOption_0()
                if (addOptionDelegate != null) return addOptionDelegate;
                var addOptionMethod = typeof(ActionMenu).GetMethods(BindingFlags.Public | BindingFlags.Instance)
                    .First(
                        m => m.GetParameters().Length == 0 && m.Name.StartsWith("Method_Private_PedalOption_") &&
                             !m.Name.Contains("PDM"));

                addOptionDelegate = (AddOptionDelegate) Delegate.CreateDelegate(
                    typeof(AddOptionDelegate),
                    null,
                    addOptionMethod);
                return addOptionDelegate;
            }
        }


        private static PushPageDelegate GetPushPageDelegate
        {
            get
            {
                //Build 1088 menu.Method_Public_ObjectNPublicAcTeAcStGaUnique_Action_Action_Texture2D_String_0(openFunc, closeFunc, icon, text);
                if (pushPageDelegate != null) return pushPageDelegate;
                var pushPageMethod = typeof(ActionMenu).GetMethods(BindingFlags.Public | BindingFlags.Instance)
                    .First( //Not scuffed I swear (theres only 1 of these methods( ignoring pdms))
                        m => m.GetParameters().Length == 4
                             && m.ReturnType == typeof(ActionMenuPage)
                             && m.GetParameters()[0].ParameterType == typeof(Il2CppSystem.Action)
                             && m.GetParameters()[1].ParameterType == typeof(Il2CppSystem.Action)
                             && m.GetParameters()[2].ParameterType == typeof(Texture2D)
                             && m.GetParameters()[3].ParameterType == typeof(string)
                             && !m.Name.Contains("PDM")
                    );

                pushPageDelegate = (PushPageDelegate) Delegate.CreateDelegate(
                    typeof(PushPageDelegate),
                    null,
                    pushPageMethod);
                return pushPageDelegate;
            }
        }

        private static ClosePuppetMenusDelegate GetClosePuppetMenusDelegate
        {
            get
            {
                //Build 1088 menu.Method_Public_Void_Boolean_2()
                if (closePuppetMenusDelegate != null) return closePuppetMenusDelegate;
                var closePuppetMenusMethod = typeof(ActionMenu).GetMethods().Single(
                    m => m.Name.StartsWith("Method_Public_Void_Boolean_")
                         && m.GetParameters().Length == 1
                         && m.GetParameters()[0].IsOptional
                         && !m.Name.Contains("PDM")
                );
                closePuppetMenusDelegate = (ClosePuppetMenusDelegate) Delegate.CreateDelegate(
                    typeof(ClosePuppetMenusDelegate),
                    null,
                    closePuppetMenusMethod);
                return closePuppetMenusDelegate;
            }
        }

        private static DestroyPageDelegate GetDestroyPageDelegate
        {
            get
            {
                //Build 1088 menu.Method_Private_Void_ObjectNPublicAcTeAcStGaUnique_0()
                if (destroyPageDelegate != null) return destroyPageDelegate;
                var destroyPageMethod = typeof(ActionMenu).GetMethods().Single(
                    m =>
                        m.Name.StartsWith("Method_Private_Void_ObjectNPublicAcTeAcStGaUnique_")
                        && m.GetParameters().Length == 1
                        && !m.Name.Contains("PDM")
                        && m.CheckStringsCount(1)
                );
                destroyPageDelegate = (DestroyPageDelegate) Delegate.CreateDelegate(
                    typeof(DestroyPageDelegate),
                    null,
                    destroyPageMethod);
                return destroyPageDelegate;
            }
        }

        public static PedalOption AddOption(this ActionMenu menu)
        {
            return
                GetAddOptionDelegate.Invoke(menu); //Honestly I have no idea how or why this works but hey... it works
        }


        public static void SetText(this PedalOption pedal, string text)
        {
            pedal.prop_String_0 = text;
        }

        public static string GetText(this PedalOption pedal)
        {
            return
                pedal.prop_String_0; //Only string prop on PedalOption. shouldnt change unless drastic changes are made to the action menu
        }

        public static void PushPage(this ActionMenu menu, Action openFunc, Action closeFunc = null,
            Texture2D icon = null, string text = null)
        {
            GetPushPageDelegate.Invoke(menu, openFunc, closeFunc, icon, text);
        }


        public static void SetBackgroundIcon(this PedalOption pedal, Texture2D icon)
        {
            pedal.GetActionButton().prop_Texture2D_0 = icon;
        }

        //Only texture2d prop on PedalOption. shouldnt change unless drastic changes are made to the action menu
        public static void SetForegroundIcon(this PedalOption pedal, Texture2D icon)
        {
            pedal.prop_Texture2D_0 = icon;
        }

        public static bool isOpen(this ActionMenuOpener actionMenuOpener)
        {
            return actionMenuOpener.field_Private_Boolean_0; //only bool on action menu opener, shouldnt change
        }

        public static void
            SetButtonPercentText(this PedalOption pedalOption,
                string text) //pedalOption.GetActionButton().prop_String_1
        {
            if (actionButtonPercentProperty != null)
            {
                actionButtonPercentProperty.SetValue(pedalOption.GetActionButton(), text);
                return;
            }

            var button = pedalOption.GetActionButton();
            actionButtonPercentProperty = typeof(ActionButton)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance).Single(
                    p => p.PropertyType == typeof(string) && ((string) p.GetValue(button)).Equals("100%")
                );
            actionButtonPercentProperty.SetValue(pedalOption.GetActionButton(), text);
        }

        public static ActionButton GetActionButton(this PedalOption pedalOption)
        {
            return pedalOption.field_Public_ActionButton_0; //only one
        }

        private static void SetPedalTriggerEvent(this PedalOption pedalOption, Func<bool> triggerEvent)
        {
            pedalOption.field_Public_Func_1_Boolean_0 = triggerEvent;
        }
        
        public static void SetPedalAction(this PedalOption pedalOption, Action action)
        {
            pedalOption.SetPedalTriggerEvent(delegate
            {
                action();
                return true;
            });
        }

        public static ActionMenuOpener GetLeftOpener(this ActionMenuDriver actionMenuDriver)
        {
            if (actionMenuDriver.field_Public_ActionMenuOpener_0.field_Public_Hand_0 ==
                ActionMenuOpener.Hand.Left)
                return actionMenuDriver.field_Public_ActionMenuOpener_0;
            return actionMenuDriver.field_Public_ActionMenuOpener_1;
        }

        public static ActionMenuOpener GetRightOpener(this ActionMenuDriver actionMenuDriver)
        {
            if (actionMenuDriver.field_Public_ActionMenuOpener_1.field_Public_Hand_0 == ActionMenuOpener.Hand.Right)
                return actionMenuDriver.field_Public_ActionMenuOpener_1;
            return actionMenuDriver.field_Public_ActionMenuOpener_0;
        }

        public static ActionMenu GetActionMenu(this ActionMenuOpener actionMenuOpener)
        {
            return actionMenuOpener.field_Public_ActionMenu_0;
        }

        private static GameObject
            GetRadialCursorGameObject(
                RadialPuppetMenu radialPuppetMenu) //Build 1088 radialPuppetMenu.field_Public_GameObject_0
        {
            if (radialPuppetCursorProperty != null)
                return getRadialCursorGameObjectDelegate(radialPuppetMenu);
            radialPuppetCursorProperty = typeof(RadialPuppetMenu)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance).Single(
                    p => p.PropertyType == typeof(GameObject) &&
                         ((GameObject) p.GetValue(radialPuppetMenu)).name.Equals("Cursor")
                );
            getRadialCursorGameObjectDelegate =
                (Func<RadialPuppetMenu, GameObject>) Delegate.CreateDelegate(typeof(Func<RadialPuppetMenu, GameObject>),
                    radialPuppetCursorProperty.GetGetMethod());
            return getRadialCursorGameObjectDelegate(radialPuppetMenu);
        }

        public static GameObject GetCursor(this RadialPuppetMenu radialPuppetMenu)
        {
            return GetRadialCursorGameObject(radialPuppetMenu);
        }

        private static GameObject
            GetAxisCursorGameObject(AxisPuppetMenu axisPuppetMenu) //Build 1088 axisPuppetMenu.field_Public_GameObject_0
        {
            if (axisPuppetCursorProperty != null)
                return getAxisCursorGameObjectDelegate(axisPuppetMenu);
            axisPuppetCursorProperty = typeof(AxisPuppetMenu).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Single(
                    p => p.PropertyType == typeof(GameObject) &&
                         ((GameObject) p.GetValue(axisPuppetMenu)).name.Equals("Cursor")
                );
            getAxisCursorGameObjectDelegate =
                (Func<AxisPuppetMenu, GameObject>) Delegate.CreateDelegate(typeof(Func<AxisPuppetMenu, GameObject>),
                    axisPuppetCursorProperty.GetGetMethod());
            return getAxisCursorGameObjectDelegate(axisPuppetMenu);
        }

        public static GameObject GetCursor(this AxisPuppetMenu axisPuppetMenu)
        {
            return GetAxisCursorGameObject(axisPuppetMenu);
        }

        private static GameObject
            GetRadialArrowGameObject(
                RadialPuppetMenu radialPuppetMenu) //Build 1088 radialPuppetMenu.field_Public_GameObject_0
        {
            if (radialPuppetArrowProperty != null)
                return getRadialArrowGameObjectDelegate(radialPuppetMenu);
            radialPuppetArrowProperty = typeof(RadialPuppetMenu)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance).Single(
                    p => p.PropertyType == typeof(GameObject) &&
                         ((GameObject) p.GetValue(radialPuppetMenu)).name.Equals("Arrow")
                );
            getRadialArrowGameObjectDelegate =
                (Func<RadialPuppetMenu, GameObject>) Delegate.CreateDelegate(typeof(Func<RadialPuppetMenu, GameObject>),
                    radialPuppetArrowProperty.GetGetMethod());
            return getRadialArrowGameObjectDelegate(radialPuppetMenu);
        }

        public static GameObject GetArrow(this RadialPuppetMenu radialPuppetMenu)
        {
            return GetRadialArrowGameObject(radialPuppetMenu);
        }

        public static PedalGraphic GetFill(this RadialPuppetMenu radialPuppetMenu)
        {
            return radialPuppetMenu.field_Public_PedalGraphic_0; //only one
        }

        public static TextMeshProUGUI GetTitle(this RadialPuppetMenu radialPuppetMenu)
        {
            return ((PuppetMenu) radialPuppetMenu).field_Public_TextMeshProUGUI_0; //only one
        }

        public static TextMeshProUGUI GetTitle(this AxisPuppetMenu axisPuppetMenu)
        {
            return axisPuppetMenu.field_Public_TextMeshProUGUI_0; //only one
        }

        public static TextMeshProUGUI GetCenterText(this RadialPuppetMenu radialPuppetMenu)
        {
            return radialPuppetMenu.field_Public_TextMeshProUGUI_0; //only one
        }

        public static PedalGraphic
            GetFillUp(this AxisPuppetMenu axisPuppetMenu) //Build 1088 axisPuppetMenu.field_Public_PedalGraphic_0
        {
            if (axisPuppetFillUpProperty != null)
                return getAxisFillUpDelegate(axisPuppetMenu);
            axisPuppetFillUpProperty = typeof(AxisPuppetMenu).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Single(
                    p => p.PropertyType == typeof(PedalGraphic) &&
                         ((PedalGraphic) p.GetValue(axisPuppetMenu)).name.Equals("Fill Up")
                );
            getAxisFillUpDelegate =
                (Func<AxisPuppetMenu, PedalGraphic>) Delegate.CreateDelegate(typeof(Func<AxisPuppetMenu, PedalGraphic>),
                    axisPuppetFillUpProperty.GetGetMethod());
            return getAxisFillUpDelegate(axisPuppetMenu);
        }

        public static PedalGraphic
            GetFillRight(this AxisPuppetMenu axisPuppetMenu) //Build 1088 axisPuppetMenu.field_Public_PedalGraphic_1
        {
            if (axisPuppetFillRightProperty != null)
                return getAxisFillRightDelegate(axisPuppetMenu);
            axisPuppetFillRightProperty = typeof(AxisPuppetMenu)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance).Single(
                    p => p.PropertyType == typeof(PedalGraphic) &&
                         ((PedalGraphic) p.GetValue(axisPuppetMenu)).name.Equals("Fill Right")
                );
            getAxisFillRightDelegate =
                (Func<AxisPuppetMenu, PedalGraphic>) Delegate.CreateDelegate(typeof(Func<AxisPuppetMenu, PedalGraphic>),
                    axisPuppetFillRightProperty.GetGetMethod());
            return getAxisFillRightDelegate(axisPuppetMenu);
        }

        public static PedalGraphic
            GetFillDown(this AxisPuppetMenu axisPuppetMenu) //Build 1088 axisPuppetMenu.field_Public_PedalGraphic_2
        {
            if (axisPuppetFillDownProperty != null)
                return getAxisFillDownDelegate(axisPuppetMenu);
            axisPuppetFillDownProperty = typeof(AxisPuppetMenu)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance).Single(
                    p => p.PropertyType == typeof(PedalGraphic) &&
                         ((PedalGraphic) p.GetValue(axisPuppetMenu)).name.Equals("Fill Down")
                );
            getAxisFillDownDelegate =
                (Func<AxisPuppetMenu, PedalGraphic>) Delegate.CreateDelegate(typeof(Func<AxisPuppetMenu, PedalGraphic>),
                    axisPuppetFillDownProperty.GetGetMethod());
            return getAxisFillDownDelegate(axisPuppetMenu);
        }

        public static PedalGraphic
            GetFillLeft(this AxisPuppetMenu axisPuppetMenu) //Build 1088 axisPuppetMenu.field_Public_PedalGraphic_3
        {
            if (axisPuppetFillLeftProperty != null)
                return getAxisFillLeftDelegate(axisPuppetMenu);
            axisPuppetFillLeftProperty = typeof(AxisPuppetMenu)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance).Single(
                    p => p.PropertyType == typeof(PedalGraphic) &&
                         ((PedalGraphic) p.GetValue(axisPuppetMenu)).name.Equals("Fill Left")
                );
            getAxisFillLeftDelegate =
                (Func<AxisPuppetMenu, PedalGraphic>) Delegate.CreateDelegate(
                    typeof(Func<AxisPuppetMenu, PedalGraphic>), axisPuppetFillLeftProperty.GetGetMethod());
            return getAxisFillLeftDelegate(axisPuppetMenu);
        }

        public static void SetButtonText(this ActionButton actionButton, string text) //actionButton.prop_String_0
        {
            if (actionButtonTextProperty != null)
            {
                setActionButtonText(actionButton, text);
                return;
            }

            actionButtonTextProperty = typeof(ActionButton).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Single(
                    p => p.PropertyType == typeof(string) && ((string) p.GetValue(actionButton)).Equals("Button Text")
                );
            setActionButtonText =
                (Action<ActionButton, string>) Delegate.CreateDelegate(typeof(Action<ActionButton, string>),
                    actionButtonTextProperty.GetSetMethod());
            setActionButtonText(actionButton, text);
        }

        public static ActionButton
            GetButtonUp(this AxisPuppetMenu axisPuppetMenu) //Build 1088 axisPuppetMenu.field_Public_ActionButton_0
        {
            if (axisPuppetButtonUpProperty != null)
                return getAxisPuppetButtonUpDelegate(axisPuppetMenu);
            axisPuppetButtonUpProperty = typeof(AxisPuppetMenu)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance).Single(
                    p => p.PropertyType == typeof(ActionButton) &&
                         ((ActionButton) p.GetValue(axisPuppetMenu)).name.Equals("ButtonUp")
                );
            getAxisPuppetButtonUpDelegate =
                (Func<AxisPuppetMenu, ActionButton>) Delegate.CreateDelegate(typeof(Func<AxisPuppetMenu, ActionButton>),
                    axisPuppetButtonUpProperty.GetGetMethod());
            return getAxisPuppetButtonUpDelegate(axisPuppetMenu);
        }

        public static ActionButton
            GetButtonRight(this AxisPuppetMenu axisPuppetMenu) //Build 1088 axisPuppetMenu.field_Public_ActionButton_1
        {
            if (axisPuppetButtonRightProperty != null)
                return getAxisPuppetButtonRightDelegate(axisPuppetMenu);
            axisPuppetButtonRightProperty = typeof(AxisPuppetMenu)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance).Single(
                    p => p.PropertyType == typeof(ActionButton) &&
                         ((ActionButton) p.GetValue(axisPuppetMenu)).name.Equals("ButtonRight")
                );
            getAxisPuppetButtonRightDelegate =
                (Func<AxisPuppetMenu, ActionButton>) Delegate.CreateDelegate(
                    typeof(Func<AxisPuppetMenu, ActionButton>), axisPuppetButtonRightProperty.GetGetMethod());
            return getAxisPuppetButtonRightDelegate(axisPuppetMenu);
        }

        public static ActionButton
            GetButtonDown(this AxisPuppetMenu axisPuppetMenu) //Build 1088 axisPuppetMenu.field_Public_ActionButton_2
        {
            if (axisPuppetButtonDownProperty != null)
                return getAxisPuppetButtonDownDelegate(axisPuppetMenu);
            axisPuppetButtonDownProperty = typeof(AxisPuppetMenu)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance).Single(
                    p => p.PropertyType == typeof(ActionButton) &&
                         ((ActionButton) p.GetValue(axisPuppetMenu)).name.Equals("ButtonDown")
                );
            getAxisPuppetButtonDownDelegate =
                (Func<AxisPuppetMenu, ActionButton>) Delegate.CreateDelegate(typeof(Func<AxisPuppetMenu, ActionButton>),
                    axisPuppetButtonDownProperty.GetGetMethod());
            return getAxisPuppetButtonDownDelegate(axisPuppetMenu);
        }

        public static ActionButton
            GetButtonLeft(this AxisPuppetMenu axisPuppetMenu) //Build 1088 axisPuppetMenu.field_Public_ActionButton_3
        {
            if (axisPuppetButtonLeftProperty != null)
                return getAxisPuppetButtonLeftDelegate(axisPuppetMenu);
            axisPuppetButtonLeftProperty = typeof(AxisPuppetMenu)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance).Single(
                    p => p.PropertyType == typeof(ActionButton) &&
                         ((ActionButton) p.GetValue(axisPuppetMenu)).name.Equals("ButtonLeft")
                );
            getAxisPuppetButtonLeftDelegate =
                (Func<AxisPuppetMenu, ActionButton>) Delegate.CreateDelegate(typeof(Func<AxisPuppetMenu, ActionButton>),
                    axisPuppetButtonLeftProperty.GetGetMethod());

            return getAxisPuppetButtonLeftDelegate(axisPuppetMenu);
        }

        // Not going to bother adding a delegate for this as its only called once on startup
        public static GameObject
            GetPedalOptionPrefab(this ActionMenu actionMenu) //Build 1093 menu.field_Public_GameObject_1
        {
            if (pedalOptionPrefabProperty != null) return (GameObject) pedalOptionPrefabProperty.GetValue(actionMenu);
            pedalOptionPrefabProperty = typeof(ActionMenu).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Single(
                    p => p.PropertyType == typeof(GameObject) &&
                         ((GameObject) p.GetValue(actionMenu)).name.Equals("PedalOption")
                );
            return (GameObject) pedalOptionPrefabProperty.GetValue(actionMenu);
        }

        public static void SetAlpha(this PedalGraphic pedalGraphic, float amount)
        {
            var temp = pedalGraphic.color;
            temp.a = amount;
            pedalGraphic.color = temp;
        }

        public static void Lock(this PedalOption pedalOption)
        {
            pedalOption.prop_Boolean_0 = true;
            ResourcesManager.AddLockChildIcon(pedalOption.GetActionButton().gameObject.GetChild("Inner"));
        }


        public static void SetAngle(this RadialPuppetMenu radialPuppet, float angle)
        {
            radialPuppet.GetFill().SetFillAngle(angle);
            radialPuppet.UpdateDisplay();
        }

        public static void SetValue(this RadialPuppetMenu radialPuppet, float value)
        {
            radialPuppet.GetFill().SetFillAngle(value / 100 * 360);
            radialPuppet.UpdateDisplay();
        }

        public static void UpdateDisplay(this RadialPuppetMenu radialPuppet)
        {
            //MelonLogger.Msg($"Original: {radialPuppet.GetFill().field_Public_Single_3}, Math:{(radialPuppet.GetFill().field_Public_Single_3  / 360f)*100f}");
            radialPuppet.GetCenterText().text = Math.Round(radialPuppet.GetFill().GetFillAngle() / 360f * 100f) + "%";
            radialPuppet.GetFill().UpdateGeometry();
        }

        public static void UpdateArrow(this RadialPuppetMenu radialPuppet, float angleOriginal, float eulerAngle)
        {
            //MelonLogger.Msg($"Original: {angleOriginal}, Euler Angle:{eulerAngle}");
            radialPuppet.GetArrow().transform.localPosition = new Vector3(
                120 * Mathf.Cos(angleOriginal / Constants.RAD_TO_DEG),
                120 * Mathf.Sin(angleOriginal / Constants.RAD_TO_DEG),
                radialPuppet.GetArrow().transform.localPosition.z);
            radialPuppet.GetArrow().transform.localEulerAngles = new Vector3(
                radialPuppet.GetArrow().transform.localEulerAngles.x,
                radialPuppet.GetArrow().transform.localEulerAngles.y, 180 - eulerAngle);
        }

        public static void ClosePuppetMenus(this ActionMenu actionMenu, bool canResetValue)
        {
            GetClosePuppetMenusDelegate(actionMenu, canResetValue);
        }

        public static void DestroyPage(this ActionMenu actionMenu, ActionMenuPage page)
        {
            GetDestroyPageDelegate(actionMenu, page);
        }

        public static void ResetMenu(this ActionMenu actionMenu)
        {
            RadialPuppetManager.CloseRadialMenu();
            FourAxisPuppetManager.CloseFourAxisMenu();
            actionMenu.ClosePuppetMenus(true);
            for (var i = 0; i < actionMenu.field_Private_List_1_Page_0.Count; i++)
                actionMenu.DestroyPage(actionMenu.field_Private_List_1_Page_0._items[i]);
            actionMenu.field_Private_List_1_Page_0?.Clear();
            //actionMenu.field_Public_List_1_ObjectNPublicPaSiAcObUnique_0?.Clear();
        }

        public static List<List<T>> Split<T>(this List<T> ourList, int chunkSize)
        {
            var list = new List<List<T>>();
            for (var i = 0; i < ourList.Count; i += chunkSize)
                list.Add(ourList.GetRange(i, Math.Min(chunkSize, ourList.Count - i)));

            return list;
        }

        public static bool HasStringLiterals(this MethodInfo m)
        {
            foreach (var instance in XrefScanner.XrefScan(m))
                try
                {
                    if (instance.Type == XrefType.Global && instance.ReadAsObject() != null) return true;
                }
                catch
                {
                }

            return false;
        }

        public static bool CheckStringsCount(this MethodInfo m, int count)
        {
            var total = 0;
            foreach (var instance in XrefScanner.XrefScan(m))
                try
                {
                    if (instance.Type == XrefType.Global && instance.ReadAsObject() != null) total++;
                }
                catch
                {
                }

            return total == count;
        }

        public static bool HasMethodCallWithName(this MethodInfo m, string txt)
        {
            foreach (var instance in XrefScanner.XrefScan(m))
                try
                {
                    if (instance.Type == XrefType.Method && instance.TryResolve() != null)
                        try
                        {
                            if (instance.TryResolve().Name.Contains(txt)) return true;
                        }
                        catch (Exception e)
                        {
                            MelonLogger.Warning(e);
                        }
                }
                catch
                {
                }

            return false;
        }

        public static bool SameClassMethodCallCount(this MethodInfo m, int calls)
        {
            var count = 0;
            foreach (var instance in XrefScanner.XrefScan(m))
                try
                {
                    if (instance.Type == XrefType.Method && instance.TryResolve() != null)
                        try
                        {
                            if (m.DeclaringType == instance.TryResolve().DeclaringType) count++;
                        }
                        catch (Exception e)
                        {
                            MelonLogger.Warning(e);
                        }
                }
                catch
                {
                }

            return count == calls;
        }

        public static bool HasMethodWithDeclaringType(this MethodInfo m, Type declaringType)
        {
            foreach (var instance in XrefScanner.XrefScan(m))
                try
                {
                    if (instance.Type == XrefType.Method && instance.TryResolve() != null)
                        try
                        {
                            if (declaringType == instance.TryResolve().DeclaringType) return true;
                        }
                        catch (Exception e)
                        {
                            MelonLogger.Warning(e);
                        }
                }
                catch
                {
                }

            return false;
        }

        public static GameObject Clone(this GameObject gameObject)
        {
            return Object
                .Instantiate(gameObject.transform, gameObject.transform.parent)
                .gameObject;
        }

        public static GameObject GetChild(this GameObject gameObject, string childName)
        {
            //MelonLogger.Msg($"Gameobject: {gameObject.name},   Child Searching for: {childName}");
            for (var i = 0; i < gameObject.transform.childCount; i++)
            {
                var child = gameObject.transform.GetChild(i).gameObject;
                //MelonLogger.Msg("   "+child.name);
                if (child.name.Equals(childName)) return child;
            }

            return null;
        }

        //These things might change, just a bit tricky to identify the correct ones using reflection
        public static void SetFillAngle(this PedalGraphic pedalGraphic, float angle)
        {
            pedalGraphic.field_Public_Single_3 = angle;
        }

        public static float GetFillAngle(this PedalGraphic pedalGraphic)
        {
            return pedalGraphic.field_Public_Single_3;
        }


        private static PropertyInfo mainCanvasGroupProperty;
        private static Func<ActionMenu, CanvasGroup> getActionMenuMainMenuCanvasGroup;
        private static CanvasGroup GetMainMenuCanvas(this ActionMenu actionMenu) 
        {
            if (mainCanvasGroupProperty != null)
                return getActionMenuMainMenuCanvasGroup(actionMenu);

            mainCanvasGroupProperty = typeof(ActionMenu).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Single(p => p.PropertyType == typeof(CanvasGroup) && ((CanvasGroup) p.GetValue(actionMenu)).gameObject.name.Equals("Main"));
            getActionMenuMainMenuCanvasGroup = (Func<ActionMenu, CanvasGroup>) Delegate.CreateDelegate(typeof(Func<ActionMenu, CanvasGroup>), mainCanvasGroupProperty.GetGetMethod());
            return getActionMenuMainMenuCanvasGroup(actionMenu);
        }

        public static void SetMainMenuOpacity(this ActionMenu actionMenu, float opacity = 1.0f)
        {
            GetMainMenuCanvas(actionMenu).alpha = opacity;
        }

        public static void DisableInput(this ActionMenu actionMenu)
        {
            actionMenu.field_Private_Boolean_3 = false;
        }

        public static void EnableInput(this ActionMenu actionMenu)
        {
            actionMenu.field_Private_Boolean_3 = true;
        }

        public static Vector2 GetCursorPos(this ActionMenu actionMenu)
        {
            return actionMenu.field_Private_Vector2_0;
        }

        public static void SetPedalTypeIcon(this PedalOption pedalOption, Texture2D icon)
        {
            pedalOption.GetActionButton().prop_Texture2D_2 = icon; //No choice needs to be hardcoded in sadly
        }

        private delegate PedalOption AddOptionDelegate(ActionMenu actionMenu);

        private delegate ActionMenuPage PushPageDelegate(ActionMenu actionMenu, Il2CppSystem.Action openFunc, Il2CppSystem.Action closeFunc = null, Texture2D icon = null, string text = null);

        private delegate void ClosePuppetMenusDelegate(ActionMenu actionMenu, bool canResetValue);

        private delegate void DestroyPageDelegate(ActionMenu actionMenu, ActionMenuPage page);
    }
}
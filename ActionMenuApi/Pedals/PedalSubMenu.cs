using System;
using ActionMenuApi.Helpers;
using ActionMenuApi.Types;
using UnityEngine;

namespace ActionMenuApi.Pedals
{
    public sealed class PedalSubMenu : PedalStruct
    {
        public PedalSubMenu(Action openFunc, string text = null, Texture2D icon = null, Action closeFunc = null,
            bool locked = false)
        {
            this.text = text;
            this.icon = icon;
            this.openFunc = openFunc;
            this.closeFunc = closeFunc;
            triggerEvent = delegate
            {
                if (Utilities.GetActionMenuOpener() == null) return;
                Utilities.GetActionMenuOpener().GetActionMenu().PushPage(openFunc, closeFunc, icon, text);
            };
            Type = PedalType.SubMenu;
            this.locked = locked;
        }

        public Action openFunc { get; set; }
        public Action closeFunc { get; set; }
    }
}
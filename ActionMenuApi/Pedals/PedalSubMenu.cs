using System;
using ActionMenuApi.Helpers;
using ActionMenuApi.Types;
using MelonLoader;
using UnityEngine;

namespace ActionMenuApi.Pedals
{
    public sealed class PedalSubMenu : PedalStruct
    {
        public PedalSubMenu(Action onSubMenuOpen, string text = null, Texture2D icon = null, Action onSubMenuClose = null, bool locked = false)
        {
            this.text = text;
            this.icon = icon;
            this.OnSubMenuOpen += onSubMenuOpen;
            this.OnSubMenuClose += onSubMenuClose;
            this.OnSubMenuClose += delegate
            {
                IsOpen = false;
            };
            triggerEvent = delegate(ActionMenu menu)
            {
                IsOpen = true;
                menu.PushPage(this._openFunc, this._closeFunc, icon, text);
            };
            Type = PedalType.SubMenu;
            this.locked = locked;
        }

        private Action _openFunc;
        /// <summary>
        /// Triggered when the submenu is opened *duh*
        /// </summary>
        public event Action OnSubMenuOpen
        {
            add
            {
                if (_openFunc is null)
                    _openFunc = value;
                else
                    _openFunc = (Action)Delegate.Combine(_openFunc, value);
            }
            remove
            {
                if (_openFunc is not null)
                    _openFunc = (Action)Delegate.Remove(_openFunc, value);
            }
        }

        private Action _closeFunc;
        
        /// <summary>
        /// Triggered when the sub menu is close *duh*
        /// </summary>
        public event Action OnSubMenuClose
        {
            add
            {
                if (_closeFunc is null)
                    _closeFunc = value;
                else
                    _closeFunc = (Action)Delegate.Combine(_closeFunc, value);
            }
            remove
            {
                if (_closeFunc is not null)
                    _closeFunc = (Action)Delegate.Remove(_closeFunc, value);
            }
        }

        public bool IsOpen { get; internal set; }
    }
}
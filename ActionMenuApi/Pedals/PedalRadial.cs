using System;
using ActionMenuApi.Helpers;
using ActionMenuApi.Managers;
using ActionMenuApi.Types;
using UnityEngine;

namespace ActionMenuApi.Pedals
{
    public sealed class PedalRadial : PedalStruct
    {
        public float currentValue;

        public PedalRadial(string text, float startingValue, Texture2D icon, Action<float> onUpdate,
            bool locked = false, bool restricted = false)
        {
            this.text = text;
            currentValue = startingValue;
            this.icon = icon;
            triggerEvent = delegate
            {
                var combinedAction = (Action<float>) Delegate.Combine(new Action<float>(delegate(float f)
                {
                    startingValue = f;
                    pedal.SetButtonPercentText($"{Math.Round(startingValue * 100)}%");
                }), onUpdate);
                RadialPuppetManager.OpenRadialMenu(startingValue, combinedAction, text, pedal, restricted);
            };
            Type = PedalType.RadialPuppet;
            this.locked = locked;
            this.restricted = restricted;
        }

        public PedalOption pedal { get; set; }
        public bool restricted { get; }
    }
}
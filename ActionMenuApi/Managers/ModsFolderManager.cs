using System;
using System.Collections.Generic;
using ActionMenuApi.Api;
using ActionMenuApi.Helpers;

namespace ActionMenuApi.Managers
{
    internal static class ModsFolderManager
    {
        public static List<Action> mods = new();
        public static List<List<Action>> splitMods;

        private static readonly Action openFunc = () =>
        {
            if (mods.Count <= Constants.MAX_PEDALS_PER_PAGE)
            {
                foreach (var action in mods) action();
            }
            else
            {
                if (splitMods == null) splitMods = mods.Split(Constants.MAX_PEDALS_PER_PAGE);
                for (var i = 0; i < splitMods.Count && i < Constants.MAX_PEDALS_PER_PAGE; i++)
                {
                    var index = i;
                    CustomSubMenu.AddSubMenu($"Page {i + 1}", () =>
                    {
                        foreach (var action in splitMods[index]) action();
                    }, ResourcesManager.GetPageIcon(i + 1));
                }
            }
        };

        public static void AddMod(Action openingAction)
        {
            mods.Add(openingAction);
        }

        /*public void RemoveMod(Action openingAction)
        {
            mods.Remove(openingAction);
        }*/

        public static void AddMainPageButton()
        {
            CustomSubMenu.AddSubMenu(Constants.MODS_FOLDER_NAME, openFunc, ResourcesManager.GetModsSectionIcon());
        }
    }
}
using System;
using System.Linq;
using UnityEngine;
using VRC;
using UIExpansionKit.API;
using WorldPredownload.DownloadManager;
using WorldPredownload.Cache;
using WorldPredownload.Helpers;

namespace WorldPredownload.UI
{
    public class PortalButton
    {
        private static Player LocalPlayer => PlayerManager.field_Private_Static_PlayerManager_0.field_Private_Player_0;

        private static void PreDownloadPortal()
        {
            // Get all portals
            var portals = Resources.FindObjectsOfTypeAll<PortalInternal>();

            // Check if there are any portals
            if (portals.Length == 0)
            {
                Utilities.ShowDismissPopup(
                    "No portals found!",
                    "No portals found in this world to download from.",
                    Constants.ERROR_BTN_TEXT,
                    new Action(delegate
                    {
                        Utilities.HideCurrentPopup();
                    })
                );
                return;
            }

            // Get the nearest portal
            var nearestPortal = portals.OrderBy(p => Vector3.Distance(p.transform.position, LocalPlayer.transform.position)).FirstOrDefault();

            // Get the target world of the portal
            var targetWorld = nearestPortal.field_Private_ApiWorld_0;

            // Check if the world is already downloaded
            if (!WorldDownloadManager.Downloading && !CacheManager.HasDownloadedWorld(targetWorld.assetUrl))
            {
                // Pre-download the world
                WorldDownloadManager.ProcessDownload(
                    DownloadInfo.CreatePortalDownloadInfo(
                        targetWorld,
                        nearestPortal.field_Private_String_4,
                        DownloadType.Portal,
                        nearestPortal
                    )
                );
            }
            else
            {
                Utilities.ShowDismissPopup(
                    "Portal world already downloaded",
                    "You can't pre-download a world that is already downloaded.",
                    Constants.ERROR_BTN_TEXT,
                    new Action(delegate
                    {
                        Utilities.HideCurrentPopup();
                    })
                );
            }
        }

        public static void Setup()
        {
            ExpansionKitApi.GetExpandedMenu(ExpandedMenu.QuickMenu).AddSimpleButton("Download world of closest portal", PreDownloadPortal);
        }
    }
}

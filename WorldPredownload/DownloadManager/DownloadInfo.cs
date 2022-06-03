using Transmtn.DTO.Notifications;
using VRC.Core;
using VRC.UI;

namespace WorldPredownload.DownloadManager
{
    public class DownloadInfo
    {
        public DownloadInfo(
            ApiWorld apiWorld,
            string instanceIDTags,
            DownloadType downloadType,
            PageUserInfo pageUserInfo = null!,
            PageWorldInfo pageWorldInfo = null!,
            Notification notification = null!,
            PortalInternal portal = null!)
        {
            ApiWorld = apiWorld;
            InstanceIDTags = instanceIDTags;
            DownloadType = downloadType;
            PageUserInfo = pageUserInfo;
            if (pageUserInfo != null) APIUser = pageUserInfo.field_Private_APIUser_0;
            PageWorldInfo = pageWorldInfo;
            Notification = notification;
            Portal = portal;
        }

        public ApiWorld ApiWorld { get; set; }
        public string InstanceIDTags { get; set; }
        public DownloadType DownloadType { get; set; }
        public PageUserInfo? PageUserInfo { get; set; }

        public APIUser? APIUser { get; set; }
        public PageWorldInfo? PageWorldInfo { get; set; }
        public Notification? Notification { get; set; }
        public PortalInternal? Portal { get; set; }

        public static DownloadInfo CreateInviteDownloadInfo(
            ApiWorld apiWorld,
            string instanceIDTags,
            DownloadType downloadType,
            Notification notification)
        {
            return new(apiWorld, instanceIDTags, downloadType, null!, null!, notification);
        }

        public static DownloadInfo CreateWorldPageDownloadInfo(
            ApiWorld apiWorld,
            string instanceIDTags,
            DownloadType downloadType,
            PageWorldInfo pageWorldInfo)
        {
            return new(apiWorld, instanceIDTags, downloadType, null!, pageWorldInfo);
        }

        public static DownloadInfo CreateUserPageDownloadInfo(
            ApiWorld apiWorld,
            string instanceIDTags,
            DownloadType downloadType,
            PageUserInfo pageUserInfo)
        {
            return new(apiWorld, instanceIDTags, downloadType, pageUserInfo);
        }

        public static DownloadInfo CreatePortalDownloadInfo(
            ApiWorld apiWorld,
            string instanceIDTags,
            DownloadType downloadType,
            PortalInternal portal)
        {
            return new(apiWorld, instanceIDTags, downloadType, null!, null!, null!, portal);
        }
    }
}

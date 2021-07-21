namespace ECollectionApp.WebUI
{
    public static class Api
    {
        public static class Collection
        {
            public static string CollectionsUrl() => "api/collections";

            public static string CollectionsUrl(int groupId) => "api/collections?groupId=" + groupId;

            public static string CollectionUrl(int id) => $"{CollectionsUrl()}/{id}";
        }

        public static class CollectionGroup
        {
            public static string GroupsUrl() => "api/collection-groups";

            public static string GroupsUrl(int accountId) => $"{GroupsUrl()}?accountId={accountId}";

            public static string GroupsWithTagsUrl() => GroupsUrl() + "?include=tags";

            public static string GroupsWithTagsUrl(int accountId) => GroupsUrl(accountId) + "&include=tags";

            public static string GroupUrl(int id) => $"{GroupsUrl()}/{id}";

            public static string GroupWithTagUrl(int id) => GroupUrl(id) + "?include=tags";
        }

        public static class Tag
        {
            public static string GroupTags(int groupId) => $"api/collection-groups/{groupId}/tags";
        }
    }
}

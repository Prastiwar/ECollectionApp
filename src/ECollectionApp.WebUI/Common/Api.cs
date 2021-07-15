namespace ECollectionApp.WebUI
{
    public static class Api
    {
        public static class Collection
        {
            public static string CollectionsUrl() => "api/collections";

            public static string CollectionsUrl(int groupId) => "api/collections?groupId=" + groupId;

            public static string CollectionUrl(int id) => $"{CollectionsUrl()}/{id}";

            public static string GroupsUrl() => "api/collection-groups";

            public static string GroupsUrl(int accountId) => "api/collection-groups?accountId=" + accountId;

            public static string GroupUrl(int id) => $"{GroupsUrl()}/{id}";
        }
    }
}

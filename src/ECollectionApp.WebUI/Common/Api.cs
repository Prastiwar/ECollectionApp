namespace ECollectionApp.WebUI
{
    public static class Api
    {
        public static class Collection
        {
            public static string GetGroups() => "api/collection-groups";

            public static string GetGroup(int id) => $"{GetGroups()}/{id}";
        }

        public static class Account
        {
            public static string SignIn() => "api/login";

            public static string SignOut() => "api/logout";
        }
    }
}

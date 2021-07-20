using System.Security.Claims;

namespace ECollectionApp
{
    public static class ECollectionAppClaims
    {
        public const string Account_Id = "https://ecollectionapp/account_id";

        public static int GetAccountId(this ClaimsPrincipal principal) => int.Parse(principal.FindFirst(Account_Id).Value);
    }
}

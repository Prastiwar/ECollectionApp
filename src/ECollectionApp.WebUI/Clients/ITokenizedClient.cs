namespace ECollectionApp.WebUI.Clients
{
    public interface ITokenizedClient<TClient> where TClient : ITokenizedClient<TClient>
    {
        TClient WithToken(string token);
    }
}

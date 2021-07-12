using System.Threading.Tasks;

namespace ECollectionApp.WebUI.Clients
{
    public interface IAccountClient
    {
        Task<string> Login();

        Task Logout();
    }
}

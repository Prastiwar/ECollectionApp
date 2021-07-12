using ECollectionApp.WebUI.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECollectionApp.WebUI.Clients
{
    public interface ICollectionClient
    {
        Task<IEnumerable<CollectionGroup>> GetGroupsAsync();
    }
}

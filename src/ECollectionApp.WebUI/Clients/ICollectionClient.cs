using ECollectionApp.WebUI.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECollectionApp.WebUI.Clients
{
    public interface ICollectionClient : ITokenizedClient<ICollectionClient>
    {
        Task<IEnumerable<CollectionGroup>> GetGroupsAsync(int accountId = 0);
        Task<CollectionGroup> GetGroupAsync(int id);
        Task CreateGroupAsync(CollectionGroup group);
        Task UpdateGroupAsync(CollectionGroup group);
        Task DeleteGroupAsync(int id);

        Task<IEnumerable<Collection>> GetCollectionsAsync(int groupId = 0);
        Task<Collection> GetCollectionAsync(int id);
        Task CreateCollectionAsync(Collection collection);
        Task UpdateCollectionAsync(Collection collection);
        Task DeleteCollectionAsync(int id);
    }
}

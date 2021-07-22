using ECollectionApp.WebUI.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECollectionApp.WebUI.Clients
{
    public interface ICollectionClient : ITokenizedClient<ICollectionClient>
    {
        Task<IEnumerable<CollectionGroup>> GetGroupsAsync(bool includeTags = false);
        Task<CollectionGroup> GetGroupAsync(int id, bool includeTags = false);
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

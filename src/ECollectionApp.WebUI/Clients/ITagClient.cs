using ECollectionApp.WebUI.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECollectionApp.WebUI.Clients
{
    public interface ITagClient : ITokenizedClient<ITagClient>
    {
        Task<IEnumerable<Tag>> GetGroupTagsAsync(int groupId);

        Task UpdateGroupTagsAsync(int groupId, IEnumerable<string> tags);
        Task UpdateGroupTagsAsync(int groupId, IEnumerable<Tag> tags);
    }
}

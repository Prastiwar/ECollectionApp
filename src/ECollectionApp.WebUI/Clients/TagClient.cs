using ECollectionApp.AspNetCore.Serialization;
using ECollectionApp.WebUI.Data;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ECollectionApp.WebUI.Clients
{
    public class TagClient : TokenizedEntityClient<ITagClient>, ITagClient
    {
        public TagClient(HttpClient client, IHttpContentSerializationHandler serializer) : base(client, serializer) { }

        protected override ITagClient ReturnClient() => this;

        public Task<IEnumerable<Tag>> GetGroupTagsAsync(int groupId) => GetEntitiesAsync<Tag>(Api.Tag.GroupTags(groupId));

        public Task UpdateGroupTagsAsync(int groupId, IEnumerable<string> tags) => UpdateEntityAsync(tags, Api.Tag.GroupTags(groupId));

        public Task UpdateGroupTagsAsync(int groupId, IEnumerable<Tag> tags) => UpdateEntityAsync(tags, Api.Tag.GroupTags(groupId));
    }
}

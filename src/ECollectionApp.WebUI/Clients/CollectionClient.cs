using ECollectionApp.AspNetCore.Serialization;
using ECollectionApp.WebUI.Data;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ECollectionApp.WebUI.Clients
{
    public class CollectionClient : TokenizedEntityClient<ICollectionClient>, ICollectionClient
    {
        public CollectionClient(HttpClient client, IHttpContentSerializationHandler serializer)
            : base(client, serializer) { }

        protected override ICollectionClient ReturnClient() => this;

        public async Task<IEnumerable<CollectionGroup>> GetGroupsAsync(bool includeTags = false)
        {
            string url = null;
            if (includeTags)
            {
                url = Api.CollectionGroup.GroupsWithTagsUrl();
            }
            else
            {
                url = Api.CollectionGroup.GroupsUrl();
            }
            IEnumerable<CollectionGroup> collectionGroups = await GetEntitiesAsync<CollectionGroup>(url);
            return collectionGroups;
        }

        public Task<CollectionGroup> GetGroupAsync(int id, bool includeTags = false)
        {
            if (includeTags)
            {
                return GetEntityAsync<CollectionGroup>(Api.CollectionGroup.GroupWithTagUrl(id));
            }
            return GetEntityAsync<CollectionGroup>(Api.CollectionGroup.GroupUrl(id));
        }

        public Task CreateGroupAsync(CollectionGroup group) => CreateEntityAsync(group, Api.CollectionGroup.GroupsUrl());

        public Task DeleteGroupAsync(int id) => DeleteEntityAsync(Api.CollectionGroup.GroupUrl(id));

        public Task UpdateGroupAsync(CollectionGroup group) => UpdateEntityAsync(group, Api.CollectionGroup.GroupUrl(group.Id));

        public Task<IEnumerable<Collection>> GetCollectionsAsync(int groupId = 0)
            => GetEntitiesAsync<Collection>(Api.Collection.CollectionsUrl(groupId));

        public Task<Collection> GetCollectionAsync(int id)
            => GetEntityAsync<Collection>(Api.Collection.CollectionUrl(id));

        public Task CreateCollectionAsync(Collection collection) => CreateEntityAsync(collection, Api.Collection.CollectionsUrl());

        public Task DeleteCollectionAsync(int id) => DeleteEntityAsync(Api.Collection.CollectionUrl(id));

        public Task UpdateCollectionAsync(Collection collection) => UpdateEntityAsync(collection, Api.Collection.CollectionUrl(collection.Id));
    }
}

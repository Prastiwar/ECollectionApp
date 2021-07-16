using ECollectionApp.WebUI.Data;
using ECollectionApp.WebUI.Serialization;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ECollectionApp.WebUI.Clients
{
    public class CollectionClient : TokenizedEntityClient<ICollectionClient>, ICollectionClient
    {
        public CollectionClient(HttpClient client, IHttpContentSerializationHandler deserializer)
            : base(client, deserializer) { }

        protected override ICollectionClient ReturnClient() => this;

        public Task<IEnumerable<CollectionGroup>> GetGroupsAsync(int accountId = 0)
            => GetEntitiesAsync<CollectionGroup>(Api.Collection.GroupsUrl(accountId));

        public Task<CollectionGroup> GetGroupAsync(int id)
            => GetEntityAsync<CollectionGroup>(Api.Collection.GroupUrl(id));

        public Task CreateGroupAsync(CollectionGroup group) => CreateEntityAsync(group, Api.Collection.GroupsUrl());

        public Task DeleteGroupAsync(int id) => DeleteEntityAsync(Api.Collection.GroupUrl(id));

        public Task UpdateGroupAsync(CollectionGroup group) => UpdateEntityAsync(group, Api.Collection.GroupUrl(group.Id));

        public Task<IEnumerable<Collection>> GetCollectionsAsync(int groupId = 0)
            => GetEntitiesAsync<Collection>(Api.Collection.CollectionsUrl(groupId));

        public Task<Collection> GetCollectionAsync(int id)
            => GetEntityAsync<Collection>(Api.Collection.CollectionUrl(id));

        public Task CreateCollectionAsync(Collection collection) => CreateEntityAsync(collection, Api.Collection.CollectionsUrl());

        public Task DeleteCollectionAsync(int id) => DeleteEntityAsync(Api.Collection.CollectionUrl(id));

        public Task UpdateCollectionAsync(Collection collection) => UpdateEntityAsync(collection, Api.Collection.CollectionUrl(collection.Id));
    }
}

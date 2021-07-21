using ECollectionApp.AspNetCore.Serialization;
using ECollectionApp.GatewayApi.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Ocelot.Middleware;
using Ocelot.Multiplexer;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ECollectionApp.GatewayApi.Aggregation
{
    public class CollectionGroupsAggregator : IDefinedAggregator
    {
        public CollectionGroupsAggregator(IHttpContentSerializationHandler serializationHandler, IHttpClientFactory httpClientFactory)
        {
            SerializationHandler = serializationHandler;
            Client = httpClientFactory.CreateClient();
        }

        protected IHttpContentSerializationHandler SerializationHandler { get; }

        protected HttpClient Client { get; }

        // TODO: Refactor this endpoint
        public async Task<DownstreamResponse> Aggregate(List<HttpContext> responses)
        {
            DownstreamResponse groupResponse = responses[0].Items.DownstreamResponse();
            HttpContent content = groupResponse.Content;
            if (content.Headers.ContentType == null)
            {
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            }
            IEnumerable<CollectionGroup> groups = null;
            bool shouldInclude = responses[0].Request.Query.TryGetValue("include", out StringValues includeFields);
            bool shouldfilterTags = responses[0].Request.Query.TryGetValue("tags", out StringValues filterTags);
            if (shouldInclude || shouldfilterTags)
            {
                groups = await SerializationHandler.DeserializeAsync<IEnumerable<CollectionGroup>>(content);
            }
            if (shouldInclude)
            {
                Ocelot.Request.Middleware.DownstreamRequest tagRequest = responses[1].Items.DownstreamRequest();
                string urlTemplate = $"{tagRequest.Scheme}://{tagRequest.Host}:{tagRequest.Port}{tagRequest.AbsolutePath}";

                static string FormatUrl(int groupId, string urlTemplate) => urlTemplate.Replace("{id}", groupId.ToString());

                foreach (string field in includeFields)
                {
                    if (string.Compare("tags", field, true) == 0)
                    {
                        foreach (CollectionGroup group in groups)
                        {
                            string url = FormatUrl(group.Id, urlTemplate);
                            Client.DefaultRequestHeaders.Authorization = responses[0].Items.DownstreamRequest().Headers.Authorization;
                            HttpResponseMessage response = await Client.GetAsync(url);
                            if (response.IsSuccessStatusCode)
                            {
                                IEnumerable<Tag> tags = await SerializationHandler.DeserializeAsync<IEnumerable<Tag>>(response.Content);
                                group.Tags = new List<Tag>(tags);
                            }
                            else
                            {
                                return new DownstreamResponse(new StringContent("[]"), response.StatusCode, response.Headers, response.ReasonPhrase);
                            }
                        }
                    }
                }
            }
            if (shouldfilterTags)
            {
                foreach (string tag in filterTags)
                {
                    groups = groups.Where(group => group.Tags.Any(t => string.Compare(t.Name, tag, true) == 0));
                }
            }
            if (groups != null)
            {
                content = await SerializationHandler.SerializeAsync(groups, groups.GetType(), content.Headers.ContentType);
            }
            return new DownstreamResponse(content, groupResponse.StatusCode, groupResponse.Headers, groupResponse.ReasonPhrase);
        }
    }
}

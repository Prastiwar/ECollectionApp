using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using Ocelot.Middleware;
using Ocelot.Multiplexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ECollectionApp.GatewayApi.Aggregation
{
    public class CollectionGroupsAggregator : IDefinedAggregator
    {
        public async Task<DownstreamResponse> Aggregate(List<HttpContext> responses)
        {
            Func<HttpStatusCode, bool> IsSuccessStatusCode = new Func<HttpStatusCode, bool>(statusCode => (int)statusCode >= 200 && (int)statusCode <= 299);
            HttpContext invalidResponse = responses.FirstOrDefault(r => !IsSuccessStatusCode.Invoke(r.Items.DownstreamResponse().StatusCode));
            if (invalidResponse != null)
            {
                DownstreamResponse downstreamResponse = invalidResponse.Items.DownstreamResponse();
                return downstreamResponse;
            }
            string collectionGroupKey = "CollectionGroups";
            string collectionGroupTagKey = "CollectionGroupsTags";
            HttpContext collectionGroupResponse = responses.FirstOrDefault(r => EqualityComparer<string>.Default.Equals(r.Items.DownstreamRoute().Key, collectionGroupKey));
            HttpContext collectionGroupTagResponse = responses.FirstOrDefault(r => EqualityComparer<string>.Default.Equals(r.Items.DownstreamRoute().Key, collectionGroupTagKey));
            if (collectionGroupResponse == null || collectionGroupTagResponse == null)
            {
                throw new InvalidProgramException($"{nameof(CollectionGroupsAggregator)} should operate on two responses: '{collectionGroupKey}' and '{collectionGroupTagKey}'");
            }

            HttpContent collectionGroupContent = collectionGroupResponse.Items.DownstreamResponse().Content;
            HttpContent collectionGroupTagContent = collectionGroupTagResponse.Items.DownstreamResponse().Content;

            // We always assume results will be application/json
            JArray collectionGroupJObject = JArray.Parse(await collectionGroupContent.ReadAsStringAsync());
            JArray collectionGroupTag = JArray.Parse(await collectionGroupTagContent.ReadAsStringAsync());
            for (int i = 0; i < collectionGroupJObject.Count; i++)
            {
                JObject obj = (JObject)collectionGroupJObject[i];
                IEnumerable<JToken> tokens = collectionGroupTag.Where(token => ((JObject)token).Property("groupId", StringComparison.OrdinalIgnoreCase).Value.ToObject<int>() == 
                                                                                            obj.Property("id", StringComparison.OrdinalIgnoreCase).Value.ToObject<int>());
                obj.Add("tags", new JArray(tokens));
            }

            List<Header> headers = responses.SelectMany(x => x.Items.DownstreamResponse().Headers).ToList();
            return new DownstreamResponse(new StringContent(collectionGroupJObject.ToString()), HttpStatusCode.OK, headers, "");
        }
    }
}

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
    public class CollectionGroupAggregator : IDefinedAggregator
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
            string collectionGroupKey = "CollectionGroup";
            string collectionGroupTagKey = "CollectionGroupTags";
            HttpContext collectionGroupResponse = responses.FirstOrDefault(r => EqualityComparer<string>.Default.Equals(r.Items.DownstreamRoute().Key, collectionGroupKey));
            HttpContext collectionGroupTagResponse = responses.FirstOrDefault(r => EqualityComparer<string>.Default.Equals(r.Items.DownstreamRoute().Key, collectionGroupTagKey));
            if (collectionGroupResponse == null || collectionGroupTagResponse == null)
            {
                throw new InvalidProgramException($"{nameof(CollectionGroupAggregator)} should operate on two responses: '{collectionGroupKey}' and '{collectionGroupTagKey}'");
            }

            HttpContent collectionGroupContent = collectionGroupResponse.Items.DownstreamResponse().Content;
            HttpContent collectionGroupTagContent = collectionGroupTagResponse.Items.DownstreamResponse().Content;

            // We always assume results will be application/json
            JObject collectionGroupJObject = JObject.Parse(await collectionGroupContent.ReadAsStringAsync());
            JArray collectionGroupTag = JArray.Parse(await collectionGroupTagContent.ReadAsStringAsync());
            collectionGroupJObject.Add("tags", collectionGroupTag);

            List<Header> headers = responses.SelectMany(x => x.Items.DownstreamResponse().Headers).ToList();
            return new DownstreamResponse(new StringContent(collectionGroupJObject.ToString()), HttpStatusCode.OK, headers, "");
        }
    }
}

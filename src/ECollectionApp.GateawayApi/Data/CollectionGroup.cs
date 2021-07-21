using System.Collections.Generic;

namespace ECollectionApp.GatewayApi.Data
{
    public class CollectionGroup
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        public string Name { get; set; }

        public IList<Tag> Tags { get; set; }
    }
}

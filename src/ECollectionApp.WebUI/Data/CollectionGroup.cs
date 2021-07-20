using System.Collections.Generic;

namespace ECollectionApp.WebUI.Data
{
    public class CollectionGroup
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        public string Name { get; set; }

        public IList<CollectionGroupTag> Tags { get; set; }

        public IList<Collection> Collections { get; set; }
    }
}

using ECollectionApp.WebUI.Data;
using System.Collections.Generic;

namespace ECollectionApp.WebUI.ViewModels
{
    public class CollectionGroupViewModel
    {
        public CollectionGroup Group { get; set; }

        public IEnumerable<Collection> Collections { get; set; }

        public string Tags { get; set; }
    }
}

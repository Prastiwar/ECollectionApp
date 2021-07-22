using ECollectionApp.WebUI.Data;
using System.Collections.Generic;

namespace ECollectionApp.WebUI.ViewModels
{
    public class CollectionMoveViewModel
    {
        public Collection Collection { get; set; }

        public IEnumerable<CollectionGroup> Groups { get; set; }

        public bool ShowGroup { get; set; }
    }
}

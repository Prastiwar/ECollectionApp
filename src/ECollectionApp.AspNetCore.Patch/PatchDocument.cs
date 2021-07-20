using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ECollectionApp.AspNetCore.Patch
{
    public class PatchDocument : IEnumerable<KeyValuePair<string, object>>
    {
        public PatchDocument(IList<KeyValuePair<string, object>> changes) => Changes = new ReadOnlyCollection<KeyValuePair<string, object>>(changes);

        public IReadOnlyCollection<KeyValuePair<string, object>> Changes { get; }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator() => Changes.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => Changes.GetEnumerator();
    }
}

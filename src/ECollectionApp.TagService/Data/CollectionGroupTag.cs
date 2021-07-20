using System;
using System.Collections.Generic;

namespace ECollectionApp.TagService.Data
{
    public class CollectionGroupTag : IEquatable<CollectionGroupTag>
    {
        public int TagId { get; set; }

        public int GroupId { get; set; }

        public override bool Equals(object obj) => Equals(obj as CollectionGroupTag);
        public bool Equals(CollectionGroupTag other) => other != null && TagId == other.TagId && GroupId == other.GroupId;
        public override int GetHashCode() => HashCode.Combine(TagId, GroupId);

        public static bool operator ==(CollectionGroupTag left, CollectionGroupTag right) => EqualityComparer<CollectionGroupTag>.Default.Equals(left, right);
        public static bool operator !=(CollectionGroupTag left, CollectionGroupTag right) => !(left == right);
    }
}

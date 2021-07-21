using System;
using System.Collections.Generic;

namespace ECollectionApp.WebUI.Data
{
    public class Tag : IEquatable<Tag>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public override bool Equals(object obj) => Equals(obj as Tag);
        public bool Equals(Tag other) => other != null && Id == other.Id && Name == other.Name;
        public override int GetHashCode() => HashCode.Combine(Id, Name);

        public static bool operator ==(Tag left, Tag right) => EqualityComparer<Tag>.Default.Equals(left, right);
        public static bool operator !=(Tag left, Tag right) => !(left == right);

        public override string ToString() => Name;
    }
}

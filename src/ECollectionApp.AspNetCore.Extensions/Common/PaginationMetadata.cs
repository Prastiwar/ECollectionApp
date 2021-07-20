using System;
using System.Collections.Generic;

namespace ECollectionApp.AspNetCore.Microservice
{
    public class PaginationMetadata : IEquatable<PaginationMetadata>
    {
        public static PaginationMetadata Empty { get; }

        static PaginationMetadata() => Empty = new PaginationMetadata();

        public int Page { get; set; }
        public int Limit { get; set; }

        public override bool Equals(object obj) => Equals(obj as PaginationMetadata);
        public bool Equals(PaginationMetadata other) => other != null && Page == other.Page && Limit == other.Limit;

        public override int GetHashCode()
        {
            int hashCode = -30036211;
            hashCode = hashCode * -1521134295 + Page.GetHashCode();
            hashCode = hashCode * -1521134295 + Limit.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(PaginationMetadata left, PaginationMetadata right) => EqualityComparer<PaginationMetadata>.Default.Equals(left, right);
        public static bool operator !=(PaginationMetadata left, PaginationMetadata right) => !(left == right);
    }
}

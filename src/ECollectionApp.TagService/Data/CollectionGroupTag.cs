namespace ECollectionApp.TagService.Data
{
    public class CollectionGroupTag
    {
        public int Id { get; set; }

        public int GroupId { get; set; }

        public string Name { get; set; }

        public override string ToString() => Name;
    }
}

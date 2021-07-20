namespace ECollectionApp.TagService.Data
{
    public class Tag
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public override string ToString() => Name;
    }
}

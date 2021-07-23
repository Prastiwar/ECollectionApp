namespace ECollectionApp.CollectionGroupService.Messaging
{
    public class CollectionGroupDeletedEvent
    {
        public CollectionGroupDeletedEvent(int groupId) => GroupId = groupId;

        public int GroupId { get; }
    }
}

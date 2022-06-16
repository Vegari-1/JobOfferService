namespace JobOfferService.Service.Messaging
{
    public static class TopicEvent
    {
        public static readonly string CREATED_EVENT = "created";
        public static readonly string UPDATED_EVENT = "updated";
        public static readonly string DELETED_EVENT = "deleted";
    }
}

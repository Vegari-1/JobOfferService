namespace JobOfferService.Dto
{
    public class JobOfferResponse
    {
        public string Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public string PositionName { get; set; }

        public string Description { get; set; }
    }
}


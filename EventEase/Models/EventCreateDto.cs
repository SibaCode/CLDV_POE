namespace EventEase.Models
{
    public class EventCreateDto
    {
        public string EventName { get; set; }
        public DateTime EventDate { get; set; }
        public int VenueId { get; set; }
        public string Description { get; set; }
    }
}

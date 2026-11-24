namespace SignalR.DtoLayer.Dtos.NotificationDtos
{
    public class UpdateNotificationDto
    {
        public int NotificationId { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public DateTime AtDate { get; set; }
        public bool Status { get; set; }
    }
}

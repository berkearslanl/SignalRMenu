namespace SignalR.DtoLayer.Dtos.MailDtos
{
    public class CreateMailDto
    {
        public string ReceiveMail { get; set; }
        public string ReceiveName { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
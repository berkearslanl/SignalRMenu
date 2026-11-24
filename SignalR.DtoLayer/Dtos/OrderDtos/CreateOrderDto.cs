namespace SignalR.DtoLayer.Dtos.OrderDtos
{
    public class CreateOrderDto
    {
        public int RestaurantTableId { get; set; }
        public string Description { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
    }
}

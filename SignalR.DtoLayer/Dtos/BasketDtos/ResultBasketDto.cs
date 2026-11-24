namespace SignalR.DtoLayer.Dtos.BasketDtos
{
    public class ResultBasketDto
    {
        public int BasketId { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
        public decimal TotalPrice { get; set; }
        public int RestaurantTableId { get; set; }
    }
}
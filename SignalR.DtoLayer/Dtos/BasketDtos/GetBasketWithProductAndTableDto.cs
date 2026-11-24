namespace SignalR.DtoLayer.Dtos.BasketDtos
{
    public class GetBasketWithProductAndTableDto
    {
        public int BasketId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Count { get; set; }
        public decimal TotalPrice { get; set; }
        public int RestaurantTableId { get; set; }
        public string TableName { get; set; }
    }
}
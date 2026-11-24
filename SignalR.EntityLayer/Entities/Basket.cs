namespace SignalR.EntityLayer.Entities
{
    public class Basket
    {
        public int BasketId { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Count { get; set; }
        public decimal TotalPrice { get; set; }
        public int RestaurantTableId { get; set; }
        public RestaurantTable RestaurantTable { get; set; }
    }
}
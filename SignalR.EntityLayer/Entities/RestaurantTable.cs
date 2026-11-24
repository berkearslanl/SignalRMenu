using SignalR.EntityLayer.Enums;

namespace SignalR.EntityLayer.Entities
{
    public class RestaurantTable
    {
        public int RestaurantTableId { get; set; }
        public string TableName { get; set; }
        public TableStatus Status { get; set; }

        public List<Order> Orders { get; set; }
        public List<Basket> Baskets { get; set; }
    }
}
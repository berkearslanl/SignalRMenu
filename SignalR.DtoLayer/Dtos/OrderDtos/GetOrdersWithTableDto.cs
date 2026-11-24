namespace SignalR.DtoLayer.Dtos.OrderDtos
{
    public class GetOrdersWithTableDto
    {
        public int OrderID { get; set; }
        public int RestaurantTableId { get; set; }
        public string TableName { get; set; }
        public string Description { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
    }
}

namespace SignalR.DtoLayer.Dtos.RestaurantTableDtos
{
    public class UpdateRestaurantTableDto
    {
        public int RestaurantTableId { get; set; }
        public string TableName { get; set; }
        public int Status { get; set; }
    }
}

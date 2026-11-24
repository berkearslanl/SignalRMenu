using SignalR.EntityLayer.Entities;

namespace SignalR.BusinessLayer.Abstract
{
    public interface IRestaurantTableService : IGenericService<RestaurantTable>
    {
        Task<int> TGetTableCount();
        Task<int> TGetActiveTableCount();
    }
}

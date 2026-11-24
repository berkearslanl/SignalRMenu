using SignalR.EntityLayer.Entities;

namespace SignalR.DataAccessLayer.Abstract
{
    public interface IRestaurantTableDal : IGenericDal<RestaurantTable>
    {
        Task<int> GetTableCount();
        Task<int> GetActiveTableCount();
    }
}
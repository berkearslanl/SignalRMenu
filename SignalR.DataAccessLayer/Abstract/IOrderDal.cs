using SignalR.EntityLayer.Entities;

namespace SignalR.DataAccessLayer.Abstract
{
    public interface IOrderDal : IGenericDal<Order>
    {
        Task<int> GetTotalOrderCount();
        Task<string?> GetTableNumberByMaxOrder();
        Task<decimal> GetLastOrderTotalPrice();
        Task<List<Order>> GetAllOrdersWithTableAsync();
        Task<decimal> GetTodayTotalPrice();
    }
}
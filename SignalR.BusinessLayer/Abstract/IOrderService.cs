using SignalR.EntityLayer.Entities;

namespace SignalR.BusinessLayer.Abstract
{
    public interface IOrderService : IGenericService<Order>
    {
        Task<int> TGetTotalOrderCount();
        Task<string?> TGetTableNumberByMaxOrder();
        Task<decimal> TGetLastOrderTotalPrice();
        Task<List<Order>> TGetAllOrdersWithTableAsync();
        Task<decimal> TGetTodayTotalPrice();
    }
}
using SignalR.BusinessLayer.Abstract;
using SignalR.DataAccessLayer.Abstract;
using SignalR.EntityLayer.Entities;
using System.Linq.Expressions;

namespace SignalR.BusinessLayer.Concrete
{
    public class OrderManager : IOrderService
    {
        private readonly IOrderDal _orderDal;

        public OrderManager(IOrderDal orderDal)
        {
            _orderDal = orderDal;
        }

        public async Task<List<Order>> TGetAllOrdersWithTableAsync()
        {
            return await _orderDal.GetAllOrdersWithTableAsync();
        }

        public async Task TCreateAsync(Order entity)
        {
            await _orderDal.CreateAsync(entity);
        }

        public async Task TDeleteAsync(int id)
        {
            await _orderDal.DeleteAsync(id);
        }

        public async Task<List<Order>> TGetAllAsync()
        {
            return await _orderDal.GetAllAsync();
        }

        public async Task<List<Order>> TGetAllFilterAsync(Expression<Func<Order, bool>> filter)
        {
            return await _orderDal.GetAllFilterAsync(filter);
        }

        public async Task<Order?> TGetByIdAsync(int id)
        {
            return await _orderDal.GetByIdAsync(id);
        }

        public async Task<decimal> TGetLastOrderTotalPrice()
        {
            return await _orderDal.GetLastOrderTotalPrice();
        }

        public async Task<string?> TGetTableNumberByMaxOrder()
        {
            return await _orderDal.GetTableNumberByMaxOrder();
        }

        public async Task<decimal> TGetTodayTotalPrice()
        {
            return await _orderDal.GetTodayTotalPrice();
        }

        public async Task<int> TGetTotalOrderCount()
        {
            return await _orderDal.GetTotalOrderCount();
        }

        public async Task TUpdateAsync(Order entity)
        {
            await _orderDal.UpdateAsync(entity);
        }
    }
}
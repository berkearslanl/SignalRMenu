using SignalR.BusinessLayer.Abstract;
using SignalR.DataAccessLayer.Abstract;
using SignalR.EntityLayer.Entities;
using System.Linq.Expressions;

namespace SignalR.BusinessLayer.Concrete
{
    public class OrderDetailManager : IOrderDetailService
    {
        private readonly IOrderDetailDal _orderDetailDal;

        public OrderDetailManager(IOrderDetailDal orderDetailDal)
        {
            _orderDetailDal = orderDetailDal;
        }

        public async Task TCreateAsync(OrderDetail entity)
        {
            await _orderDetailDal.CreateAsync(entity);
        }

        public async Task TDeleteAsync(int id)
        {
            await _orderDetailDal.DeleteAsync(id);
        }

        public async Task<List<OrderDetail>> TGetAllAsync()
        {
            return await _orderDetailDal.GetAllAsync();
        }

        public async Task<List<OrderDetail>> TGetAllFilterAsync(Expression<Func<OrderDetail, bool>> filter)
        {
            return await _orderDetailDal.GetAllFilterAsync(filter);
        }

        public async Task<OrderDetail?> TGetByIdAsync(int id)
        {
            return await _orderDetailDal.GetByIdAsync(id);
        }

        public async Task TUpdateAsync(OrderDetail entity)
        {
            await _orderDetailDal.UpdateAsync(entity);
        }
    }
}
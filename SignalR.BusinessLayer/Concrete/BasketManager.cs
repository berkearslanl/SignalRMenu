using SignalR.BusinessLayer.Abstract;
using SignalR.DataAccessLayer.Abstract;
using SignalR.EntityLayer.Entities;
using System.Linq.Expressions;

namespace SignalR.BusinessLayer.Concrete
{
    public class BasketManager : IBasketService
    {
        private readonly IBasketDal _basketDal;

        public BasketManager(IBasketDal basketDal)
        {
            _basketDal = basketDal;
        }

        public async Task TCreateAsync(Basket entity)
        {
            await _basketDal.CreateAsync(entity);
        }

        public async Task TDeleteAsync(int id)
        {
            await _basketDal.DeleteAsync(id);
        }

        public async Task TDeleteBasketByTableId(int id)
        {
            await _basketDal.DeleteBasketByTableId(id);
        }

        public async Task<List<Basket>> TGetAllAsync()
        {
            return await _basketDal.GetAllAsync();
        }

        public async Task<List<Basket>> TGetAllFilterAsync(Expression<Func<Basket, bool>> filter)
        {
            return await _basketDal.GetAllFilterAsync(filter);
        }

        public async Task<List<Basket>> TGetBasketByTable(int id)
        {
            return await _basketDal.GetBasketByTable(id);
        }

        public async Task<int> TGetBasketCountByCurrentTable(int id)
        {
            return await _basketDal.GetBasketCountByCurrentTable(id);
        }

        public async Task<Basket?> TGetByIdAsync(int id)
        {
            return await _basketDal.GetByIdAsync(id);
        }

        public async Task TUpdateAsync(Basket entity)
        {
            await _basketDal.UpdateAsync(entity);
        }
    }
}

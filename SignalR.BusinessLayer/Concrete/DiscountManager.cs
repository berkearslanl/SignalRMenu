using SignalR.BusinessLayer.Abstract;
using SignalR.DataAccessLayer.Abstract;
using SignalR.EntityLayer.Entities;
using System.Linq.Expressions;

namespace SignalR.BusinessLayer.Concrete
{
    public class DiscountManager : IDiscountService
    {
        private readonly IDiscountDal _discountDal;

        public DiscountManager(IDiscountDal DiscountDal)
        {
            _discountDal = DiscountDal;
        }

        public async Task TCreateAsync(Discount entity)
        {
            await _discountDal.CreateAsync(entity);
        }

        public async Task TDeleteAsync(int id)
        {
            await _discountDal.DeleteAsync(id);
        }

        public async Task<List<Discount>> TGetAllAsync()
        {
            return await _discountDal.GetAllAsync();
        }

        public async Task<List<Discount>> TGetAllFilterAsync(Expression<Func<Discount, bool>> filter)
        {
            return await _discountDal.GetAllFilterAsync(filter);
        }

        public async Task<Discount?> TGetByIdAsync(int id)
        {
            return await _discountDal.GetByIdAsync(id);
        }

        public async Task<int> TGetDiscountCount()
        {
            return await _discountDal.GetDiscountCount();
        }

        public async Task<List<Discount>> TGetLast2DiscountAsync()
        {
            return await _discountDal.GetLast2DiscountAsync();
        }

        public async Task TUpdateAsync(Discount entity)
        {
            await _discountDal.UpdateAsync(entity);
        }
    }
}

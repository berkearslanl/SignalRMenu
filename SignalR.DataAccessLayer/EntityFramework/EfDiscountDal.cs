using Microsoft.EntityFrameworkCore;
using SignalR.DataAccessLayer.Abstract;
using SignalR.DataAccessLayer.Concrete;
using SignalR.DataAccessLayer.Repositories;
using SignalR.EntityLayer.Entities;

namespace SignalR.DataAccessLayer.EntityFramework
{
    public class EfDiscountDal : GenericRepository<Discount>, IDiscountDal
    {
        public EfDiscountDal(SignalrContext context) : base(context)
        {
        }

        public async Task<int> GetDiscountCount()
        {
            return await _context.Discounts.CountAsync();
        }

        public async Task<List<Discount>> GetLast2DiscountAsync()
        {
            return await _context.Discounts.OrderByDescending(x => x.Amount).Take(2).ToListAsync();
        }
    }
}
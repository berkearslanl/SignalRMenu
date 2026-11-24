using Microsoft.EntityFrameworkCore;
using SignalR.DataAccessLayer.Abstract;
using SignalR.DataAccessLayer.Concrete;
using SignalR.DataAccessLayer.Repositories;
using SignalR.EntityLayer.Entities;

namespace SignalR.DataAccessLayer.EntityFramework
{
    public class EfBasketDal : GenericRepository<Basket>, IBasketDal
    {
        public EfBasketDal(SignalrContext context) : base(context)
        {
        }

        public async Task DeleteBasketByTableId(int id)
        {
            await _context.Baskets.Where(x => x.RestaurantTableId == id).ExecuteDeleteAsync();
        }

        public async Task<List<Basket>> GetBasketByTable(int id)
        {
            return await _context.Baskets.Include(x => x.RestaurantTable).Include(t => t.Product).Where(x => x.RestaurantTableId == id).ToListAsync();
        }

        public async Task<int> GetBasketCountByCurrentTable(int id)
        {
            return await _context.Baskets.Where(x => x.RestaurantTableId == id).SumAsync(t => t.Count);
        }
    }
}
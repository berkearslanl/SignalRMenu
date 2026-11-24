using Microsoft.EntityFrameworkCore;
using SignalR.DataAccessLayer.Abstract;
using SignalR.DataAccessLayer.Concrete;
using SignalR.DataAccessLayer.Repositories;
using SignalR.EntityLayer.Entities;

namespace SignalR.DataAccessLayer.EntityFramework
{
    public class EfOrderDal : GenericRepository<Order>, IOrderDal
    {
        public EfOrderDal(SignalrContext context) : base(context)
        {
        }

        public async Task<List<Order>> GetAllOrdersWithTableAsync()
        {
            return await _context.Orders.Include(x => x.RestaurantTable).ToListAsync();
        }

        public async Task<decimal> GetLastOrderTotalPrice()
        {
            return await _context.Orders.OrderByDescending(x => x.OrderID).Select(t => t.TotalPrice).FirstOrDefaultAsync();
        }

        public async Task<string?> GetTableNumberByMaxOrder()
        {
            return await _context.Orders.GroupBy(x => x.RestaurantTable.TableName).OrderByDescending(g => g.Count()).Select(g => g.Key).FirstOrDefaultAsync();
        }

        public async Task<decimal> GetTodayTotalPrice()
        {
            var start = DateTime.Today;
            var end = start.AddDays(1);
            return await _context.Orders.Where(x => x.OrderDate >= start && x.OrderDate < end).SumAsync(t => t.TotalPrice);
        }

        public async Task<int> GetTotalOrderCount()
        {
            return await _context.Orders.CountAsync();
        }
    }
}
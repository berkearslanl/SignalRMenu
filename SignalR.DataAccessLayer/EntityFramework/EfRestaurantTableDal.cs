using Microsoft.EntityFrameworkCore;
using SignalR.DataAccessLayer.Abstract;
using SignalR.DataAccessLayer.Concrete;
using SignalR.DataAccessLayer.Repositories;
using SignalR.EntityLayer.Entities;
using SignalR.EntityLayer.Enums;

namespace SignalR.DataAccessLayer.EntityFramework
{
    public class EfRestaurantTableDal : GenericRepository<RestaurantTable>, IRestaurantTableDal
    {
        public EfRestaurantTableDal(SignalrContext context) : base(context)
        {
        }


        public async Task<int> GetActiveTableCount()
        {
            return await _context.RestaurantTables.Where(x => x.Status == 0).CountAsync();
        }

        public async Task<int> GetTableCount()
        {
            return await _context.RestaurantTables.CountAsync();
        }
    }
}
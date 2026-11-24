using Microsoft.EntityFrameworkCore;
using SignalR.DataAccessLayer.Abstract;
using SignalR.DataAccessLayer.Concrete;
using SignalR.DataAccessLayer.Repositories;
using SignalR.EntityLayer.Entities;

namespace SignalR.DataAccessLayer.EntityFramework
{
    public class EfBookingDal : GenericRepository<Booking>, IBookingDal
    {
        public EfBookingDal(SignalrContext context) : base(context)
        {
        }

        public async Task<int> ActiveBookingCountAsync()
        {
            return await _context.Bookings
        .Where(x => x.BookDate >= DateTime.Now)
        .CountAsync();

        }

        public async Task<int> GetBookingCountAsync()
        {
            return await _context.Bookings.CountAsync();
        }
    }
}
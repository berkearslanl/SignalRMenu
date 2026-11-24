using SignalR.BusinessLayer.Abstract;
using SignalR.DataAccessLayer.Abstract;
using SignalR.EntityLayer.Entities;
using System.Linq.Expressions;

namespace SignalR.BusinessLayer.Concrete
{
    public class BookingManager : IBookingService
    {
        private readonly IBookingDal _bookingDal;

        public BookingManager(IBookingDal BookingDal)
        {
            _bookingDal = BookingDal;
        }

        public async Task<int> TActiveBookingCountAsync()
        {
            return await _bookingDal.ActiveBookingCountAsync();
        }

        public async Task TCreateAsync(Booking entity)
        {
            await _bookingDal.CreateAsync(entity);
        }

        public async Task TDeleteAsync(int id)
        {
            await _bookingDal.DeleteAsync(id);
        }

        public async Task<List<Booking>> TGetAllAsync()
        {
            return await _bookingDal.GetAllAsync();
        }

        public async Task<List<Booking>> TGetAllFilterAsync(Expression<Func<Booking, bool>> filter)
        {
            return await _bookingDal.GetAllFilterAsync(filter);
        }

        public async Task<int> TGetBookingCountAsync()
        {
            return await _bookingDal.GetBookingCountAsync();
        }

        public async Task<Booking?> TGetByIdAsync(int id)
        {
            return await _bookingDal.GetByIdAsync(id);
        }

        public async Task TUpdateAsync(Booking entity)
        {
            await _bookingDal.UpdateAsync(entity);
        }
    }
}

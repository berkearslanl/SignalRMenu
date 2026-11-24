using SignalR.EntityLayer.Entities;

namespace SignalR.DataAccessLayer.Abstract
{
    public interface IBookingDal : IGenericDal<Booking>
    {
        Task<int> GetBookingCountAsync();
        Task<int> ActiveBookingCountAsync();
    }
}
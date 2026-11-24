using SignalR.EntityLayer.Entities;

namespace SignalR.BusinessLayer.Abstract
{
    public interface IBookingService : IGenericService<Booking>
    {
        Task<int> TGetBookingCountAsync();
        Task<int> TActiveBookingCountAsync();
    }
}
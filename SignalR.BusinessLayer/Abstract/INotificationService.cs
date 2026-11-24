using SignalR.EntityLayer.Entities;

namespace SignalR.BusinessLayer.Abstract
{
    public interface INotificationService : IGenericService<Notification>
    {
        Task<int> TGetNotificationCountByStatusFalse();
        Task TChangeNotificationStatus(int id);
    }
}
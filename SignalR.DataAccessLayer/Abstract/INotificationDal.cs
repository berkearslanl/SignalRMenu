using SignalR.EntityLayer.Entities;

namespace SignalR.DataAccessLayer.Abstract
{
    public interface INotificationDal : IGenericDal<Notification>
    {
        Task<int> GetNotificationCountByStatusFalse();
        Task ChangeNotificationStatus(int id);
    }
}
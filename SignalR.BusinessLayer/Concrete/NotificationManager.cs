using SignalR.BusinessLayer.Abstract;
using SignalR.DataAccessLayer.Abstract;
using SignalR.EntityLayer.Entities;
using System.Linq.Expressions;

namespace SignalR.BusinessLayer.Concrete
{
    public class NotificationManager : INotificationService
    {
        private readonly INotificationDal _notificationDal;

        public NotificationManager(INotificationDal notificationDal)
        {
            _notificationDal = notificationDal;
        }

        public async Task TChangeNotificationStatus(int id)
        {
            await _notificationDal.ChangeNotificationStatus(id);
        }

        public async Task TCreateAsync(Notification entity)
        {
            await _notificationDal.CreateAsync(entity);
        }

        public async Task TDeleteAsync(int id)
        {
            await _notificationDal.DeleteAsync(id);
        }

        public async Task<List<Notification>> TGetAllAsync()
        {
            return await _notificationDal.GetAllAsync();
        }

        public async Task<List<Notification>> TGetAllFilterAsync(Expression<Func<Notification, bool>> filter)
        {
            return await _notificationDal.GetAllFilterAsync(filter);
        }

        public async Task<Notification?> TGetByIdAsync(int id)
        {
            return await _notificationDal.GetByIdAsync(id);
        }

        public async Task<int> TGetNotificationCountByStatusFalse()
        {
            return await _notificationDal.GetNotificationCountByStatusFalse();
        }

        public async Task TUpdateAsync(Notification entity)
        {
            await _notificationDal.UpdateAsync(entity);
        }
    }
}
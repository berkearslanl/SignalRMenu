using Microsoft.EntityFrameworkCore;
using SignalR.DataAccessLayer.Abstract;
using SignalR.DataAccessLayer.Concrete;
using SignalR.DataAccessLayer.Repositories;
using SignalR.EntityLayer.Entities;

namespace SignalR.DataAccessLayer.EntityFramework
{
    public class EfNotificationDal : GenericRepository<Notification>, INotificationDal
    {
        public EfNotificationDal(SignalrContext context) : base(context)
        {
        }

        public async Task ChangeNotificationStatus(int id)
        {
            var notification = await _context.Notifications.Where(x => x.NotificationId == id).FirstOrDefaultAsync();
            notification.Status = !notification.Status;
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetNotificationCountByStatusFalse()
        {
            return await _context.Notifications.Where(x => x.Status == false).CountAsync();
        }
    }
}
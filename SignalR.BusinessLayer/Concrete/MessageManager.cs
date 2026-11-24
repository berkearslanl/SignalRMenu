using SignalR.BusinessLayer.Abstract;
using SignalR.DataAccessLayer.Abstract;
using SignalR.EntityLayer.Entities;
using System.Linq.Expressions;

namespace SignalR.BusinessLayer.Concrete
{
    public class MessageManager : IMessageService
    {
        private readonly IMessageDal _messageDal;

        public MessageManager(IMessageDal messageDal)
        {
            _messageDal = messageDal;
        }

        public async Task TCreateAsync(Message entity)
        {
            await _messageDal.CreateAsync(entity);
        }

        public async Task TDeleteAsync(int id)
        {
            await _messageDal.DeleteAsync(id);
        }

        public async Task<List<Message>> TGetAllAsync()
        {
            return await _messageDal.GetAllAsync();
        }

        public async Task<List<Message>> TGetAllFilterAsync(Expression<Func<Message, bool>> filter)
        {
            return await _messageDal.GetAllFilterAsync(filter);
        }

        public async Task<Message?> TGetByIdAsync(int id)
        {
            return await _messageDal.GetByIdAsync(id);
        }

        public async Task TUpdateAsync(Message entity)
        {
            await _messageDal.UpdateAsync(entity);
        }
    }
}
using SignalR.BusinessLayer.Abstract;
using SignalR.DataAccessLayer.Abstract;
using SignalR.EntityLayer.Entities;
using System.Linq.Expressions;

namespace SignalR.BusinessLayer.Concrete
{
    public class ContactManager : IContactService
    {
        private readonly IContactDal _contactDal;

        public ContactManager(IContactDal ContactDal)
        {
            _contactDal = ContactDal;
        }

        public async Task TCreateAsync(Contact entity)
        {
            await _contactDal.CreateAsync(entity);
        }

        public async Task TDeleteAsync(int id)
        {
            await _contactDal.DeleteAsync(id);
        }

        public async Task<List<Contact>> TGetAllAsync()
        {
            return await _contactDal.GetAllAsync();
        }

        public async Task<List<Contact>> TGetAllFilterAsync(Expression<Func<Contact, bool>> filter)
        {
            return await _contactDal.GetAllFilterAsync(filter);
        }

        public async Task<Contact?> TGetByIdAsync(int id)
        {
            return await _contactDal.GetByIdAsync(id);
        }

        public async Task TUpdateAsync(Contact entity)
        {
            await _contactDal.UpdateAsync(entity);
        }
    }
}
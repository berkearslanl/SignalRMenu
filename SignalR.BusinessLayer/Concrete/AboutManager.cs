using SignalR.BusinessLayer.Abstract;
using SignalR.DataAccessLayer.Abstract;
using SignalR.EntityLayer.Entities;
using System.Linq.Expressions;

namespace SignalR.BusinessLayer.Concrete
{
    public class AboutManager : IAboutService
    {
        private readonly IAboutDal _aboutDal;

        public AboutManager(IAboutDal aboutDal)
        {
            _aboutDal = aboutDal;
        }

        public async Task TCreateAsync(About entity)
        {
            await _aboutDal.CreateAsync(entity);
        }

        public async Task TDeleteAsync(int id)
        {
            await _aboutDal.DeleteAsync(id);
        }

        public async Task<List<About>> TGetAllAsync()
        {
            return await _aboutDal.GetAllAsync();
        }

        public async Task<List<About>> TGetAllFilterAsync(Expression<Func<About, bool>> filter)
        {
            return await _aboutDal.GetAllFilterAsync(filter);
        }

        public async Task<About?> TGetByIdAsync(int id)
        {
            return await _aboutDal.GetByIdAsync(id);
        }

        public async Task TUpdateAsync(About entity)
        {
            await _aboutDal.UpdateAsync(entity);
        }
    }
}
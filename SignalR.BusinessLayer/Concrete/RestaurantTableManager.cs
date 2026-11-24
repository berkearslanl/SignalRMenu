using SignalR.BusinessLayer.Abstract;
using SignalR.DataAccessLayer.Abstract;
using SignalR.EntityLayer.Entities;
using System.Linq.Expressions;

namespace SignalR.BusinessLayer.Concrete
{
    public class RestaurantTableManager : IRestaurantTableService
    {
        private readonly IRestaurantTableDal _restaurantTableDal;

        public RestaurantTableManager(IRestaurantTableDal restaurantTableDal)
        {
            _restaurantTableDal = restaurantTableDal;
        }

        public async Task TCreateAsync(RestaurantTable entity)
        {
            await _restaurantTableDal.CreateAsync(entity);
        }

        public async Task TDeleteAsync(int id)
        {
            await _restaurantTableDal.DeleteAsync(id);
        }

        public async Task<int> TGetActiveTableCount()
        {
           return await _restaurantTableDal.GetActiveTableCount();
        }

        public async Task<List<RestaurantTable>> TGetAllAsync()
        {
            return await _restaurantTableDal.GetAllAsync();
        }

        public async Task<List<RestaurantTable>> TGetAllFilterAsync(Expression<Func<RestaurantTable, bool>> filter)
        {
            return await _restaurantTableDal.GetAllFilterAsync(filter);
        }

        public async Task<RestaurantTable?> TGetByIdAsync(int id)
        {
            return await _restaurantTableDal.GetByIdAsync(id);
        }

        public async Task<int> TGetTableCount()
        {
            return await _restaurantTableDal.GetTableCount();
        }

        public async Task TUpdateAsync(RestaurantTable entity)
        {
            await _restaurantTableDal.UpdateAsync(entity);
        }
    }
}

using SignalR.BusinessLayer.Abstract;
using SignalR.DataAccessLayer.Abstract;
using SignalR.EntityLayer.Entities;
using System.Linq.Expressions;

namespace SignalR.BusinessLayer.Concrete
{
    public class FeatureManager : IFeatureService
    {
        private readonly IFeatureDal _featureDal;

        public FeatureManager(IFeatureDal FeatureDal)
        {
            _featureDal = FeatureDal;
        }

        public async Task TCreateAsync(Feature entity)
        {
            await _featureDal.CreateAsync(entity);
        }

        public async Task TDeleteAsync(int id)
        {
            await _featureDal.DeleteAsync(id);
        }

        public async Task<List<Feature>> TGetAllAsync()
        {
            return await _featureDal.GetAllAsync();
        }

        public async Task<List<Feature>> TGetAllFilterAsync(Expression<Func<Feature, bool>> filter)
        {
            return await _featureDal.GetAllFilterAsync(filter);
        }

        public async Task<Feature?> TGetByIdAsync(int id)
        {
            return await _featureDal.GetByIdAsync(id);
        }

        public async Task TUpdateAsync(Feature entity)
        {
            await _featureDal.UpdateAsync(entity);
        }
    }
}

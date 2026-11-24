using SignalR.BusinessLayer.Abstract;
using SignalR.DataAccessLayer.Abstract;
using SignalR.EntityLayer.Entities;
using System.Linq.Expressions;

namespace SignalR.BusinessLayer.Concrete
{
    public class MoneyCaseManager : IMoneyCaseService
    {
        private readonly IMoneyCaseDal _moneyCaseDal;

        public MoneyCaseManager(IMoneyCaseDal moneyCaseDal)
        {
            _moneyCaseDal = moneyCaseDal;
        }

        public async Task<decimal> TGetTotalAmountMoneyCase()
        {
            return await _moneyCaseDal.GetTotalAmountMoneyCase();
        }

        public Task TCreateAsync(MoneyCase entity)
        {
            throw new NotImplementedException();
        }

        public Task TDeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<MoneyCase>> TGetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<MoneyCase>> TGetAllFilterAsync(Expression<Func<MoneyCase, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public Task<MoneyCase?> TGetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task TUpdateAsync(MoneyCase entity)
        {
            throw new NotImplementedException();
        }
    }
}

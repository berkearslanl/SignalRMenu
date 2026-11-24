using Microsoft.EntityFrameworkCore;
using SignalR.DataAccessLayer.Abstract;
using SignalR.DataAccessLayer.Concrete;
using SignalR.DataAccessLayer.Repositories;
using SignalR.EntityLayer.Entities;

namespace SignalR.DataAccessLayer.EntityFramework
{
    public class EfMoneyCaseDal : GenericRepository<MoneyCase>, IMoneyCaseDal
    {
        public EfMoneyCaseDal(SignalrContext context) : base(context)
        {
        }

        public async Task<decimal> GetTotalAmountMoneyCase()
        {
            return await _context.MoneyCases.Select(x => x.TotalAmount).FirstOrDefaultAsync();
        }
    }
}
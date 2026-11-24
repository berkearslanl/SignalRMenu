using Microsoft.EntityFrameworkCore;
using SignalR.DataAccessLayer.Abstract;
using SignalR.DataAccessLayer.Concrete;
using SignalR.DataAccessLayer.Repositories;
using SignalR.EntityLayer.Entities;

namespace SignalR.DataAccessLayer.EntityFramework
{
    public class EfTestimonialDal : GenericRepository<Testimonial>, ITestimonialDal
    {
        public EfTestimonialDal(SignalrContext context) : base(context)
        {
        }

        public async Task<int> GetTestimonialCountAsync()
        {
            return await _context.Testimonials.CountAsync(x => x.Status);
        }
    }
}
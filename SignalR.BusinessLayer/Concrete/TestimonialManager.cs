using SignalR.BusinessLayer.Abstract;
using SignalR.DataAccessLayer.Abstract;
using SignalR.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace SignalR.BusinessLayer.Concrete
{
    public class TestimonialManager : ITestimonialService
    {
        private readonly ITestimonialDal _testimonialDal;

        public TestimonialManager(ITestimonialDal testimonialDal)
        {
            _testimonialDal = testimonialDal;
        }

        public async Task TCreateAsync(Testimonial entity)
        {
            await _testimonialDal.CreateAsync(entity);
        }

        public async Task TDeleteAsync(int id)
        {
            await _testimonialDal.DeleteAsync(id);
        }

        public async Task<List<Testimonial>> TGetAllAsync()
        {
            return await _testimonialDal.GetAllAsync();
        }

        public async Task<List<Testimonial>> TGetAllFilterAsync(Expression<Func<Testimonial, bool>> filter)
        {
            return await _testimonialDal.GetAllFilterAsync(filter);
        }

        public async Task<Testimonial?> TGetByIdAsync(int id)
        {
            return await _testimonialDal.GetByIdAsync(id);
        }

        public async Task<int> TGetTestimonialCountAsync()
        {
            return await _testimonialDal.GetTestimonialCountAsync();
        }

        public async Task TUpdateAsync(Testimonial entity)
        {
            await _testimonialDal.UpdateAsync(entity);
        }
    }
}
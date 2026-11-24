using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SignalR.BusinessLayer.Abstract;
using SignalR.DtoLayer.Dtos.TestimonialDtos;
using SignalR.EntityLayer.Entities;

namespace SignalRApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class TestimonialsController : ControllerBase
    {
        private readonly ITestimonialService _testimonialService;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateTestimonialDto> _createValidator;
        private readonly IValidator<UpdateTestimonialDto> _updateValidator;

        public TestimonialsController(ITestimonialService TestimonialService, IMapper mapper, IValidator<CreateTestimonialDto> createValidator, IValidator<UpdateTestimonialDto> updateValidator)
        {
            _testimonialService = TestimonialService;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }
        [HttpGet]
        public async Task<IActionResult> TestimonialList()
        {
            var values = await _testimonialService.TGetAllAsync();
            var Testimonials = _mapper.Map<List<ResultTestimonialDto>>(values);
            return Ok(Testimonials);
        }
        [HttpGet("GetTestimonialById/{id}")]
        public async Task<IActionResult> GetTestimonialById(int id)
        {
            var values = await _testimonialService.TGetByIdAsync(id);
            var Testimonials = _mapper.Map<ResultTestimonialDto>(values);
            return Ok(Testimonials);
        }
        [HttpPost]
        public async Task<IActionResult> CreateTestimonial(CreateTestimonialDto dto)
        {
            var result = await _createValidator.ValidateAsync(dto);
            if (!result.IsValid)
                return BadRequest(result.Errors.Select(e => e.ErrorMessage));

            var values = _mapper.Map<Testimonial>(dto);
            await _testimonialService.TCreateAsync(values);
            return Ok("Ekleme işlemi başarılı.");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTestimonial(int id)
        {
            await _testimonialService.TDeleteAsync(id);
            return Ok("Silme işlemi başarılı.");
        }
        [HttpPut]
        public async Task<IActionResult> UpdateTestimonial(UpdateTestimonialDto dto)
        {
            var result = await _updateValidator.ValidateAsync(dto);
            if (!result.IsValid)
                return BadRequest(result.Errors.Select(e => e.ErrorMessage));

            var values = _mapper.Map<Testimonial>(dto);
            await _testimonialService.TUpdateAsync(values);
            return Ok("Güncelleme işlemi başarılı.");
        }
        [HttpGet("ChangeStatus/{id}")]
        public async Task<IActionResult> ChangeTestimonialStatus(int id)
        {
            var value = await _testimonialService.TGetByIdAsync(id);
            if (value == null)
                return NotFound();
            value.Status = !value.Status;
            await _testimonialService.TUpdateAsync(value);
            return Ok("Durum başarıyla değiştirildi.");
        }
    }
}

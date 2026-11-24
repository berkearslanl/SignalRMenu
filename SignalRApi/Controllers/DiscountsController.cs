using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SignalR.BusinessLayer.Abstract;
using SignalR.DtoLayer.Dtos.DiscountDtos;
using SignalR.EntityLayer.Entities;

namespace SignalRApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class DiscountsController : ControllerBase
    {
        private readonly IDiscountService _discountService;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateDiscountDto> _createValidator;
        private readonly IValidator<UpdateDiscountDto> _updateValidator;

        public DiscountsController(IDiscountService DiscountService, IMapper mapper, IValidator<CreateDiscountDto> createValidator, IValidator<UpdateDiscountDto> updateValidator)
        {
            _discountService = DiscountService;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }
        [HttpGet]
        public async Task<IActionResult> DiscountList()
        {
            var values = await _discountService.TGetAllAsync();
            var Discounts = _mapper.Map<List<ResultDiscountDto>>(values);
            return Ok(Discounts);
        }
        [HttpGet("Last2Discount")]
        public async Task<IActionResult> Last2Discount()
        {
            var values = await _discountService.TGetLast2DiscountAsync();
            var Discounts = _mapper.Map<List<ResultDiscountDto>>(values);
            return Ok(Discounts);
        }
        [HttpGet("GetDiscountById/{id}")]
        public async Task<IActionResult> GetDiscountById(int id)
        {
            var values = await _discountService.TGetByIdAsync(id);
            var Discounts = _mapper.Map<ResultDiscountDto>(values);
            return Ok(Discounts);
        }
        [HttpPost]
        public async Task<IActionResult> CreateDiscount(CreateDiscountDto dto)
        {
            var result = await _createValidator.ValidateAsync(dto);
            if (!result.IsValid)
                return BadRequest(result.Errors.Select(e => e.ErrorMessage));

            var values = _mapper.Map<Discount>(dto);
            await _discountService.TCreateAsync(values);
            return Ok("Ekleme işlemi başarılı.");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDiscount(int id)
        {
            await _discountService.TDeleteAsync(id);
            return Ok("Silme işlemi başarılı.");
        }
        [HttpPut]
        public async Task<IActionResult> UpdateDiscount(UpdateDiscountDto dto)
        {
            var result = await _updateValidator.ValidateAsync(dto);
            if (!result.IsValid)
                return BadRequest(result.Errors.Select(e => e.ErrorMessage));

            var values = _mapper.Map<Discount>(dto);
            await _discountService.TUpdateAsync(values);
            return Ok("Güncelleme işlemi başarılı.");
        }
    }
}

using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SignalR.BusinessLayer.Abstract;
using SignalR.DtoLayer.Dtos.AboutDtos;
using SignalR.EntityLayer.Entities;

namespace SignalRApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AboutsController : ControllerBase
    {
        private readonly IAboutService _aboutService;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateAboutDto> _createValidator;
        private readonly IValidator<UpdateAboutDto> _updateValidator;

        public AboutsController(IAboutService aboutService, IMapper mapper, IValidator<CreateAboutDto> createValidator, IValidator<UpdateAboutDto> updateValidator)
        {
            _aboutService = aboutService;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }
        [HttpGet]
        public async Task<IActionResult> AboutList()
        {
            var values = await _aboutService.TGetAllAsync();
            var abouts = _mapper.Map<List<ResultAboutDto>>(values);
            return Ok(abouts);
        }
        [HttpGet("GetAboutById/{id}")]
        public async Task<IActionResult> GetAboutById(int id)
        {
            var values = await _aboutService.TGetByIdAsync(id);
            var abouts = _mapper.Map<ResultAboutDto>(values);
            return Ok(abouts);
        }
        [HttpPost]
        public async Task<IActionResult> CreateAbout(CreateAboutDto dto)
        {
            var result = await _createValidator.ValidateAsync(dto);
            if (!result.IsValid)
                return BadRequest(result.Errors.Select(e => e.ErrorMessage));

            var values = _mapper.Map<About>(dto);
            await _aboutService.TCreateAsync(values);
            return Ok("Ekleme işlemi başarılı.");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAbout(int id)
        {
            await _aboutService.TDeleteAsync(id);
            return Ok("Silme işlemi başarılı.");
        }
        [HttpPut]
        public async Task<IActionResult> UpdateAbout(UpdateAboutDto dto)
        {
            var result = await _updateValidator.ValidateAsync(dto);
            if (!result.IsValid)
                return BadRequest(result.Errors.Select(e => e.ErrorMessage));

            var values = _mapper.Map<About>(dto);
            await _aboutService.TUpdateAsync(values);
            return Ok("Güncelleme işlemi başarılı.");
        }
    }
}
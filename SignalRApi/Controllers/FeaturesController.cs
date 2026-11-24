using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SignalR.BusinessLayer.Abstract;
using SignalR.DtoLayer.Dtos.FeatureDtos;
using SignalR.EntityLayer.Entities;

namespace SignalRApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class FeaturesController : ControllerBase
    {
        private readonly IFeatureService _featureService;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateFeatureDto> _createValidator;
        private readonly IValidator<UpdateFeatureDto> _updateValidator;

        public FeaturesController(IFeatureService FeatureService, IMapper mapper, IValidator<CreateFeatureDto> createValidator, IValidator<UpdateFeatureDto> updateValidator)
        {
            _featureService = FeatureService;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }
        [HttpGet]
        public async Task<IActionResult> FeatureList()
        {
            var values = await _featureService.TGetAllAsync();
            var Features = _mapper.Map<List<ResultFeatureDto>>(values);
            return Ok(Features);
        }
        [HttpGet("GetFeatureById/{id}")]
        public async Task<IActionResult> GetFeatureById(int id)
        {
            var values = await _featureService.TGetByIdAsync(id);
            var Features = _mapper.Map<ResultFeatureDto>(values);
            return Ok(Features);
        }
        [HttpPost]
        public async Task<IActionResult> CreateFeature(CreateFeatureDto dto)
        {
            var result = await _createValidator.ValidateAsync(dto);
            if (!result.IsValid)
                return BadRequest(result.Errors.Select(e => e.ErrorMessage));

            var values = _mapper.Map<Feature>(dto);
            await _featureService.TCreateAsync(values);
            return Ok("Ekleme işlemi başarılı.");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeature(int id)
        {
            await _featureService.TDeleteAsync(id);
            return Ok("Silme işlemi başarılı.");
        }
        [HttpPut]
        public async Task<IActionResult> UpdateFeature(UpdateFeatureDto dto)
        {
            var result = await _updateValidator.ValidateAsync(dto);
            if (!result.IsValid)
                return BadRequest(result.Errors.Select(e => e.ErrorMessage));

            var values = _mapper.Map<Feature>(dto);
            await _featureService.TUpdateAsync(values);
            return Ok("Güncelleme işlemi başarılı.");
        }
    }
}

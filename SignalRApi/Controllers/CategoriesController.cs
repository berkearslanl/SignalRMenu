using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SignalR.BusinessLayer.Abstract;
using SignalR.DtoLayer.Dtos.CategoryDtos;
using SignalR.EntityLayer.Entities;

namespace SignalRApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateCategoryDto> _createValidator;
        private readonly IValidator<UpdateCategoryDto> _updateValidator;

        public CategoriesController(ICategoryService CategoryService, IMapper mapper, IValidator<CreateCategoryDto> createValidator, IValidator<UpdateCategoryDto> updateValidator)
        {
            _categoryService = CategoryService;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }
        [HttpGet]
        public async Task<IActionResult> CategoryList()
        {
            var values = await _categoryService.TGetAllAsync();
            var Categorys = _mapper.Map<List<ResultCategoryDto>>(values);
            return Ok(Categorys);
        }
        [HttpGet("CategoryCount")]
        public async Task<IActionResult> CategoryCount()
        {
            var values = await _categoryService.TGetCategoryCountAsync();
            return Ok(values);
        }
        [HttpGet("ActiveCategoryCount")]
        public async Task<IActionResult> ActiveCategoryCount()
        {
            var values = await _categoryService.TGetActiveCategoryCountAsync();
            return Ok(values);
        }
        [HttpGet("PasiveCategoryCount")]
        public async Task<IActionResult> PasiveCategoryCount()
        {
            var values = await _categoryService.TGetPasiveCategoryCountAsync();
            return Ok(values);
        }
        [HttpGet("GetCategoryById/{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var values = await _categoryService.TGetByIdAsync(id);
            var Categorys = _mapper.Map<ResultCategoryDto>(values);
            return Ok(Categorys);
        }
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryDto dto)
        {
            var result = await _createValidator.ValidateAsync(dto);
            if (!result.IsValid)
                return BadRequest(result.Errors.Select(e => e.ErrorMessage));

            var values = _mapper.Map<Category>(dto);
            await _categoryService.TCreateAsync(values);
            return Ok("Ekleme işlemi başarılı.");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await _categoryService.TDeleteAsync(id);
            return Ok("Silme işlemi başarılı.");
        }
        [HttpPut]
        public async Task<IActionResult> UpdateCategory(UpdateCategoryDto dto)
        {
            var result = await _updateValidator.ValidateAsync(dto);
            if (!result.IsValid)
                return BadRequest(result.Errors.Select(e => e.ErrorMessage));

            var values = _mapper.Map<Category>(dto);
            await _categoryService.TUpdateAsync(values);
            return Ok("Güncelleme işlemi başarılı.");
        }
        [HttpGet("ChangeStatus/{id}")]
        public async Task<IActionResult> ChangeCategoryStatus(int id)
        {
            var value = await _categoryService.TGetByIdAsync(id);
            if (value == null)
                return NotFound();
            value.Status = !value.Status;
            await _categoryService.TUpdateAsync(value);
            return Ok("Durum başarıyla değiştirildi.");
        }
    }
}

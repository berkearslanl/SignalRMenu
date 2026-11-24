using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SignalR.BusinessLayer.Abstract;
using SignalR.DtoLayer.Dtos.ProductDtos;
using SignalR.EntityLayer.Entities;

namespace SignalRApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateProductDto> _createValidator;
        private readonly IValidator<UpdateProductDto> _updateValidator;

        public ProductsController(IProductService ProductService, IMapper mapper, IValidator<CreateProductDto> createValidator, IValidator<UpdateProductDto> updateValidator)
        {
            _productService = ProductService;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }
        [HttpGet]
        public async Task<IActionResult> ProductList()
        {
            var values = await _productService.TGetAllAsync();
            var Products = _mapper.Map<List<ResultProductDto>>(values);
            return Ok(Products);
        }
        [HttpGet("Last9Product")]
        public async Task<IActionResult> Last9Product()
        {
            var values = await _productService.TLast9ProductsWithCategoriesAsync();
            var Products = _mapper.Map<List<GetProductsWithCategoryDto>>(values);
            return Ok(Products);
        }
        [HttpGet("ProductCount")]
        public async Task<IActionResult> ProductCount()
        {
            var values = await _productService.TGetProductCountAsync();
            return Ok(values);
        }
        [HttpGet("AvgProductPrice")]
        public async Task<IActionResult> AvgProductPrice()
        {
            var values = await _productService.TGetAvgProductPrice();
            return Ok(values);
        }
        [HttpGet("MaxProductPrice")]
        public async Task<IActionResult> MaxProductPrice()
        {
            var values = await _productService.TGetProductNameByMaxPrice();
            return Ok(values);
        }
        [HttpGet("MinProductPrice")]
        public async Task<IActionResult> MinProductPrice()
        {
            var values = await _productService.TGetProductNameByMinPrice();
            return Ok(values);
        }
        [HttpGet("ProductCountByHamburger")]
        public async Task<IActionResult> ProductCountByHamburger()
        {
            var values = await _productService.TGetProductCountByCategoryNameHamburger();
            return Ok(values);
        }
        [HttpGet("ProductCountByDrink")]
        public async Task<IActionResult> ProductCountByDrink()
        {
            var values = await _productService.TGetProductCountByCategoryNameDrink();
            return Ok(values);
        }
        [HttpGet("GetProductsWithCategory")]
        public async Task<IActionResult> GetProductsWithCategory()
        {
            var values = await _productService.TGetAllProductsWithCategoriesAsync();
            var Products = _mapper.Map<List<GetProductsWithCategoryDto>>(values);
            return Ok(Products);
        }
        [HttpGet("GetProductById/{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var values = await _productService.TGetByIdAsync(id);
            var Products = _mapper.Map<ResultProductDto>(values);
            return Ok(Products);
        }
        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductDto dto)
        {
            var result = await _createValidator.ValidateAsync(dto);
            if (!result.IsValid)
                return BadRequest(result.Errors.Select(e => e.ErrorMessage));

            var values = _mapper.Map<Product>(dto);
            await _productService.TCreateAsync(values);
            return Ok("Ekleme işlemi başarılı.");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _productService.TDeleteAsync(id);
            return Ok("Silme işlemi başarılı.");
        }
        [HttpPut]
        public async Task<IActionResult> UpdateProduct(UpdateProductDto dto)
        {
            var result = await _updateValidator.ValidateAsync(dto);
            if (!result.IsValid)
                return BadRequest(result.Errors.Select(e => e.ErrorMessage));

            var values = _mapper.Map<Product>(dto);
            await _productService.TUpdateAsync(values);
            return Ok("Güncelleme işlemi başarılı.");
        }
        [HttpGet("ChangeStatus/{id}")]
        public async Task<IActionResult> ChangeProductStatus(int id)
        {
            var value = await _productService.TGetByIdAsync(id);
            if (value == null)
                return NotFound();
            value.Status = !value.Status;
            await _productService.TUpdateAsync(value);
            return Ok("Durum başarıyla değiştirildi.");
        }
    }
}

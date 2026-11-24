using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalR.BusinessLayer.Abstract;
using SignalR.DtoLayer.Dtos.BasketDtos;
using SignalR.EntityLayer.Entities;
using SignalR.EntityLayer.Enums;
using SignalRApi.Hubs;

namespace SignalRApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class BasketsController : ControllerBase
    {
        private readonly IBasketService _basketService;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateBasketDto> _createValidator;
        private readonly IRestaurantTableService _restaurantTableService;
        private readonly IHubContext<SignalRHub> _hub;

        public BasketsController(IBasketService basketService, IMapper mapper, IProductService productService, IValidator<CreateBasketDto> createValidator, IRestaurantTableService restaurantTableService, IHubContext<SignalRHub> hub)
        {
            _basketService = basketService;
            _mapper = mapper;
            _createValidator = createValidator;
            _productService = productService;
            _restaurantTableService = restaurantTableService;
            _hub = hub;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBasketByTable(int id)
        {
            var values = await _basketService.TGetBasketByTable(id);
            var baskets = _mapper.Map<List<GetBasketWithProductAndTableDto>>(values);
            return Ok(baskets);
        }
        [HttpPost]
        public async Task<IActionResult> CreateBasket(CreateBasketDto dto)
        {
            var result = await _createValidator.ValidateAsync(dto);
            if (!result.IsValid)
                return BadRequest(result.Errors.Select(e => e.ErrorMessage));

            var table = await _restaurantTableService.TGetByIdAsync(dto.RestaurantTableId);
            if (table == null || table.Status != TableStatus.Dolu)
                return Conflict("Masanız kapatılmış. Lütfen masaya tekrar oturun.");

            var products = await _productService.TGetByIdAsync(dto.ProductId);
            if (products == null)
                return NotFound("Ürün bulunamadı.");

            var baskets = await _basketService.TGetBasketByTable(dto.RestaurantTableId);
            var existing = baskets.FirstOrDefault(x => x.ProductId == dto.ProductId);//bu ürün var

            if(existing!=null)//eğer boş değilse yani bu ürün varsa
            {
                existing.Count += 1;
                existing.TotalPrice = existing.Count * products.Price;
                await _basketService.TUpdateAsync(existing);
            }

            else
            {
                await _basketService.TCreateAsync(new Basket
                {
                    ProductId = dto.ProductId,
                    Count = 1,
                    RestaurantTableId = dto.RestaurantTableId,
                    TotalPrice = products.Price
                });
            }
            await SendBasketCountAsync(dto.RestaurantTableId);
            return Ok("Ekleme işlemi başarılı.");
        }
        [HttpDelete("DeleteBasketItem/{id}")]
        public async Task<IActionResult> DeleteBasketItem(int id)
        {
            var basket = await _basketService.TGetByIdAsync(id);
            var tableId = basket.RestaurantTableId;
            if (basket == null)
                return NotFound("Sepet kaydı bulunamadı.");

            if (basket.Count > 1)
            {
                var product = await _productService.TGetByIdAsync(basket.ProductId);
                basket.Count -= 1;
                basket.TotalPrice = basket.Count * product.Price;
                await _basketService.TUpdateAsync(basket);
                return Ok("Ürün adedi bir azaltıldı.");
            }
            await _basketService.TDeleteAsync(id);
            await SendBasketCountAsync(tableId);
            return Ok("Ürün sepetten silindi.");
        }
        [HttpDelete("DeleteFullBasket/{id}")]
        public async Task<IActionResult> DeleteFullBasket(int id)
        {
            await _basketService.TDeleteAsync(id);
            return Ok("Başarıyla silindi.");
        }
        private async Task SendBasketCountAsync(int tableId)
        {
            var baskets = await _basketService.TGetBasketByTable(tableId);
            var count = baskets.Sum(x => x.Count);
            await _hub.Clients.Group($"Table-{tableId}").SendAsync("ReceiveBasketCount", count);
        }
    }
}
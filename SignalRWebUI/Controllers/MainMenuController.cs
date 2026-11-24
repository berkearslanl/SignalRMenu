using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SignalR.DtoLayer.Dtos.BasketDtos;
using SignalR.DtoLayer.Dtos.ProductDtos;
using System.Text;

namespace SignalRWebUI.Controllers
{
    [AllowAnonymous]
    public class MainMenuController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public MainMenuController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7003/api/Products/GetProductsWithCategory");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<GetProductsWithCategoryDto>>(json);
                return View(values);
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddBasket(CreateBasketDto dto, int id)
        {
            if (!int.TryParse(Request.Cookies["TableId"], out var tableId))
                return BadRequest("Masanızdaki QR kodu okutunuz.");
            
            dto.ProductId = id;
            dto.RestaurantTableId = tableId;
            
            var client = _httpClientFactory.CreateClient();
            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://localhost:7003/api/Baskets", content);
            
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index", "Basket", new { id=dto.RestaurantTableId });

            if(response.StatusCode ==System.Net.HttpStatusCode.Conflict)
            {
                Response.Cookies.Delete("TableId");
                return Conflict("Hesabınız kapatılmış, lütfen tekrar masa seçiniz.");
            }
            return BadRequest("Ürün sepete eklenemedi.");
        }
    }
}

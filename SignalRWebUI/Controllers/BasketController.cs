using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SignalR.DtoLayer.Dtos.BasketDtos;

namespace SignalRWebUI.Controllers
{
    [AllowAnonymous]
    public class BasketController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public BasketController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IActionResult> Index(int id)
        {
            if (!int.TryParse(Request.Cookies["TableId"], out var tableId))
                return BadRequest("Masanızdaki QR kodu okutunuz.");

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7003/api/Baskets/{tableId}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<GetBasketWithProductAndTableDto>>(json);
                return View(values);
            }
            return View();
        }
        public async Task<IActionResult> DeleteBasket(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.DeleteAsync($"https://localhost:7003/api/Baskets/DeleteBasketItem/{id}");
            if (response.IsSuccessStatusCode)
                return Json(new { success = true });

            return Json(new { success = false });
        }
        [HttpPost]
        public async Task<IActionResult> CompleteOrder()
        {
            if (!int.TryParse(Request.Cookies["TableId"], out var tableId))
                return BadRequest("Masanızdaki QR kodu okutunuz.");

            var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsync($"https://localhost:7003/api/Orders/CompleteOrder/{tableId}", null);

            if (response.IsSuccessStatusCode)
                return Json(new { success = true });

            var message = await response.Content.ReadAsStringAsync();
            return BadRequest(message);
        }
    }
}
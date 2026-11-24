using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using SignalR.DtoLayer.Dtos.RestaurantTableDtos;

namespace SignalRWebUI.Controllers
{
    [AllowAnonymous]
    public class CustomerTableController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CustomerTableController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IActionResult> CustomerTableList()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7003/api/RestaurantTables");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultRestaurantTableDto>>(json);
                return View(values);
            }
            return View();
        }

        public async Task<IActionResult> Join(int id)
        {
            var current = Request.Cookies["TableId"];
            if (current == id.ToString())
                return RedirectToAction("Index", "MainMenu");
            if (!string.IsNullOrEmpty(current))
            {
                TempData["TableError"] = "Önce şu anki masanızdan kalkmalısınız.";
                return RedirectToAction("CustomerTableList");
            }

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7003/api/RestaurantTables/SitTable/{id}");
            if (!response.IsSuccessStatusCode)
            {
                TempData["TableError"] = "Bu masa dolu ya da bulunamadı.";
                return RedirectToAction("CustomerTableList");
            }
            Response.Cookies.Append("TableId", id.ToString(), new CookieOptions
            {
                HttpOnly = true,
                IsEssential = true,
                SameSite = SameSiteMode.Lax
            });
            return RedirectToAction("Index", "MainMenu");
        }
        public async Task<IActionResult> Leave()
        {
            if (int.TryParse(Request.Cookies["TableId"], out var id))
            {
                var client = _httpClientFactory.CreateClient();
                await client.GetAsync($"https://localhost:7003/api/RestaurantTables/LeaveTable/{id}");
            }
            Response.Cookies.Delete("TableId");
            return RedirectToAction("CustomerTableList", "CustomerTable");
        }
    }
}
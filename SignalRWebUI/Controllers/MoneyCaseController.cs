using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace SignalRWebUI.Controllers
{
    public class MoneyCaseController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public MoneyCaseController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7003/api/MoneyCases/GetTotalAmount");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var totalAmount = JsonConvert.DeserializeObject<decimal>(json);
                return View(totalAmount);
            }
            ViewBag.Error = "Kasa bilgisi alınamadı.";
            return View(0m);
        }
    }
}
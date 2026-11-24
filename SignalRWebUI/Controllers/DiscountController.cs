using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SignalR.DtoLayer.Dtos.Errors;
using SignalR.DtoLayer.Dtos.DiscountDtos;
using System.Text;

namespace SignalRWebUI.Controllers
{
    public class DiscountController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DiscountController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7003/api/Discounts");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultDiscountDto>>(json);
                return View(values);
            }
            return View();
        }
        [HttpGet]
        public IActionResult CreateDiscount()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateDiscount(CreateDiscountDto dto)
        {
            ModelState.Clear();
            var client = _httpClientFactory.CreateClient();
            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://localhost:7003/api/Discounts", content);
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index", "Discount");
            var errorContent = await response.Content.ReadAsStringAsync();
            var errors = JsonConvert.DeserializeObject<List<ApiError>>(errorContent);
            foreach (var err in errors)
                ModelState.AddModelError(err.PropertyName, err.ErrorMessage);
            return View(dto);
        }
        [HttpGet]
        public async Task<IActionResult> UpdateDiscount(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7003/api/Discounts/GetDiscountById/{id}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<UpdateDiscountDto>(json);
                return View(values);
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UpdateDiscount(UpdateDiscountDto dto)
        {
            ModelState.Clear();
            var client = _httpClientFactory.CreateClient();
            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync("https://localhost:7003/api/Discounts", content);
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index", "Discount");
            var errorContent = await response.Content.ReadAsStringAsync();
            var errors = JsonConvert.DeserializeObject<List<ApiError>>(errorContent);
            foreach (var err in errors)
                ModelState.AddModelError(err.PropertyName, err.ErrorMessage);
            return View(dto);
        }
        public async Task<IActionResult> DeleteDiscount(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.DeleteAsync($"https://localhost:7003/api/Discounts/{id}");
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index", "Discount");
            return View();
        }
    }
}

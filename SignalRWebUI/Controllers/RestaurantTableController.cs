using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SignalR.DtoLayer.Dtos.Errors;
using SignalR.DtoLayer.Dtos.RestaurantTableDtos;
using System.Text;

namespace SignalRWebUI.Controllers
{
    public class RestaurantTableController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public RestaurantTableController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7003/api/RestaurantTables");
            if(response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultRestaurantTableDto>>(json);
                return View(values);
            }
            return View();
        }
        public async Task<IActionResult> DeleteTable(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.DeleteAsync($"https://localhost:7003/api/RestaurantTables/{id}");
            return RedirectToAction("Index", "RestaurantTable");
        }

        [HttpGet]
        public IActionResult CreateTable() { return View(); }
        [HttpPost]
        public async Task<IActionResult> CreateTable(CreateRestaurantTableDto dto)
        {
            ModelState.Clear();
            var client = _httpClientFactory.CreateClient();
            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://localhost:7003/api/RestaurantTables", content);
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index", "RestaurantTable");
            var errorContent = await response.Content.ReadAsStringAsync();
            var errors = JsonConvert.DeserializeObject<List<ApiError>>(errorContent);
            foreach (var err in errors)
                ModelState.AddModelError(err.PropertyName, err.ErrorMessage);
            return View(dto);
        }
        public async Task<IActionResult> ChangeTableStatus(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7003/api/RestaurantTables/GetRestaurantTableById/{id}");
            var json = await response.Content.ReadAsStringAsync();
            var table = JsonConvert.DeserializeObject<ResultRestaurantTableDto>(json);

            var action = table.Status == 1 ? "LeaveTable" : "SitTable";
            await client.GetAsync($"https://localhost:7003/api/RestaurantTables/{action}/{id}");
            return RedirectToAction("Index", "RestaurantTable");
        }
    }
}
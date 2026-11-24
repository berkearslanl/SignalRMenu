using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SignalR.DtoLayer.Dtos.Errors;
using SignalR.DtoLayer.Dtos.OrderDtos;
using SignalR.DtoLayer.Dtos.RestaurantTableDtos;
using System.Text;

namespace SignalRWebUI.Controllers
{
    public class OrderController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public OrderController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7003/api/Orders/GetOrdersWithTable");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<GetOrdersWithTableDto>>(json);
                return View(values);
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> CreateOrder()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7003/api/RestaurantTables");
            var json = await response.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResultRestaurantTableDto>>(json);
            List<SelectListItem> table = (from x in values
                                          select new SelectListItem
                                          {
                                              Value = x.RestaurantTableId.ToString(),
                                              Text = x.TableName
                                          }).ToList();
            ViewBag.Tables = table;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderDto dto)
        {
            ModelState.Clear();
            var client = _httpClientFactory.CreateClient();
            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://localhost:7003/api/Orders", content);
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index", "Order");
            var errorContent = await response.Content.ReadAsStringAsync();
            var errors = JsonConvert.DeserializeObject<List<ApiError>>(errorContent);
            foreach (var err in errors)
                ModelState.AddModelError(err.PropertyName, err.ErrorMessage);
            await FillTablesAsync();
            return View(dto);
        }
        [HttpGet]
        public async Task<IActionResult> UpdateOrder(int id)
        {
            var client = _httpClientFactory.CreateClient();

            var tableResponse = await client.GetAsync("https://localhost:7003/api/RestaurantTables");
            var tableJson = await tableResponse.Content.ReadAsStringAsync();
            var tableValues = JsonConvert.DeserializeObject<List<ResultRestaurantTableDto>>(tableJson);
            List<SelectListItem> table = (from x in tableValues
                                          select new SelectListItem
                                          {
                                              Value = x.RestaurantTableId.ToString(),
                                              Text = x.TableName
                                          }).ToList();
            ViewBag.Tables = table;

            var response = await client.GetAsync($"https://localhost:7003/api/Orders/GetOrderById/{id}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<UpdateOrderDto>(json);
                return View(values);
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UpdateOrder(UpdateOrderDto dto)
        {
            ModelState.Clear();
            var client = _httpClientFactory.CreateClient();
            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync("https://localhost:7003/api/Orders", content);
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index", "Order");
            var errorContent = await response.Content.ReadAsStringAsync();
            var errors = JsonConvert.DeserializeObject<List<ApiError>>(errorContent);
            foreach (var err in errors)
                ModelState.AddModelError(err.PropertyName, err.ErrorMessage);
            await FillTablesAsync();
            return View(dto);
        }
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.DeleteAsync($"https://localhost:7003/api/Orders/{id}");
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index", "Order");
            return View();
        }
        private async Task FillTablesAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7003/api/RestaurantTables");
            var json = await response.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResultRestaurantTableDto>>(json);
            ViewBag.Tables = values.Select(x => new SelectListItem { Value = x.RestaurantTableId.ToString(), Text = x.TableName }).ToList();
        }
    }
}

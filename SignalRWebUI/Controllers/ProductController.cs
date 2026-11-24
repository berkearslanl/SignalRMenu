using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SignalR.DtoLayer.Dtos.Errors;
using SignalR.DtoLayer.Dtos.CategoryDtos;
using SignalR.DtoLayer.Dtos.ProductDtos;
using System.Text;

namespace SignalRWebUI.Controllers
{
    public class ProductController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductController(IHttpClientFactory httpClientFactory)
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
        [HttpGet]
        public async Task<IActionResult> CreateProduct()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7003/api/Categories");
            var json = await response.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResultCategoryDto>>(json);
            List<SelectListItem> category = (from x in values
                                       select new SelectListItem
                                       {
                                           Value = x.CategoryId.ToString(),
                                           Text = x.CategoryName
                                       }).ToList();
            ViewBag.Categories = category;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductDto dto)
        {
            ModelState.Clear();
            dto.Status = true;
            var client = _httpClientFactory.CreateClient();
            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://localhost:7003/api/Products", content);
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index", "Product");
            var errorContent = await response.Content.ReadAsStringAsync();
            var errors = JsonConvert.DeserializeObject<List<ApiError>>(errorContent);
            foreach (var err in errors)
                ModelState.AddModelError(err.PropertyName, err.ErrorMessage);
            await FillCategoriesAsync();
            return View(dto);
        }
        [HttpGet]
        public async Task<IActionResult> UpdateProduct(int id)
        {
            var client = _httpClientFactory.CreateClient();

            var categoryResponse = await client.GetAsync("https://localhost:7003/api/Categories");
            var categoryJson = await categoryResponse.Content.ReadAsStringAsync();
            var categoryValues = JsonConvert.DeserializeObject<List<ResultCategoryDto>>(categoryJson);
            List<SelectListItem> category = (from x in categoryValues
                                             select new SelectListItem
                                             {
                                                 Value = x.CategoryId.ToString(),
                                                 Text = x.CategoryName
                                             }).ToList();
            ViewBag.Categories = category;
            
            var response = await client.GetAsync($"https://localhost:7003/api/Products/GetProductById/{id}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<UpdateProductDto>(json);
                return View(values);
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UpdateProduct(UpdateProductDto dto)
        {
            ModelState.Clear();
            var client = _httpClientFactory.CreateClient();
            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync("https://localhost:7003/api/Products", content);
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index", "Product");
            var errorContent = await response.Content.ReadAsStringAsync();
            var errors = JsonConvert.DeserializeObject<List<ApiError>>(errorContent);
            foreach (var err in errors)
                ModelState.AddModelError(err.PropertyName, err.ErrorMessage);
            await FillCategoriesAsync();
            return View(dto);
        }
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.DeleteAsync($"https://localhost:7003/api/Products/{id}");
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index", "Product");
            return View();
        }
        public async Task<IActionResult> ChangeProductStatus(int id)
        {
            var client = _httpClientFactory.CreateClient();
            await client.GetAsync($"https://localhost:7003/api/Products/ChangeStatus/{id}");
            return RedirectToAction("Index", "Product");
        }
        private async Task FillCategoriesAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7003/api/Categories");
            var json = await response.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResultCategoryDto>>(json);
            ViewBag.Categories = values.Select(x => new SelectListItem { Value = x.CategoryId.ToString(), Text = x.CategoryName }).ToList();
        }
    }
}

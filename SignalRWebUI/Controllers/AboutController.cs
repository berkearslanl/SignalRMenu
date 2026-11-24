using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SignalR.DtoLayer.Dtos.Errors;
using SignalR.DtoLayer.Dtos.AboutDtos;
using System.Text;

namespace SignalRWebUI.Controllers
{
    public class AboutController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AboutController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7003/api/Abouts");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultAboutDto>>(json);
                return View(values);
            }
            return View();
        }
        [HttpGet]
        public IActionResult CreateAbout()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateAbout(CreateAboutDto dto)
        {
            ModelState.Clear();
            var client = _httpClientFactory.CreateClient();
            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://localhost:7003/api/Abouts", content);
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index", "About");
            var errorContent = await response.Content.ReadAsStringAsync();
            var errors = JsonConvert.DeserializeObject<List<ApiError>>(errorContent);
            foreach (var err in errors)
                ModelState.AddModelError(err.PropertyName, err.ErrorMessage);
            return View(dto);
        }
        [HttpGet]
        public async Task<IActionResult> UpdateAbout(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7003/api/Abouts/GetAboutById/{id}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<UpdateAboutDto>(json);
                return View(values);
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UpdateAbout(UpdateAboutDto dto)
        {
            ModelState.Clear();
            var client = _httpClientFactory.CreateClient();
            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync("https://localhost:7003/api/Abouts", content);
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index", "About");
            var errorContent = await response.Content.ReadAsStringAsync();
            var errors = JsonConvert.DeserializeObject<List<ApiError>>(errorContent);
            foreach (var err in errors)
                ModelState.AddModelError(err.PropertyName, err.ErrorMessage);
            return View(dto);
        }
        public async Task<IActionResult> DeleteAbout(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.DeleteAsync($"https://localhost:7003/api/Abouts/{id}");
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index", "About");
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SignalR.DtoLayer.Dtos.SettingDtos;
using System.Text;

namespace SignalRWebUI.Controllers
{
    public class SettingController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public SettingController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7003/api/Settings/{User.Identity.Name}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<ResultUserInfoDto>(json);
                return View(values);
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(ResultUserInfoDto dto)
        {
            var client = _httpClientFactory.CreateClient();
            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync("https://localhost:7003/api/Settings", content);
            if (response.IsSuccessStatusCode)
            {
                TempData["SettingSuccess"] = "Bilgileriniz başarıyla güncellendi.";
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Error = "Bir sorun oluştu. Lütfen tekrar deneyin.";
            }
            return View();
        }
    }
}
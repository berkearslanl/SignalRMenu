using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SignalR.DtoLayer.Dtos.RapidApiDtos;
namespace SignalRWebUI.Controllers
{
    [AllowAnonymous]
    public class FoodRapidApiController : Controller
    {
        private readonly IConfiguration _configuration;

        public FoodRapidApiController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<IActionResult> Index()
        {
            var rapidApiKey = _configuration["RapidApi:Key"];
            if (string.IsNullOrWhiteSpace(rapidApiKey))
                return Content("RapidApi:Key tanımlı değil. User Secrets'a ekleyin.");

            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://tasty.p.rapidapi.com/recipes/list?from=0&size=20&tags=under_30_minutes"),
                Headers =
    {
        { "x-rapidapi-key", rapidApiKey },
        { "x-rapidapi-host", "" },
    },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                //var results = JsonConvert.DeserializeObject<List<ResultTastyDto>>(body);
                //return View(results);
                var root = JsonConvert.DeserializeObject<RootTasty>(body);
                var values = root.Results.ToList();
                return View(values);
            }
        }
    }
}
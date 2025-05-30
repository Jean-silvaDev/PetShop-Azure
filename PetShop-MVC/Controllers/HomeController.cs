using System.Diagnostics;
using AzureProject.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AzureProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpFactory;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpFactory)
        {
            _logger = logger;
            _httpFactory = httpFactory;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(SalesRequest salesRequest)
        {
            using var client = _httpFactory.CreateClient();
            client.BaseAddress = new Uri("http://localhost:7239/api/");

            var content = new StringContent(
                JsonConvert.SerializeObject(salesRequest),
                System.Text.Encoding.UTF8,
                "application/json"
            );

            using HttpResponseMessage response = await client.PostAsync("OnSalesUpdate", content);
            string responseBody = await response.Content.ReadAsStringAsync();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

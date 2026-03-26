using Core.Abstracts.IServices;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;
using UI.Models;

namespace UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IShowroomService service;
        public HomeController(IShowroomService service)
        {
            this.service = service;
        }

        public async Task<IActionResult> Index()
        {
            var result = await service.GetProductsAysnc();
            if (result.IsSuccess)
            {
                return View(result.Data);
            }
            return View();
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

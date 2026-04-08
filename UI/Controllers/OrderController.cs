using Core.Abstracts.IServices;
using Core.Concretes.Entities;
using Core.Concretes.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace UI.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderService service;
        private readonly UserManager<Customer> userManager;
        private readonly HttpClient client;
        public OrderController(IOrderService service, UserManager<Customer> userManager, IHttpClientFactory factory)
        {
            this.service = service;
            this.userManager = userManager;
            client = factory.CreateClient("payment");
        }

        public async Task<IActionResult> Index(int? id)
        {
            if (id.HasValue)
            {
                string? uid = userManager.GetUserId(User);
                if (!string.IsNullOrEmpty(uid))
                {
                    var result = await service.GetOrderAsync(id.Value, uid);
                    if (result != null)
                    {
                        return View(result);
                    }
                }
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create()
        {
            string? uid = userManager.GetUserId(User);
            if (!string.IsNullOrEmpty(uid))
            {
                int? id = await service.CreateOrderAsync(uid);
                return RedirectToAction("index", new { id });
            }
            return RedirectToAction("index", "shop");
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStatus(int id, int status)
        {
            await service.ChangeOrderStatusAsync(id, (OrderStatus)status);
            return RedirectToAction("index", new { id });
        }

        [HttpPost]
        public async Task<IActionResult> CheckOut(int id)
        {
            await service.CheckOutAsync(id, client);
            return RedirectToAction("index", new { id });
        }
    }
}
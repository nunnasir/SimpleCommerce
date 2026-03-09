using Microsoft.AspNetCore.Mvc;
using SimpleCommerce.Contract;

namespace SimpleCommerce.Web.Areas.Admin.Controllers;

[Area("Admin")]
public class HomeController : Controller
{
    private readonly IOrderService _orderService;

    public HomeController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public async Task<IActionResult> Index()
    {
        var orders = await _orderService.GetAllOrdersAsync();
        ViewBag.TotalOrders = orders.Count;
        ViewBag.PendingCount = orders.Count(o => o.Status == "Pending");
        ViewBag.InProgressCount = orders.Count(o => o.Status == "InProgress");
        ViewBag.DeliveredCount = orders.Count(o => o.Status == "Delivered");
        return View();
    }
}


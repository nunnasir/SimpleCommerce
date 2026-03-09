using Microsoft.AspNetCore.Mvc;
using SimpleCommerce.Contract;

namespace SimpleCommerce.Web.Areas.Admin.Controllers;

[Area("Admin")]
public class OrderController : Controller
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public async Task<IActionResult> Index()
    {
        var orders = await _orderService.GetAllOrdersAsync();
        return View(orders);
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var order = await _orderService.GetOrderByIdAsync(id);
        if (order is null)
            return NotFound();
        return View(order);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(Guid id, string status)
    {
        var allowed = new[] { "Pending", "InProgress", "Delivered" };
        if (string.IsNullOrEmpty(status) || !allowed.Contains(status))
            return RedirectToAction(nameof(Details), new { id });

        await _orderService.UpdateStatusAsync(id, status);
        TempData["ToastMessage"] = "Order status updated.";
        TempData["ToastType"] = "success";
        return RedirectToAction(nameof(Details), new { id });
    }
}

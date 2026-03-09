using Microsoft.AspNetCore.Mvc;
using SimpleCommerce.BLL.Interfaces;
using SimpleCommerce.Contract;
using SimpleCommerce.Web.Models;

namespace SimpleCommerce.Web.Controllers;

public class CartController : Controller
{
    private readonly IProductService _productService;
    private readonly IOrderService _orderService;

    public CartController(IProductService productService, IOrderService orderService)
    {
        _productService = productService;
        _orderService = orderService;
    }

    public IActionResult Index()
    {
        var cart = HttpContext.Session.GetCart();
        return View(cart);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddToCart(Guid id, int quantity = 1)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product is null)
            return NotFound();

        var cart = HttpContext.Session.GetCart();
        var existing = cart.FirstOrDefault(x => x.ProductId == id);
        if (existing != null)
            existing.Quantity += quantity;
        else
            cart.Add(new CartItem
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                Quantity = quantity
            });

        HttpContext.Session.SetCart(cart);
        TempData["ToastMessage"] = $"{product.Name} added to cart.";
        TempData["ToastType"] = "success";

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Remove(Guid id)
    {
        var cart = HttpContext.Session.GetCart();
        var item = cart.FirstOrDefault(x => x.ProductId == id);
        if (item is null)
            return RedirectToAction(nameof(Index));

        cart.Remove(item);
        HttpContext.Session.SetCart(cart);
        TempData["ToastMessage"] = "Item removed from cart.";
        TempData["ToastType"] = "success";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateQuantity(Guid id, int quantity)
    {
        var cart = HttpContext.Session.GetCart();
        var item = cart.FirstOrDefault(x => x.ProductId == id);
        if (item is null)
            return RedirectToAction(nameof(Index));

        if (quantity < 1)
        {
            cart.Remove(item);
            TempData["ToastMessage"] = "Item removed from cart.";
        }
        else
        {
            item.Quantity = quantity;
            TempData["ToastMessage"] = "Quantity updated.";
        }

        TempData["ToastType"] = "success";
        HttpContext.Session.SetCart(cart);
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Checkout()
    {
        var cart = HttpContext.Session.GetCart();
        if (cart == null || !cart.Any())
            return RedirectToAction(nameof(Index));

        return View(new CheckoutViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Checkout(CheckoutViewModel model)
    {
        var cart = HttpContext.Session.GetCart();
        if (cart == null || !cart.Any())
            return RedirectToAction(nameof(Index));

        if (!ModelState.IsValid)
            return View(model);

        var items = cart.Select(c => new CartItemDto
        {
            ProductId = c.ProductId,
            ProductName = c.ProductName,
            Price = c.Price,
            Quantity = c.Quantity
        }).ToList();

        await _orderService.CreateOrderAsync(model.CustomerName, model.Mobile, model.Address, items);
        HttpContext.Session.ClearCart();

        TempData["ToastMessage"] = "Order placed successfully. Thank you!";
        TempData["ToastType"] = "success";
        return RedirectToAction(nameof(Index), "Home");
    }
}

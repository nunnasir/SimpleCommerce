using Microsoft.AspNetCore.Mvc;
using SimpleCommerce.BLL.Interfaces;
using SimpleCommerce.Web.Models;

namespace SimpleCommerce.Web.Controllers;

public class CartController : Controller
{
    private const string CartKey = "Cart";
    private readonly IProductService _productService;

    public CartController(IProductService productService)
    {
        _productService = productService;
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
}

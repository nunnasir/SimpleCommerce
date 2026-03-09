using Microsoft.AspNetCore.Mvc;
using SimpleCommerce.BLL.Interfaces;
using SimpleCommerce.Contract;

namespace SimpleCommerce.Web.Areas.Admin.Controllers;

[Area("Admin")]
public class ProductController(IProductService productService, IWebHostEnvironment environment) : Controller
{
    public async Task<IActionResult> Index()
    {
        var products = await productService.GetAllAsync();
        return View(products);
    }

    public IActionResult Create()
    {
        return View(new ProductDto());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductDto model, IFormFile? imageFile)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        if (imageFile is not null && imageFile.Length > 0)
        {
            var uploadsFolder = Path.Combine(environment.WebRootPath, "images", "products");
            Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            await using (var stream = System.IO.File.Create(filePath))
            {
                await imageFile.CopyToAsync(stream);
            }

            model.ImageUrl = $"/images/products/{fileName}";
        }

        await productService.CreateAsync(model);
        TempData["ToastMessage"] = "Product created successfully.";
        TempData["ToastType"] = "success";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var product = await productService.GetByIdAsync(id);
        if (product is null)
        {
            return NotFound();
        }

        return View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ProductDto model, IFormFile? imageFile)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        if (imageFile is not null && imageFile.Length > 0)
        {
            var uploadsFolder = Path.Combine(environment.WebRootPath, "images", "products");
            Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            await using (var stream = System.IO.File.Create(filePath))
            {
                await imageFile.CopyToAsync(stream);
            }

            model.ImageUrl = $"/images/products/{fileName}";
        }

        await productService.UpdateAsync(model);
        TempData["ToastMessage"] = "Product updated successfully.";
        TempData["ToastType"] = "success";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        var product = await productService.GetByIdAsync(id);
        if (product is null)
        {
            return NotFound();
        }

        return View(product);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await productService.DeleteAsync(id);
        TempData["ToastMessage"] = "Product deleted successfully.";
        TempData["ToastType"] = "success";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var product = await productService.GetByIdAsync(id);
        if (product is null)
        {
            return NotFound();
        }

        return View(product);
    }
}

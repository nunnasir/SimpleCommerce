using Microsoft.AspNetCore.Mvc;

namespace SimpleCommerce.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

using System.ComponentModel.DataAnnotations;

namespace SimpleCommerce.Web.Models;

public class CheckoutViewModel
{
    [Required(ErrorMessage = "Customer name is required")]
    [StringLength(100)]
    [Display(Name = "Customer Name")]
    public string CustomerName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Mobile number is required")]
    [StringLength(20)]
    [Display(Name = "Mobile")]
    public string Mobile { get; set; } = string.Empty;

    [Required(ErrorMessage = "Address is required")]
    [StringLength(500)]
    [Display(Name = "Address")]
    public string Address { get; set; } = string.Empty;
}

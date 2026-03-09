using System.Text.Json;
using SimpleCommerce.Web.Models;

namespace SimpleCommerce.Web.Controllers;

public static class SessionCartExtensions
{
    private const string CartKey = "Cart";

    public static List<CartItem> GetCart(this ISession session)
    {
        var json = session.GetString(CartKey);
        if (string.IsNullOrEmpty(json))
            return new List<CartItem>();
        try
        {
            return JsonSerializer.Deserialize<List<CartItem>>(json) ?? new List<CartItem>();
        }
        catch
        {
            return new List<CartItem>();
        }
    }

    public static void SetCart(this ISession session, List<CartItem> cart)
    {
        session.SetString(CartKey, JsonSerializer.Serialize(cart));
    }

    public static int GetCartItemCount(this ISession session)
    {
        return session.GetCart().Sum(x => x.Quantity);
    }
}

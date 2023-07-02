
using System;
using System.Collections.Generic;
using System.Text;
using VynilShop.Domain.DTO;

namespace VynilShop.Services.Interfaces
{
    public interface IShoppingCartService
    {
        ShoppingCartDto getShoppingCartInfo(string userId);
        bool deleteVynilFromShoppingCart(string userId, Guid id);
        bool orderNow(string userId);
    }
}

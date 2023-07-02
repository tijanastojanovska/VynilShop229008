
using System;
using System.Collections.Generic;
using System.Text;
using VynilShop.Domain.DomainModels;

namespace VynilShop.Services.Interfaces
{
    public interface IOrderService
    {
        List<Order> getAllOrders();
        List<Order> getAllOrdersByUser(string userId);

        Order getOrderDetails(Guid id);
    }
}

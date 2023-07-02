using System;
using System.Collections.Generic;
using System.Text;
using VynilShop.Domain.DomainModels;

namespace VynilShop.Repository.Interfaces
{
    public interface IOrderRepository
    {
        List<Order> getAllOrders();
        List<Order> getAllOrdersByUser(string UserId);
        Order getOrderDetails(Guid id);


    }
}


using System;
using System.Collections.Generic;
using System.Text;
using VynilShop.Domain.DomainModels;
using VynilShop.Repository.Interfaces;
using VynilShop.Services.Interfaces;

namespace VynilShop.Services.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            this._orderRepository = orderRepository;
        }

        public List<Order> getAllOrders()
        {
            return this._orderRepository.getAllOrders();
        }

        public List<Order> getAllOrdersByUser(string UserId)
        {
           return this._orderRepository.getAllOrdersByUser(UserId);
        }

        public Order getOrderDetails(Guid id)
        {
            return this._orderRepository.getOrderDetails(id);
        }
    }
}

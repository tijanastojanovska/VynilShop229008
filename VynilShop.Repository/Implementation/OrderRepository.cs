using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using VynilShop.Repository.Interfaces;
using VynilShop.Domain.DomainModels;
using VynilShop.Repository.Data;
using System.Linq;

namespace VynilShop.Repository.Implementation
{
    public class OrderRepository : IOrderRepository
    {

        private readonly ApplicationDbContext context;
        private DbSet<Order> entities;
        string errorMessage = string.Empty;

        public OrderRepository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<Order>();
        }


        public List<Order> getAllOrders()
        {
            return entities
                .Include(z => z.User)
                .Include(z => z.VynilInOrders)
                .Include("VynilInOrders.OrderedVynil")
                .ToList() ;
        }

        public List<Order> getAllOrdersByUser(string UserId)
        {
            return entities
                .Include(z => z.User)
                .Include(z => z.VynilInOrders)
                .Include("VynilInOrders.OrderedVynil")
                .Where(x => x.UserId == UserId)
                .ToList();
        }

        public Order getOrderDetails(Guid id)
        {
            return entities
               .Include(z => z.User)
               .Include(z => z.VynilInOrders)
               .Include("VynilInOrders.OrderedVynil")
               .FirstOrDefault(z => z.Id == id);
        }

    }
}

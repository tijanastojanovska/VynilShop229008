using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VynilShop.Domain.Identity;
using VynilShop.Repository.Data;
using VynilShop.Repository.Interfaces;

namespace VynilShop.Repository.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<VynilShopUser> entities;
        string errorMessage = string.Empty;

        public UserRepository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<VynilShopUser>();
        }
        public List<VynilShopUser> GetAll()
        {
            return entities.ToList();
        }

        public VynilShopUser Get(string id)
        {
            return entities
               .Include(z => z.UserCart)
               .Include("UserCart.VynilInShoppingCarts")
               .Include("UserCart.VynilInShoppingCarts.Vynil")
               .SingleOrDefault(s => s.Id == id);
        }
        public void Insert(VynilShopUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
            context.SaveChanges();
        }

        public void Update(VynilShopUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Update(entity);
            context.SaveChanges();
        }

        public void Delete(VynilShopUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            context.SaveChanges();
        }
    }
}

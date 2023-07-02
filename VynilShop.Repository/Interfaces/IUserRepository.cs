using System;
using System.Collections.Generic;
using System.Text;
using VynilShop.Domain.Identity;

namespace VynilShop.Repository.Interfaces
{
    public interface IUserRepository
    {
        List<VynilShopUser> GetAll();
        VynilShopUser Get(string id);
        void Insert(VynilShopUser entity);
        void Update(VynilShopUser entity);
        void Delete(VynilShopUser entity);
    }
}

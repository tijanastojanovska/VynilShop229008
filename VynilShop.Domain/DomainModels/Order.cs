
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VynilShop.Domain.Identity;

namespace VynilShop.Domain.DomainModels
{
    public class Order : BaseEntity
    {
        public string UserId { get; set; }
        public VynilShopUser User { get; set; }

        public IEnumerable<VynilInOrder> VynilInOrders { get; set; }
    }
}

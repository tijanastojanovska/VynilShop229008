using System.Collections.Generic;
using VynilShop.Domain.Identity;

namespace VynilShop.Domain.DomainModels
{
    public class ShoppingCart : BaseEntity
    {

        public string OwnerId { get; set; }
        public VynilShopUser Owner { get; set; }
        public virtual ICollection<VynilInShoppingCart> VynilInShoppingCarts { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VynilShop.Domain.DomainModels
{
    public class VynilInShoppingCart : BaseEntity
    {
        public Guid VynilId { get; set; }
        public Vynil Vynil { get; set; }
        public Guid ShoppingCartId { get; set; }
        public ShoppingCart ShoppingCart { get; set; }

        public int Quantity { get; set; }
    }
}

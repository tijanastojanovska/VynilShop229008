
using VynilShop.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VynilShop.Domain.DTO
{ 
    public class ShoppingCartDto
    {
        public List<VynilInShoppingCart> Vynils { get; set; }
        public double TotalPrice { get; set; }
    }
}

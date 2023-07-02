

using VynilShop.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VynilShop.Domain.DTO

{
    public class AddToShoppingCardDto
    {
        public Vynil SelectedVynil { get; set; }
        public Guid VynilId { get; set; }
        public int Quantity { get; set; }
    }
}

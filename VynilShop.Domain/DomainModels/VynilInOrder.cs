using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VynilShop.Domain.DomainModels
{
    public class VynilInOrder : BaseEntity
    {
        public Guid VynilId { get; set; }
        public Vynil OrderedVynil { get; set; }

        public Guid OrderId { get; set; }
        public Order UserOrder { get; set; }
        public int Quantity { get; set; }
    }
}

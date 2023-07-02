using System;
using System.Collections.Generic;
using System.Text;

namespace VynilShop.Domain.DomainModels
{
    public class Artist : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Vynil> Vynils { get; set; }
    }
}

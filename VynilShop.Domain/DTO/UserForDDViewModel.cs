using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VynilShop.Domain.DTO
{
    public class UserForDDViewModel
    {
        [Display(Name = "User")]
        public string Id { get; set; }
        public string FullName { get; set; }
    }
}

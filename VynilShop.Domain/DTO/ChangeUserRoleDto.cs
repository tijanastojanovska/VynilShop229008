using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using VynilShop.Domain.DomainModels;

namespace VynilShop.Domain.DTO
{
    public class ChangeUserRoleDto
    {
        [Display(Name = "User")]
        public string Id { get; set; }
         
        public EnumRoles Role { get; set; }
        public EnumRoles? CurrentUserRole { get; set; }
    }
}

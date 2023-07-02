using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using VynilShop.Domain.DomainModels;

namespace VynilShop.Domain.DTO
{
   public class VynilDto
    {
        public List<Vynil> Vynils { get; set; }
        [Display(Name ="Date od release")]
        public DateTime Date { get; set; }
        public EnumGenre? Genre { get; set; }
        public EnumRoles? CurrentUserRole { get; set; }
        [Display(Name = "Artist")]
        public Guid? ArtistId { get; set; }
    }
}

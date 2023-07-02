using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VynilShop.Domain.DomainModels
{
    public class Vynil : BaseEntity
    {
        [Required]
        [Display(Name ="Vynil Name")]
        public string VynilName { get; set; }
        [Required]
        [Display(Name = "Vynil Image")]
        public string VynilImage { get; set; }
        [Required]
        [Display(Name = "Short Vynil Description")]
        public string VynilDescription { get; set; }
        [Required]
        [Display(Name = "Date of release")]
        public DateTime VynilDate { get; set; }
      
        [Required]
        [Display(Name = "Price")]
        public int VynilPrice { get; set; }

        [Required]
        public EnumGenre Genre { get; set; }

        [Display(Name = "Rating")]
        public int VynilRating { get; set; }

        public string Songs { get; set; }

        public Artist Artist { get; set; }
        [Required]
        public Guid ArtistId { get; set; }

        public virtual ICollection<VynilInShoppingCart> VynilInShoppingCarts { get; set; }

        public IEnumerable<VynilInOrder> VynilInOrders { get; set; }

    }
}

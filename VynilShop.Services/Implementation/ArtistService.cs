using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VynilShop.Domain.DomainModels;
using VynilShop.Domain.DTO;
using VynilShop.Repository.Interfaces;
using VynilShop.Services.Interfaces;

namespace VynilShop.Services.Implementation
{
    public class ArtistService : IArtistService
    {
        private readonly IRepository<Artist> _artistRepository;

        public ArtistService(IRepository<Artist> artistRepository)
        {
            this._artistRepository = artistRepository;
        }

        public List<ArtistForDDViewModel> GetAllArtistsForDropdown()
        {
            List<Artist> artistDB = (List<Artist>)_artistRepository.GetAll();

            List<ArtistForDDViewModel> artists = artistDB.Select(x => new ArtistForDDViewModel
            {
                Id = x.Id, 
                FullName = $"{x.FirstName} {x.LastName}"

            }).ToList();

            return artists;
        }
    }
}

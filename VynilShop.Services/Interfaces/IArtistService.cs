using System;
using System.Collections.Generic;
using System.Text;
using VynilShop.Domain.DTO;

namespace VynilShop.Services.Interfaces
{
    public interface IArtistService
    {
        List<ArtistForDDViewModel> GetAllArtistsForDropdown();
    }
}

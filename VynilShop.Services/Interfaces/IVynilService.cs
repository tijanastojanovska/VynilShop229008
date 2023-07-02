using System;
using System.Collections.Generic;
using System.Text;
using VynilShop.Domain.DomainModels;
using VynilShop.Domain.DTO;

namespace VynilShop.Services.Interfaces
{
    public interface IVynilService
    {
        List<Vynil> GetAllVynils();
        Vynil GetDetailsForVynil(Guid? id);
        void CreateNewVynil(Vynil p);
        void UpdeteExistingVynil(Vynil p);
        AddToShoppingCardDto GetShoppingCartInfo(Guid? id);
        void DeleteVynil(Guid id);
        bool AddToShoppingCart(AddToShoppingCardDto item, string userID);
    }
}

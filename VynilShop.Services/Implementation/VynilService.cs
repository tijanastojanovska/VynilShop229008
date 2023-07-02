using Microsoft.Extensions.Logging;
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
    public class VynilService : IVynilService
    {
        private readonly IRepository<Vynil> _VynilRepository;
        private readonly IRepository<VynilInShoppingCart> _VynilInShoppingCartRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<VynilService> _logger;
        public VynilService(IRepository<Vynil> VynilRepository, ILogger<VynilService> logger, IRepository<VynilInShoppingCart> VynilInShoppingCartRepository, IUserRepository userRepository)
        {
            _VynilRepository = VynilRepository;
            _userRepository = userRepository;
            _VynilInShoppingCartRepository = VynilInShoppingCartRepository;
            _logger = logger;
        }

        public bool AddToShoppingCart(AddToShoppingCardDto item, string userID)
        {

            var user = this._userRepository.Get(userID);

            var userShoppingCard = user.UserCart;

            if (item.VynilId != null && userShoppingCard != null)
            {
                var Vynil = this.GetDetailsForVynil(item.VynilId);

                if (Vynil != null)
                {
                    VynilInShoppingCart itemToAdd = new VynilInShoppingCart
                    {
                        Id = Guid.NewGuid(),
                        Vynil = Vynil,
                        VynilId = Vynil.Id,
                        ShoppingCart = userShoppingCard,
                        ShoppingCartId = userShoppingCard.Id,
                        Quantity = item.Quantity
                    };

                    this._VynilInShoppingCartRepository.Insert(itemToAdd);
                    _logger.LogInformation("Vynil was successfully added into ShoppingCart");
                    return true;
                }
                return false;
            }
            _logger.LogInformation("Something was wrong. VynilId or UserShoppingCard may be unaveliable!");
            return false;
        }

        public void CreateNewVynil(Vynil p)
        {
            this._VynilRepository.Insert(p);
        }

        public void DeleteVynil(Guid id)
        {
            var Vynil = this.GetDetailsForVynil(id);
            this._VynilRepository.Delete(Vynil);
        }

        public List<Vynil> GetAllVynils()
        {
            _logger.LogInformation("GetAllVynils was called!");
            return this._VynilRepository.GetAll().ToList();
        }

        public Vynil GetDetailsForVynil(Guid? id)
        {
            return this._VynilRepository.Get(id);
        }

        public AddToShoppingCardDto GetShoppingCartInfo(Guid? id)
        {
            var Vynil = this.GetDetailsForVynil(id);
            AddToShoppingCardDto model = new AddToShoppingCardDto
            {
                SelectedVynil = Vynil,
                VynilId = Vynil.Id,
                Quantity = 1
            };

            return model;
        }

        public void UpdeteExistingVynil(Vynil p)
        {
            this._VynilRepository.Update(p);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VynilShop.Domain.DTO;
using VynilShop.Domain.Identity;
using VynilShop.Repository.Interfaces;
using VynilShop.Services.Interfaces;

namespace VynilShop.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }

        public List<UserForDDViewModel> GetAllUsersForDropdown()
        {
            List<VynilShopUser> userDb = (List<VynilShopUser>)_userRepository.GetAll();

            List<UserForDDViewModel> users = userDb.Select(x => new UserForDDViewModel
            {
                Id = x.Id,
                FullName = x.UserName

            }).ToList();

            return users;
        }
    }
}

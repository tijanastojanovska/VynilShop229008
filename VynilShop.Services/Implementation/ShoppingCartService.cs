
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
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IRepository<ShoppingCart> _shoppingCartRepositorty;
        private readonly IRepository<Order> _orderRepositorty;
        private readonly IRepository<VynilInOrder> _VynilInOrderRepositorty;
        private readonly IUserRepository _userRepository;
        private readonly IRepository<EmailMessage> _mailRepository;

        public ShoppingCartService(IRepository<EmailMessage> mailRepository, IRepository<ShoppingCart> shoppingCartRepository, IRepository<VynilInOrder> VynilInOrderRepositorty, IRepository<Order> orderRepositorty, IUserRepository userRepository)
        {
            _shoppingCartRepositorty = shoppingCartRepository;
            _userRepository = userRepository;
            _orderRepositorty = orderRepositorty;
            _VynilInOrderRepositorty = VynilInOrderRepositorty;
            _mailRepository = mailRepository;
        }

        public bool deleteVynilFromShoppingCart(string userId, Guid id)
        {
            if (!string.IsNullOrEmpty(userId) && id != null)
            {
              

                var loggedInUser = this._userRepository.Get(userId);

                var userShoppingCart = loggedInUser.UserCart;

                var itemToDelete = userShoppingCart.VynilInShoppingCarts.Where(z => z.VynilId.Equals(id)).FirstOrDefault();

                userShoppingCart.VynilInShoppingCarts.Remove(itemToDelete);

                this._shoppingCartRepositorty.Update(userShoppingCart);

                return true;
            }

            return false;
        }
        
        public ShoppingCartDto getShoppingCartInfo(string userId)
        {
            var loggedInUser = this._userRepository.Get(userId);

            var userShoppingCart = loggedInUser.UserCart;

            var AllVynils = userShoppingCart.VynilInShoppingCarts.ToList();

            var allVynilPrice = AllVynils.Select(z => new
            {
                VynilPrice = z.Vynil.VynilPrice,
                Quanitity = z.Quantity
            }).ToList();

            var totalPrice = 0;


            foreach (var item in allVynilPrice)
            {
                totalPrice += item.Quanitity * item.VynilPrice;
            }


            ShoppingCartDto scDto = new ShoppingCartDto
            {
                Vynils = AllVynils,
                TotalPrice = totalPrice
            };


            return scDto;

        }

        public bool orderNow(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
               

                var loggedInUser = this._userRepository.Get(userId);

                var userShoppingCart = loggedInUser.UserCart;

                EmailMessage mail = new EmailMessage();
                mail.MailTo = loggedInUser.Email;
                mail.Subject = "Successfully created order";
                mail.Status = false;

                Order order = new Order
                {
                    Id = Guid.NewGuid(),
                    User = loggedInUser,
                    UserId = userId
                };

                this._orderRepositorty.Insert(order);

                List<VynilInOrder> VynilInOrders = new List<VynilInOrder>();

                var result = userShoppingCart.VynilInShoppingCarts.Select(z => new VynilInOrder
                {
                    Id = Guid.NewGuid(),
                    VynilId = z.Vynil.Id,
                    OrderedVynil = z.Vynil,
                    OrderId = order.Id,
                    UserOrder = order,
                    Quantity = z.Quantity
                }).ToList();

                StringBuilder sb = new StringBuilder();

                var totalPrice = 0;

                sb.AppendLine("Your order is completed. The order conains: ");

                for (int i = 1; i <= result.Count(); i++)
                {
                    var item = result[i-1];

                    totalPrice += item.Quantity * item.OrderedVynil.VynilPrice;

                    sb.AppendLine(i.ToString() + ". " + item.OrderedVynil.VynilName + " with price of: " + item.OrderedVynil.VynilPrice + " and quantity of: " + item.Quantity);
                }

                sb.AppendLine("Total price: " + totalPrice.ToString());


                mail.Content = sb.ToString();


                VynilInOrders.AddRange(result);

                foreach (var item in VynilInOrders)
                {
                    this._VynilInOrderRepositorty.Insert(item);
                }

                loggedInUser.UserCart.VynilInShoppingCarts.Clear();

                this._userRepository.Update(loggedInUser);
                this._mailRepository.Insert(mail);

                return true;
            }
            return false;
        }

       
    }
}

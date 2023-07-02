
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using VynilShop.Domain.DomainModels;
using VynilShop.Domain.DTO;
using VynilShop.Domain.Idenitity;
using VynilShop.Domain.Identity;
using VynilShop.Repository.Interfaces;
using VynilShop.Services.Interfaces;

namespace VynilShop.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<VynilShopUser> userManager;
        private readonly SignInManager<VynilShopUser> signInManager;
        private readonly IUserService _userService;
       
        public AccountController(UserManager<VynilShopUser> userManager, SignInManager<VynilShopUser> signInManager, IUserService userService)
        {

            this.userManager = userManager;
            this.signInManager = signInManager;
            _userService = userService;
            
        }

        public IActionResult Register()
        {
            UserRegistrationDto model = new UserRegistrationDto();
            return View(model);
        }

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Register(UserRegistrationDto request)
        {
            if (ModelState.IsValid)
            {
                var userCheck = await userManager.FindByEmailAsync(request.Email);
                if (userCheck == null)
                {
                    var user = new VynilShopUser
                    {
                        UserName = request.Email,
                        NormalizedUserName = request.Email,
                        Email = request.Email,
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        UserCart = new ShoppingCart()
                    };
                    var result = await userManager.CreateAsync(user, request.Password);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        if (result.Errors.Count() > 0)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("message", error.Description);
                            }
                        }
                        return View(request);
                    }
                }
                else
                {
                    ModelState.AddModelError("message", "Email already exists.");
                    return View(request);
                }
            }
            return View(request);

        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            UserLoginDto model = new UserLoginDto();
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDto model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user != null && !user.EmailConfirmed)
                {
                    ModelState.AddModelError("message", "Email not confirmed yet");
                    return View(model);

                }
                if (await userManager.CheckPasswordAsync(user, model.Password) == false)
                {
                    ModelState.AddModelError("message", "Invalid credentials");
                    return View(model);

                }

                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, true);

                if (result.Succeeded)
                {
                    await userManager.AddClaimAsync(user, new Claim("UserRole", "Admin"));
                    return RedirectToAction("Index", "Home");
                }
                else if (result.IsLockedOut)
                {
                    return View("AccountLocked");
                }
                else
                {
                    ModelState.AddModelError("message", "Invalid login attempt");
                    return View(model);
                }
            }
            return View(model);
        }
        [Authorize]
        public async Task<IActionResult> ChangeRoleOfUser()
        {
            var currentUser = await userManager.GetUserAsync(HttpContext.User);
            var user = new ChangeUserRoleDto();

            user.CurrentUserRole = currentUser.Role;
            ViewBag.Users = _userService.GetAllUsersForDropdown();

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeRoleOfUser(ChangeUserRoleDto toChange)
        {
            var user = await userManager.FindByIdAsync(toChange.Id); //go naoga korisnikot
            user.Role = toChange.Role; //promena na uloga
            var result = await userManager.UpdateAsync(user); //update na user
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Vynils");
            }
            else
            {
                return View(user);
            }
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}

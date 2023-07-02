using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using VynilShop.Domain.DomainModels;
using VynilShop.Domain.DTO;
using VynilShop.Domain.Identity;
using VynilShop.Services.Interfaces;

namespace VynilShop.Controllers
{
    public class VynilsController : Controller
    {
        private readonly IVynilService _VynilService;
        private readonly IArtistService _ArtistService;
        private readonly UserManager<VynilShopUser> _userManager;
        public VynilsController(IVynilService VynilServicet, UserManager<VynilShopUser> userManager, IArtistService ArtistService)
        {
            _VynilService = VynilServicet;
            _userManager = userManager;
            _ArtistService = ArtistService;
        }

        // GET: Vynils
        [Authorize]
        public async Task<IActionResult> Index()
        {
            ViewBag.Artists = _ArtistService.GetAllArtistsForDropdown();

            var user = await _userManager.GetUserAsync(HttpContext.User);
            var toFilter = new VynilDto
            {
                Vynils = this._VynilService.GetAllVynils(),
                Date = DateTime.Now,
                CurrentUserRole = user.Role
            };
            return View(toFilter);
        }
        [HttpPost]
        public async Task<IActionResult> Index(VynilDto toFilter)
        {
            ViewBag.Artists = _ArtistService.GetAllArtistsForDropdown();
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var Vynils = _VynilService.GetAllVynils()
                .Where(z => z.ArtistId == toFilter.ArtistId
                && z.Genre == toFilter.Genre).ToList(); 
            var filtered = new VynilDto
            {
                Vynils = Vynils,
                Date = toFilter.Date,
                ArtistId = toFilter.ArtistId,
                Genre = toFilter.Genre,
                CurrentUserRole = user.Role
            };
            return View(filtered); //vrati filtirani
        }


        public IActionResult AddVynilToCard(Guid? id)
        {
            var model = this._VynilService.GetShoppingCartInfo(id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddVynilToCard([Bind("VynilId", "Quantity")] AddToShoppingCardDto item)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = this._VynilService.AddToShoppingCart(item, userId);

            if (result)
            {
                return RedirectToAction("Index", "Vynils");
            }

            return View(item);
        }

        // GET: Vynils/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Vynil = this._VynilService.GetDetailsForVynil(id);

            if (Vynil == null)
            {
                return NotFound();
            }

            return View(Vynil);
        }

        // GET: Vynils/Create
        public IActionResult Create()
        {
            ViewBag.Artists = _ArtistService.GetAllArtistsForDropdown();
            return View();
        }

        // POST: Vynils/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,VynilName,VynilImage,VynilDescription,VynilDate,VynilPrice,Genre,ArtistId,VynilRating")] Vynil Vynil)
        {
            if (ModelState.IsValid)
            {
                this._VynilService.CreateNewVynil(Vynil);
                return RedirectToAction(nameof(Index));
            }
            return View(Vynil);
        }

        public IActionResult Edit(Guid? p)
        {
            if (p == null)
            {
                return NotFound();
            }

            var Vynil = this._VynilService.GetDetailsForVynil(p);

            if (Vynil == null)
            {
                return NotFound();
            }
            return View(Vynil);
        }

        
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("Id,VynilName,VynilImage,VynilDescription,VynilDate,VynilPrice,Genre,ArtistId,VynilRating")] Vynil Vynil)
        {
            if (id != Vynil.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    this._VynilService.UpdeteExistingVynil(Vynil);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VynilExists(Vynil.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(Vynil);
        }

        
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Vynil = this._VynilService.GetDetailsForVynil(id);

            if (Vynil == null)
            {
                return NotFound();
            }

            return View(Vynil);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            this._VynilService.DeleteVynil(id);
            return RedirectToAction(nameof(Index));
        }

        private bool VynilExists(Guid id)
        {
            return this._VynilService.GetDetailsForVynil(id) != null;
        }
      
        public async Task<FileContentResult> ExportAllVynils(VynilDto toFilter)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if(user.Role != EnumRoles.Administrator)
            {
                throw new Exception("Only administrators can export Vynils");
            }
            string fileName = "Vynils.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("All Vynils");

                worksheet.Cell(1, 1).Value = "Vynil Id";
                worksheet.Cell(1, 2).Value = "Vynil Name";
                worksheet.Cell(1, 3).Value = "Vynil Genre";
                worksheet.Cell(1, 4).Value = "Vynil Price";
                List<Vynil> result = new List<Vynil>();
              
                if (!toFilter.Genre.HasValue)
                {
                   result = _VynilService.GetAllVynils();
                }

                else
                {
                   result = _VynilService.GetAllVynils().Where(z => z.Genre == toFilter.Genre.Value).ToList();
                }

                for (int i = 1; i <= result.Count(); i++)
                {
                    var item = result[i - 1];

                    worksheet.Cell(i + 1, 1).Value = item.Id.ToString();
                    worksheet.Cell(i + 1, 2).Value = item.VynilName;
                    worksheet.Cell(i + 1, 3).Value = item.Genre;
                    worksheet.Cell(i + 1, 4).Value = item.VynilPrice;

                    //for (int p = 0; p < item.VynilInOrders.Count(); p++)
                    //{
                    //    worksheet.Cell(1, p + 3).Value = "Vynil-" + (p + 1);
                    //    worksheet.Cell(i + 1, p + 3).Value = item.VynilInOrders.ElementAt(p).OrderedVynil.VynilName;  
                    //}
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(content, contentType, fileName);
                }

            }
        }
    }
}

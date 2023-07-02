using ClosedXML.Excel;
using GemBox.Document;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VynilShop.Domain.DomainModels;
using VynilShop.Domain.Identity;
using VynilShop.Repository.Data;
using VynilShop.Services.Interfaces;

namespace VynilShop.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IOrderService _orderService;
        private readonly UserManager<VynilShopUser> _userManager;


        public OrderController(ApplicationDbContext context, IOrderService orderService, UserManager<VynilShopUser> userManager)
        {

            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
            _context = context;
            _orderService = orderService;
            _userManager = userManager;

        }

        // GET: Orders
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if(currentUser.Role == EnumRoles.Administrator)
            {
                return View(this._orderService.getAllOrders());
            }
            else
            {
                return View(this._orderService.getAllOrdersByUser(currentUser.Id));
            }
            
        }

        public async Task<IActionResult> Details(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = _orderService.getOrderDetails(id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        public FileContentResult CreateInvoice(Guid id)
        {
            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Invoice.docx");
            var document = DocumentModel.Load(templatePath);

            Order order = _orderService.getOrderDetails(id);

            document.Content.Replace("{{OrderNumber}}", order.Id.ToString());
            document.Content.Replace("{{UserName}}", order.User.UserName);

            StringBuilder sb = new StringBuilder();

            var totalPrice = 0.0;

            foreach (var item in order.VynilInOrders)
            {
                totalPrice += item.Quantity * item.OrderedVynil.VynilPrice;
                sb.AppendLine(item.OrderedVynil.VynilName + " with quantity of: " + item.Quantity + " and price of: " + item.OrderedVynil.VynilPrice + "$");
            }


            document.Content.Replace("{{VynilList}}", sb.ToString());
            document.Content.Replace("{{TotalPrice}}", totalPrice.ToString() + "$");


            var stream = new MemoryStream();

            document.Save(stream, new PdfSaveOptions());

            return File(stream.ToArray(), new PdfSaveOptions().ContentType, "ExportInvoice.pdf");
        }

        public async Task<FileContentResult> ExportAllOrders()
        {
            string fileName = "Orders.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("All Orders");

                worksheet.Cell(1, 1).Value = "Order Id";
                worksheet.Cell(1, 2).Value = "Costumer Email";
                List<Order> orders;
                var currentUser = await _userManager.GetUserAsync(HttpContext.User);
                if (currentUser.Role == EnumRoles.Administrator)
                {
                    orders = _orderService.getAllOrders();
                }
                else
                {
                    orders = _orderService.getAllOrdersByUser(currentUser.Id);
                }
               

                for (int i = 1; i <= orders.Count(); i++)
                {
                    var item = orders[i - 1];

                    worksheet.Cell(i + 1, 1).Value = item.Id.ToString();
                    worksheet.Cell(i + 1, 2).Value = item.User.Email;

                    for (int p = 0; p < item.VynilInOrders.Count(); p++)
                    {
                        worksheet.Cell(1, p + 3).Value = "Order-" + (p + 1);
                        worksheet.Cell(i + 1, p + 3).Value = item.VynilInOrders.ElementAt(p).OrderedVynil.VynilName;
                    }
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

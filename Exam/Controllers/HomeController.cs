
using Exam.Areas.Identity.Data;
using Exam.Data;
using Exam.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Exam.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        SignInManager<User> _signInManager;
        
        IWebHostEnvironment _hostingEnvironment;
        Context _context;
        UserManager<User> _userManager;
        private readonly IEmailSender _emailSender;
        public HomeController(ILogger<HomeController> logger,  IEmailSender emailSender,
        UserManager<User> userManager, SignInManager<User> signInManager, Context context, 
        IWebHostEnvironment hostingEnvironment)
        {
            _logger = logger;
            _signInManager = signInManager;
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
            _emailSender = emailSender;
    }
       
        public IActionResult Index()
        {
            if (User.IsInRole("Admin")) {
                return View(_context.Users.Where(x => x.Email != "2@gmal.com"));
            }
            else
            {
                return RedirectToAction("Index", "Orders");
            }
        }
       
        public IActionResult Create()
        {
            return View();
        }
        public async Task<IActionResult> IndexOrders(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

          // var master = _context.Users.Where(x => x.UserId == id);
            var orders = await _context.orders.Include(x=>x.user)
                .Where(z=>z.user.Id==id)
                .ToListAsync();
           
            return View(orders);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UploadFiles(MasterViewModel model)
        {
            // проверка корректности валидации
            if (ModelState.IsValid)
            {
                // перебрать коллекцию полученных файлов
                foreach (IFormFile file in model.files)
                {
                    // проверка наличия файла
                    if (file != null)
                    {
                        var savePath = _hostingEnvironment.WebRootPath + "/Files/" + file.FileName;
                      
                        // сохранение файла
                        using (var fileStream = new FileStream(savePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                        User masterViewModel = new User()
                        {
                            City = model.City,
                            DateofBirth = model.DateofBirth,
                            Education = model.Education,
                            Email = model.email,
                            ImagePath = "Files/" + file.FileName,
                            Phone = model.Phone,
                            Relationship = model.Relationship,
                            Language = model.Language,
                            UserName = model.email,
                            Name=model.Name

                        };
                        var result = await _userManager.CreateAsync(masterViewModel,model.password);
                    string  returnUrl = Url.Content("~/");
                        if (result.Succeeded) {
                            _logger.LogInformation("User created a new account with password.");

                            var code = await _userManager.GenerateEmailConfirmationTokenAsync(masterViewModel);
                            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                            var callbackUrl = Url.Page(
                                "/Account/ConfirmEmail",
                                pageHandler: null,
                                values: new { area = "Identity", userId = masterViewModel.Id, code = code, returnUrl = returnUrl },
                                protocol: Request.Scheme);

                            await _emailSender.SendEmailAsync(masterViewModel.Email, "Confirm your email",
                                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                           
                            _context.Add(masterViewModel);

                        }
                       
                    }

                }
                await _context.SaveChangesAsync();
              
            }
            return View();


        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var currentmaster = _context.Users.FirstOrDefault(x => x.Email == User.Identity.Name);

            if (currentmaster == null)
            {
                return NotFound();
            }
            MasterViewModel userViewModel = new MasterViewModel()
            {
                City = currentmaster.City,
                DateofBirth = currentmaster.DateofBirth
            };
            return View(userViewModel);
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.orders
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.orders.FindAsync(id);
            _context.orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Complete(int id)
        {
            _context.orders.FirstOrDefault(x => x.OrderId == id).Completed = true;
            _context.orders.FirstOrDefault(x => x.OrderId == id).End = DateTime.Now;
            _context.SaveChanges();
            return RedirectToAction(nameof(IndexOrders));
        }
        public IActionResult UnComplete(int id)
        {
            _context.orders.FirstOrDefault(x => x.OrderId == id).Completed = false;
            _context.orders.FirstOrDefault(x => x.OrderId == id).End = null;
            _context.SaveChanges();
            return RedirectToAction(nameof(IndexOrders));
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return LocalRedirect("/Identity/Account/Login");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

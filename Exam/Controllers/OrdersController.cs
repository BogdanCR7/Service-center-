using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Exam.Data;
using Exam.Models;

namespace Exam.Controllers
{
    public class OrdersController : Controller
    {
        private readonly Context _context;

        public OrdersController(Context context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Admin")) return View(await _context.orders.ToListAsync());
            else return View(_context.orders.Where(x => x.user.Email == User.Identity.Name).ToList());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: Orders/Create
        public IActionResult Create()
        {
            return View(new OrderCreationViewModel());
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderCreationViewModel order)
        {
            if (ModelState.IsValid)
            {
                Client client = _context.clients.Include(x => x.orders).FirstOrDefault(x => x.PassportNumber == order.PassportNumber);
                Good good = _context.goods.Include(x => x.Category).FirstOrDefault(x => x.SerialNumber == order.SerialNumber);
                if (client==null||good==null)
                {
                    return View(order);
                }
                client.Adress = order.Adress;
                client.Name = order.Name;
                client.PassportNumber = order.PassportNumber;
                client.PhoneNumber = order.PhoneNumber;
                client.YearBirth = order.YearBirth;
                _context.Update(client);
                        
               

                good.Brand = order.Brand;
                good.Category = _context.categories.FirstOrDefault(x => x.category == order.Category);
                good.Model = order.Model;
                good.Price = order.Price;
                good.YearOfIssue = order.YearOfIssue;
                good.SerialNumber = order.SerialNumber;
                _context.Update(good);

               

                _context.SaveChanges();

                Order new_order = new Order
                {
                    user = _context.Users.FirstOrDefault(x => x.Email == User.Identity.Name),
                    Start = DateTime.Now,
                    Wage = order.Wage,
                    client = client,
                    goods = good,
                };

                _context.Add(new_order);
                client.orders.Add(new_order);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,Start,End,Wage")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
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
            return View(order);
        }

        // GET: Orders/Delete/5
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

        private bool OrderExists(int id)
        {
            return _context.orders.Any(e => e.OrderId == id);
        }


        [HttpPost]
        public JsonResult CheckSerialNumber(string id)
        {
            Good good = _context.goods.Include(x => x.Category).FirstOrDefault(x => x.SerialNumber == id);
            if (good == null) return Json(null);
            return Json(good);
        }


        [HttpPost]
        public JsonResult CheckPassport(string id)
        {
            Client good = _context.clients.FirstOrDefault(x => x.PassportNumber == id);
            if (good == null) return Json(null);
            return Json(good);
        }

        public IActionResult Complete(int id)
		{
            _context.orders.FirstOrDefault(x => x.OrderId == id).Completed = true;
            _context.orders.FirstOrDefault(x => x.OrderId == id).End = DateTime.Now;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult UnComplete(int id)
        {
            _context.orders.FirstOrDefault(x => x.OrderId == id).Completed = false;
            _context.orders.FirstOrDefault(x => x.OrderId == id).End = null;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Exam.Data;
using Exam.Models;
using Exam.Areas.Identity.Data;

namespace Exam.Controllers
{
    public class ClientsController : Controller
    {
        private readonly Context _context;

        public ClientsController(Context context)
        {
            _context = context;
            User claims = new User();
            
        }

        // GET: Clients
        public async Task<IActionResult> Index()
        {
            return View(await _context.clients.ToListAsync());
        }

        // GET: Clients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.clients
                .FirstOrDefaultAsync(m => m.ClientsId == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // GET: Clients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClientsId,PassportNumber,Name,Adress,PhoneNumber,YearBirth")] Client client)
        {
            if (ModelState.IsValid)
            {
                Client elem = _context.clients.Where(x => x.PassportNumber == client.PassportNumber).FirstOrDefault();
                if (elem == null)
                {
                    _context.Add(client);

                }
                else
                {
                    elem.Adress = client.Adress;
                    elem.Name = client.Name;
                    elem.PassportNumber = client.PassportNumber;
                    elem.PhoneNumber = client.PhoneNumber;
                    elem.YearBirth = client.YearBirth;
                    _context.Update(client);
                    _context.Update(elem);
                }
               
               
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client =  _context.clients.Where(x=>x.PassportNumber==id).FirstOrDefault();
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ClientsId,PassportNumber,Name,Adress,PhoneNumber,YearBirth")] Client client)
        {
            if (id != client.PassportNumber)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(client);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientExists(client.ClientsId))
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
            return View(client);
        }

        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.clients
                .FirstOrDefaultAsync(m => m.ClientsId == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var client = _context.clients.Where(x=>x.PassportNumber==id).FirstOrDefault();
            _context.clients.Remove(client);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClientExists(int id)
        {
            return _context.clients.Any(e => e.ClientsId == id);
        }
    }
}

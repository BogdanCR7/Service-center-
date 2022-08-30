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
    public class GoodsController : Controller
    {
        private readonly Context _context;

        public GoodsController(Context context)
        {
            _context = context;
        }

        // GET: Goods
        public async Task<IActionResult> Index()
        {
            return View(await _context.goods.Include(x=>x.Category).ToListAsync());
        }

        // GET: Goods/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var good = await _context.goods
                .FirstOrDefaultAsync(m => m.GoodsId == id);
            if (good == null)
            {
                return NotFound();
            }

            return View(good);
        }

        // GET: Goods/Create
        public IActionResult Create()
        {
            ViewBag.selections = _context.categories.Select(x=>x.category).ToList();
            return View();
        }

        // POST: Goods/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Good good)
        {
            if (ModelState.IsValid)
            {
                Good elem= _context.goods.Where(x => x.SerialNumber == good.SerialNumber).FirstOrDefault();
                if(elem ==null)
                {
                    good.Category = _context.categories.Where(x => x.category.Equals(good.Category.category)).FirstOrDefault();
                    _context.Add(good);
                   
                }
                else
                {
                    elem.Brand = good.Brand;
                    elem.Category = _context.categories.FirstOrDefault(x => x.category == good.Category.category);
                    elem.Model = good.Model;
                    elem.Price = good.Price;
                    elem.YearOfIssue = good.YearOfIssue;
                    elem.SerialNumber = good.SerialNumber;
                    _context.Update(elem);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(good);
        }

        // GET: Goods/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
           
            var good =  _context.goods.Include(x=>x.Category).Where(y=>y.GoodsId==id).FirstOrDefault();
            if (good == null)
            {
                return NotFound();
            }
            ViewBag.selections = _context.categories.Select(x => x.category).ToList();
            ViewBag.selected = good.Category.category;
            return View(good);
        }

        // POST: Goods/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  Good good)
        {
            if (id != good.GoodsId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    good.Category = _context.categories.Where(x => x.category.Equals(good.Category.category)).FirstOrDefault();
                    _context.Update(good);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GoodExists(good.GoodsId))
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
            return View(good);
        }

        // GET: Goods/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var good = await _context.goods
                .FirstOrDefaultAsync(m => m.GoodsId == id);
            if (good == null)
            {
                return NotFound();
            }

            return View(good);
        }

        // POST: Goods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var good = await _context.goods.FindAsync(id);

            ;
            _context.orders.RemoveRange(_context.orders.Include(y => y.goods).Where(x => x.goods == good).ToList());
            _context.goods.Remove(good);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GoodExists(int id)
        {
            return _context.goods.Any(e => e.GoodsId == id);
        }
    }
}

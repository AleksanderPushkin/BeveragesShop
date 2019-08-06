using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BeveragesShop.Models;

namespace BeveragesShop.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly BeveragesShopContext _context;

        public PaymentsController(BeveragesShopContext context)
        {
            _context = context;
        }

        // GET: Payments
        public async Task<IActionResult> Index()
        {
            return View(await _context.Payments.ToListAsync());
        }

        // GET: Payments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payments = await _context.Payments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (payments == null)
            {
                return NotFound();
            }

            return View(payments);
        }

        // GET: Payments/Create
        public IActionResult Create()
        {
            Payments payments = new Payments();
            payments.Coins = _context.Coins.AsNoTracking().ToList();
            payments.Beverages = _context.Beverages.AsNoTracking().ToList();
            return View(payments);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Change(Payments pay)
        {
            if (ModelState.IsValid)
            {
                return View(pay);
            }


            return View(pay);
        }

        private void Populate()
        {
            var coinsQuery = from d in _context.Coins
                             orderby d.Title
                             select d;
            ViewBag.CoinsQuery = coinsQuery.AsNoTracking().ToListAsync();


        }

        // POST: Payments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Payments payments)
        {
            if (ModelState.IsValid)
            {
                Payments p = payments;
                p.Beverages = _context.Beverages.AsNoTracking().ToList();
                var beverage = await _context.Beverages.FirstOrDefaultAsync(m => m.Id == payments.BeverageId);
                if (beverage.Qty - 1 < 0)
                {
                    ViewBag["Error"] = "Error: Отсутствет напиток";
                    return View(payments);
                }
                beverage.Qty--;

                foreach (var coin in payments.Coins)
                {
                    coin.Count = coin.Count + coin.CurrentCount;
                    coin.CurrentCount = 0;
                }

                _context.Update(beverage);
                _context.Add(payments);
                //считаю сдачу
                int change = payments.Change;
                int count = payments.Coins.Count;
                foreach (var coin in payments.Coins.OrderByDescending(t => t.Cost))
                {
                    if (coin.Count >= 0)
                    {
                        int c = change / coin.Cost;
                        if (c == 0) continue;
                        if (c <= coin.Count)
                        {
                            change = change - c * coin.Cost;
                            coin.Count = coin.Count - c;
                            coin.CurrentCount = c;
                        }
                        else
                        {
                            change = change - (coin.Count * coin.Cost);
                            coin.Count = 0;
                            coin.CurrentCount = coin.Count;
                        }
                        _context.Attach(coin);
                        _context.Entry(coin).State = EntityState.Modified;
                    }
                }
                if (change > 0)
                {
                    ModelState.Clear();
                    ModelState.AddModelError("Error", "Error: Нельзя дать сдачу");
                    ViewBag.Error = "Error: Нельзя дать сдачу";
                   
                    return View(p);
                }

                Payments payments1 = new Payments();
                payments1.Coins = _context.Coins.AsNoTracking().ToList();
                payments1.Beverages = _context.Beverages.AsNoTracking().ToList();
                payments1.Money = payments.Change;
                payments1.Change = payments.Change;
                foreach (var coin in payments.Coins)
                {
                    var c = payments1.Coins.Where(id => id.Id == coin.Id && id.Cost == coin.Cost).FirstOrDefault();
                    if (c == null)
                    {
                        ViewBag.Error = "Error: Произошла ошибка, пожалуйста, получите сдачу и начните работу сначала";
                        return View(p);
                    }
                    c.CurrentCount = coin.CurrentCount;
                    c.Count = coin.Count;
                }


                await _context.SaveChangesAsync();
                ModelState.Clear();
                return View("Create", payments1);


            }

            return View(payments);
        }

        // GET: Payments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payments = await _context.Payments.FindAsync(id);
            if (payments == null)
            {
                return NotFound();
            }
            return View(payments);
        }

        // POST: Payments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BeverageId,Count,Money,Change,Price")] Payments payments)
        {
            if (id != payments.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(payments);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaymentsExists(payments.Id))
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
            return View(payments);
        }

        // GET: Payments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payments = await _context.Payments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (payments == null)
            {
                return NotFound();
            }

            return View(payments);
        }

        // POST: Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var payments = await _context.Payments.FindAsync(id);
            _context.Payments.Remove(payments);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PaymentsExists(int id)
        {
            return _context.Payments.Any(e => e.Id == id);
        }
    }
}

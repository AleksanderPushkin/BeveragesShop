using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BeveragesShop.Models;
using Microsoft.AspNetCore.Authorization;
using BeveragesShop.Repository;

namespace BeveragesShop.Controllers
{
    //[Authorize(Policy ="AdminKey")]
    public class CoinsController : Controller
    {
        private readonly IRepositoryWrapper _repoWrapper;

        public CoinsController(IRepositoryWrapper context)
        {
            _repoWrapper = context;
        }

        // GET: Coins
        public async Task<IActionResult> Index()
        {
            return View( _repoWrapper.Coins.FindAll());
        }

        // GET: Coins/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coins =  _repoWrapper.Coins
                .GetById(id.Value);
            if (coins == null)
            {
                return NotFound();
            }

            return View(coins);
        }

        // GET: Coins/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Coins/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Cost,Count,IsActive,MaxQty")] Coins coins)
        {
            if (ModelState.IsValid)
            {
                _repoWrapper.Coins.Create(coins);
                 _repoWrapper.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(coins);
        }

        // GET: Coins/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coins =  _repoWrapper.Coins.GetById(id.Value);
            if (coins == null)
            {
                return NotFound();
            }
            return View(coins);
        }

        // POST: Coins/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Cost,Count,IsActive,MaxQty")] Coins coins)
        {
            if (id != coins.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _repoWrapper.Coins.Update(coins);
                     _repoWrapper.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CoinsExists(coins.Id))
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
            return View(coins);
        }

        // GET: Coins/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coins =  _repoWrapper.Coins
                .GetById(id.Value);
            if (coins == null)
            {
                return NotFound();
            }
            return View(coins);
        }

        // POST: Coins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var coins =  _repoWrapper.Coins.GetById(id);
            _repoWrapper.Coins.Delete(coins);
             _repoWrapper.Save();
            return RedirectToAction(nameof(Index));
        }

        private bool CoinsExists(int id)
        {
            return _repoWrapper.Coins.FindByCondition(t=>t.Id ==id).Any(e => e.Id == id);
        }
    }
}

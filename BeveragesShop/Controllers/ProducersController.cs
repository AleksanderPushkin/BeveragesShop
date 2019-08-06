using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BeveragesShop.Models;
using BeveragesShop.Repository;

namespace BeveragesShop.Controllers
{
    public class ProducersController : Controller
    {
        private IRepositoryWrapper _repoWrapper;

        public ProducersController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        // GET: Producers
        public async Task<IActionResult> Index()
        {
            return View( _repoWrapper.Producers.FindAll());
        }

        // GET: Producers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producers =  _repoWrapper.Producers.GetById(id.Value);
            if (producers == null)
            {
                return NotFound();
            }

            return View(producers);
        }

        // GET: Producers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Producers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title")] Producers producers)
        {
            if (ModelState.IsValid)
            {
                _repoWrapper.Producers.Create(producers);
                 _repoWrapper.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(producers);
        }

        // GET: Producers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producers = _repoWrapper.Producers.GetById(id.Value);
            if (producers == null)
            {
                return NotFound();
            }
            return View(producers);
        }

        // POST: Producers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title")] Producers producers)
        {
            if (id != producers.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _repoWrapper.Producers.Update(producers);
                     _repoWrapper.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProducersExists(producers.Id))
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
            return View(producers);
        }

        // GET: Producers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producers = _repoWrapper.Producers
              .GetById(id.Value);
            if (producers == null)
            {
                return NotFound();
            }

            return View(producers);
        }

        // POST: Producers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var producers = _repoWrapper.Producers.GetById(id);
            _repoWrapper.Producers.Delete(producers);
             _repoWrapper.Save();
            return RedirectToAction(nameof(Index));
        }

        private bool ProducersExists(int id)
        {
            return _repoWrapper.Producers.FindAll().Any(e => e.Id == id);
        }
      
    }
}

using System.Linq;
using System.Threading.Tasks;
using BeveragesShop.Models;
using System.IO;

namespace BeveragesShop.Controllers
{
    public class BeveragesController : Controller
    {
        private readonly BeveragesShopContext _context;

        public BeveragesController(BeveragesShopContext context)
        {
            _context = context;
        }

        // GET: Beverages
        public async Task<IActionResult> Index()
        {
            return View(await _context.Beverages
                .Include(c => c.Producer)
                .AsNoTracking()
                .ToListAsync());
        }

        // GET: Beverages
        public async Task<IActionResult> Grid()
        {
            return View(await _context.Beverages
                .Include(c => c.Producer)
                .AsNoTracking()
                .ToListAsync());
        }

        // GET: Beverages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var beverages = await _context.Beverages
                .Include(c => c.Producer)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (beverages == null)
            {
                return NotFound();
            }

            return View(beverages);
        }

        // GET: Beverages/Create
        public IActionResult Create()
        {
            PopulateProducersDropDownList();
            return View();
        }

        private void PopulateProducersDropDownList(object selectedProducer = null)
        {
            var dproducersQuery = from d in _context.Producers
                                   orderby d.Title
                                   select d;
            ViewBag.ProducerId = new SelectList(dproducersQuery.AsNoTracking(), "Id", "Title", selectedProducer);

        }

        // POST: Beverages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BeveragesCreateViewModel beverages)
        {
            if (ModelState.IsValid)
            {
                var _beverages = new Beverages
                {
                    Title = beverages.Title,
                    Description = beverages.Description,
                    Price = beverages.Price,
                    ProducerId = beverages.ProducerId,
                    Category = beverages.Category,
                    Qty = beverages.Qty
                };
                using (var memoryStream = new MemoryStream())
                {
                    if (beverages.Image != null)
                    {
                        await beverages.Image.CopyToAsync(memoryStream);
                        _beverages.Image = memoryStream.ToArray();
                    }
                }
                _context.Add(_beverages);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(beverages);
        }

        // GET: Beverages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var beverages = await _context.Beverages
                .Include(c=>c.Producer)
                .AsNoTracking()
                .FirstOrDefaultAsync(m=> m.Id == id);
            if (beverages == null)
            {
                return NotFound();
            }
            PopulateProducersDropDownList(beverages.Producer);
            var _beverages = new BeveragesCreateViewModel
            {
                Title = beverages.Title,
                Description = beverages.Description,
                Price = beverages.Price,
                ProducerId =beverages.ProducerId.Value,
                Category = beverages.Category,
                Qty = beverages.Qty,
            };
            return View(_beverages);
        }

        // POST: Beverages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BeveragesCreateViewModel beverages)
        {
            if (id != beverages.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var _beverages = await _context.Beverages.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);

                    _beverages.Title = beverages.Title;
                    _beverages.Description = beverages.Description;
                    _beverages.Price = beverages.Price;
                    _beverages.ProducerId = beverages.ProducerId;
                    _beverages.Category = beverages.Category;
                    _beverages.Qty = beverages.Qty;
           
                    using (var memoryStream = new MemoryStream())
                    {
                        if (beverages.Image != null)
                        {
                            await beverages.Image.CopyToAsync(memoryStream);
                            _beverages.Image = memoryStream.ToArray();
                        }
                    }
                    _context.Attach(_beverages);
                    _context.Entry(_beverages).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BeveragesExists(beverages.Id))
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
            return View(beverages);
        }

        // GET: Beverages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var beverages = await _context.Beverages
                .Include(c => c.Producer)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (beverages == null)
            {
                return NotFound();
            }

            return View(beverages);
        }

        // POST: Beverages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var beverages = await _context.Beverages.FindAsync(id);
            _context.Beverages.Remove(beverages);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BeveragesExists(int id)
        {
            return _context.Beverages.Any(e => e.Id == id);
        }
    }
}

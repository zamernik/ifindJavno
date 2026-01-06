using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using web.Data;
using web.Models;
using System.Globalization;

namespace web.Controllers
{
    [Authorize]
    public class DogodkiController : Controller
    {
        private readonly iFindContext _context;

        public DogodkiController(iFindContext context)
        {
            _context = context;
        }

        // GET: Dogodki
        public async Task<IActionResult> Index()
        {
            var dogodki = _context.Dogodek
                .Include(d => d.Kategorija);

            return View(await dogodki.ToListAsync());
        }

        // GET: Dogodki/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var dogodek = await _context.Dogodek
                .Include(d => d.Kategorija)
                .Include(d => d.Lokacija)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (dogodek == null)
                return NotFound();

            return View(dogodek);
        }

        // GET: Dogodki/Create
        public IActionResult Create()
        {
            ViewBag.KategorijaId = new SelectList(_context.Kategorija.ToList(), "Id", "Naziv");
            return View();
        }

        // POST: Dogodki/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Dogodek dogodek, string Lat, string Lng)
        {
            ModelState.Remove("OrganizatorId");
            ModelState.Remove("Organizator");

            ViewBag.KategorijaId = new SelectList(_context.Kategorija.ToList(), "Id", "Naziv", dogodek.KategorijaId);

            if (!ModelState.IsValid)
            {
                return View(dogodek);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            dogodek.OrganizatorId = userId;

            _context.Dogodek.Add(dogodek);
            await _context.SaveChangesAsync();

            if (!string.IsNullOrEmpty(Lat) && !string.IsNullOrEmpty(Lng) &&
                double.TryParse(Lat, NumberStyles.Any, CultureInfo.InvariantCulture, out double lat) &&
                double.TryParse(Lng, NumberStyles.Any, CultureInfo.InvariantCulture, out double lng))
            {
                _context.Lokacija.Add(new Lokacija
                {
                    DogodekId = dogodek.Id,
                    Latitude = lat,
                    Longitude = lng
                });
                await _context.SaveChangesAsync();
            }

            return Redirect("/Home/Index");
        }
 





 
        // GET: Dogodki/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var dogodek = await _context.Dogodek.FindAsync(id);
            if (dogodek == null)
                return NotFound();

            ViewData["KategorijaId"] =
                new SelectList(_context.Kategorija, "Id", "Naziv", dogodek.KategorijaId);

            return View(dogodek);
        }

        // POST: Dogodki/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Naziv,Opis,DatumCas,OrganizatorId,KategorijaId")] Dogodek dogodek)
        {
            if (id != dogodek.Id)
                return NotFound();

            if (!ModelState.IsValid)
            {
                ViewData["KategorijaId"] =
                    new SelectList(_context.Kategorija, "Id", "Naziv", dogodek.KategorijaId);

                return View(dogodek);
            }

            try
            {
                _context.Update(dogodek);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Dogodek.Any(e => e.Id == dogodek.Id))
                    return NotFound();
                else
                    throw;
            }

            return Redirect("/Home/Index");
        }

        // GET: Dogodki/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var dogodek = await _context.Dogodek
                .Include(d => d.Kategorija)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (dogodek == null)
                return NotFound();

            return View(dogodek);
        }

        // POST: Dogodki/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dogodek = await _context.Dogodek.FindAsync(id);
            if (dogodek != null)
                _context.Dogodek.Remove(dogodek);

            await _context.SaveChangesAsync();
            return Redirect("/Home/Index");
        }
    }
}

using web.Data;
using web.Models;
using System.Globalization;

namespace web.Controllers
{
    [Authorize]
    public class DogodkiController : Controller
    {
        private readonly iFindContext _context;

        public DogodkiController(iFindContext context)
        {
            _context = context;
        }

        // GET: Dogodki
        public async Task<IActionResult> Index()
        {
            var dogodki = _context.Dogodek
                .Include(d => d.Kategorija);

            return View(await dogodki.ToListAsync());
        }

        // GET: Dogodki/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var dogodek = await _context.Dogodek
                .Include(d => d.Kategorija)
                .Include(d => d.Lokacija)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (dogodek == null)
                return NotFound();

            return View(dogodek);
        }

        // GET: Dogodki/Create
        public IActionResult Create()
        {
            ViewBag.KategorijaId = new SelectList(_context.Kategorija.ToList(), "Id", "Naziv");
            return View();
        }

        // POST: Dogodki/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Dogodek dogodek, string Lat, string Lng)
        {
            ModelState.Remove("OrganizatorId");
            ModelState.Remove("Organizator");

            ViewBag.KategorijaId = new SelectList(_context.Kategorija.ToList(), "Id", "Naziv", dogodek.KategorijaId);

            if (!ModelState.IsValid)
            {
                return View(dogodek);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            dogodek.OrganizatorId = userId;

            _context.Dogodek.Add(dogodek);
            await _context.SaveChangesAsync();

            if (!string.IsNullOrEmpty(Lat) && !string.IsNullOrEmpty(Lng) &&
                double.TryParse(Lat, NumberStyles.Any, CultureInfo.InvariantCulture, out double lat) &&
                double.TryParse(Lng, NumberStyles.Any, CultureInfo.InvariantCulture, out double lng))
            {
                _context.Lokacija.Add(new Lokacija
                {
                    DogodekId = dogodek.Id,
                    Latitude = lat,
                    Longitude = lng
                });
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
 





 
        // GET: Dogodki/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var dogodek = await _context.Dogodek.FindAsync(id);
            if (dogodek == null)
                return NotFound();

            ViewData["KategorijaId"] =
                new SelectList(_context.Kategorija, "Id", "Naziv", dogodek.KategorijaId);

            return View(dogodek);
        }

        // POST: Dogodki/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Naziv,Opis,DatumCas,OrganizatorId,KategorijaId")] Dogodek dogodek)
        {
            if (id != dogodek.Id)
                return NotFound();

            if (!ModelState.IsValid)
            {
                ViewData["KategorijaId"] =
                    new SelectList(_context.Kategorija, "Id", "Naziv", dogodek.KategorijaId);

                return View(dogodek);
            }

            try
            {
                _context.Update(dogodek);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Dogodek.Any(e => e.Id == dogodek.Id))
                    return NotFound();
                else
                    throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Dogodki/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var dogodek = await _context.Dogodek
                .Include(d => d.Kategorija)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (dogodek == null)
                return NotFound();

            return View(dogodek);
        }

        // POST: Dogodki/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dogodek = await _context.Dogodek.FindAsync(id);
            if (dogodek != null)
                _context.Dogodek.Remove(dogodek);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}

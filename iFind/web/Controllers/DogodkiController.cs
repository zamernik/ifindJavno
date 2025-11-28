using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using web.Data;
using web.Models;

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
            var iFindContext = _context.Dogodek
                .Include(d => d.Kategorija);   // Organizator odstranjeno

            return View(await iFindContext.ToListAsync());
        }

        // GET: Dogodki/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var dogodek = await _context.Dogodek
                .Include(d => d.Kategorija)   // Organizator odstranjeno
                .FirstOrDefaultAsync(m => m.Id == id);

            if (dogodek == null)
                return NotFound();

            return View(dogodek);
        }

        // GET: Dogodki/Create
        public IActionResult Create()
        {
            ViewData["KategorijaId"] = new SelectList(_context.Kategorija, "Id", "Naziv");
            return View();
        }

        // POST: Dogodki/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Naziv,Opis,DatumCas,KategorijaId")] Dogodek dogodek)
        {
            if (ModelState.IsValid)
            {
                var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userIdClaim))
                    return Unauthorized("Niste prijavljeni!");

                dogodek.OrganizatorId = int.Parse(userIdClaim);

                _context.Add(dogodek);
                await _context.SaveChangesAsync();

                var latStr = Request.Form["Lat"];
                var lngStr = Request.Form["Lng"];

                if (decimal.TryParse(latStr, out var lat) &&
                    decimal.TryParse(lngStr, out var lng) &&
                    lat != 0 && lng != 0)
                {
                    var lokacija = new Lokacija
                    {
                        DogodekId = dogodek.Id,
                        Latitude = (double)lat,
                        Longitude = (double)lng
                    };

                    _context.Add(lokacija);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["KategorijaId"] =
                new SelectList(_context.Kategorija, "Id", "Naziv", dogodek.KategorijaId);

            return View(dogodek);
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

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dogodek);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DogodekExists(dogodek.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["KategorijaId"] =
                new SelectList(_context.Kategorija, "Id", "Naziv", dogodek.KategorijaId);

            return View(dogodek);
        }

        // GET: Dogodki/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var dogodek = await _context.Dogodek
                .Include(d => d.Kategorija) // Organizator odstranjeno
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

        private bool DogodekExists(int id)
        {
            return _context.Dogodek.Any(e => e.Id == id);
        }
    }
}

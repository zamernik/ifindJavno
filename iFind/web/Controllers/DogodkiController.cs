using System;
using System.Collections.Generic;
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
    [Authorize] // Zahteva prijavo za vse akcije (lahko omejiš samo na Create, če želiš)
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
            var iFindContext = _context.Dogodek.Include(d => d.Kategorija).Include(d => d.Organizator);
            return View(await iFindContext.ToListAsync());
        }

        // GET: Dogodki/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dogodek = await _context.Dogodek
                .Include(d => d.Kategorija)
                .Include(d => d.Organizator)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dogodek == null)
            {
                return NotFound();
            }

            return View(dogodek);
        }

        // GET: Dogodki/Create
        public IActionResult Create()
        {
            ViewData["KategorijaId"] = new SelectList(_context.Kategorija, "Id", "Naziv");
            // Odstranjeno ViewData["OrganizatorId"] - ni več potrebno, saj je avtomatsko
            return View();
        }

        // POST: Dogodki/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Naziv,Opis,DatumCas,KategorijaId")] Dogodek dogodek)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // AVTOMATSKO NASTAVI OrganizatorId na ID prijavljenega uporabnika
                    var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    if (string.IsNullOrEmpty(userIdClaim))
                    {
                        return Unauthorized("Niste prijavljeni!");
                    }
                    dogodek.OrganizatorId = int.Parse(userIdClaim); // Predpostavljam, da je ID int; če string, prilagodi

                    _context.Add(dogodek);
                    await _context.SaveChangesAsync(); // Shrani Dogodek, da dobi ID

                    // DODAJ: Ustvari Lokacijo, če sta lat/lng podani (iz hidden inputov v view-u)
                    var latStr = Request.Form["Lat"];
                    var lngStr = Request.Form["Lng"];
                    if (decimal.TryParse(latStr, out var lat) && decimal.TryParse(lngStr, out var lng) && lat != 0 && lng != 0)
                    {
                        var lokacija = new Lokacija
                        {
                            DogodekId = dogodek.Id, // FK na novi Dogodek
                            Latitude = (double)lat,
                            Longitude = (double)lng
                            // Dodaj druge lastnosti, npr. Naslov, če imaš polje v formi
                        };
                        _context.Add(lokacija);
                        await _context.SaveChangesAsync();
                    }

                    return RedirectToAction(nameof(Index));
                }
                ViewData["KategorijaId"] = new SelectList(_context.Kategorija, "Id", "Naziv", dogodek.KategorijaId);
                // Odstranjeno ViewData["OrganizatorId"] - ni več potrebno
            }
            catch (Exception ex)
            {
                // Debug: Izpiši v TempData (vidno v view-u z @TempData["Error"])
                TempData["Error"] = ex.Message + " | " + ex.InnerException?.Message;
                ViewData["KategorijaId"] = new SelectList(_context.Kategorija, "Id", "Naziv", dogodek.KategorijaId);
                return View(dogodek);
            }
            return View(dogodek);
        }

        // GET: Dogodki/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dogodek = await _context.Dogodek.FindAsync(id);
            if (dogodek == null)
            {
                return NotFound();
            }
            ViewData["KategorijaId"] = new SelectList(_context.Kategorija, "Id", "Naziv", dogodek.KategorijaId);
            ViewData["OrganizatorId"] = new SelectList(_context.Uporabnik, "Id", "Geslo", dogodek.OrganizatorId);
            return View(dogodek);
        }

        // POST: Dogodki/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Naziv,Opis,DatumCas,OrganizatorId,KategorijaId")] Dogodek dogodek)
        {
            if (id != dogodek.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Opcijsko: Če želiš, da se OrganizatorId ne spremeni pri editu, ga lahko override-aš tukaj
                    // dogodek.OrganizatorId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    
                    _context.Update(dogodek);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DogodekExists(dogodek.Id))
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
            ViewData["KategorijaId"] = new SelectList(_context.Kategorija, "Id", "Naziv", dogodek.KategorijaId);
            ViewData["OrganizatorId"] = new SelectList(_context.Uporabnik, "Id", "Geslo", dogodek.OrganizatorId);
            return View(dogodek);
        }

        // GET: Dogodki/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dogodek = await _context.Dogodek
                .Include(d => d.Kategorija)
                .Include(d => d.Organizator)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dogodek == null)
            {
                return NotFound();
            }

            return View(dogodek);
        }

        // POST: Dogodki/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dogodek = await _context.Dogodek.FindAsync(id);
            if (dogodek != null)
            {
                _context.Dogodek.Remove(dogodek);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DogodekExists(int id)
        {
            return _context.Dogodek.Any(e => e.Id == id);
        }
    }
}
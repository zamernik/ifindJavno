using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web.Data;
using web.Models;

namespace web.Controllers
{
    public class UporabnikiController : Controller
    {
        private readonly iFindContext _context;

        public UporabnikiController(iFindContext context)
        {
            _context = context;
        }

        // GET: Uporabniki
        public async Task<IActionResult> Index()
        {
            return View(await _context.Uporabnik.ToListAsync());
        }

        // GET: Uporabniki/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var uporabnik = await _context.Uporabnik
                .FirstOrDefaultAsync(m => m.Id == id);
            if (uporabnik == null) return NotFound();

            return View(uporabnik);
        }

        // GET: Uporabniki/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Uporabniki/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Ime,Priimek,Spol,Mail,Geslo,JeAdministrator,JeOrganizator")] Uporabnik uporabnik)
        {
            if (ModelState.IsValid)
            {
                // Nastavi trenutni datum ob registraciji
                uporabnik.DatumRegistracije = DateTime.Now;

                _context.Add(uporabnik);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(uporabnik);
        }

        // GET: Uporabniki/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var uporabnik = await _context.Uporabnik.FindAsync(id);
            if (uporabnik == null) return NotFound();

            return View(uporabnik);
        }

        // POST: Uporabniki/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ime,Priimek,Spol,Mail,Geslo,DatumRegistracije,JeAdministrator,JeOrganizator")] Uporabnik uporabnik)
        {
            if (id != uporabnik.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(uporabnik);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UporabnikExists(uporabnik.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(uporabnik);
        }

        // GET: Uporabniki/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var uporabnik = await _context.Uporabnik
                .FirstOrDefaultAsync(m => m.Id == id);
            if (uporabnik == null) return NotFound();

            return View(uporabnik);
        }

        // POST: Uporabniki/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var uporabnik = await _context.Uporabnik.FindAsync(id);
            if (uporabnik != null)
            {
                _context.Uporabnik.Remove(uporabnik);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // Preveri, če uporabnik obstaja
        private bool UporabnikExists(int id)
        {
            return _context.Uporabnik.Any(e => e.Id == id);
        }

        // GET: Uporabniki/Prijava
        public IActionResult Prijava()
        {
            return View(); // prikaz login forme
        }

        // POST: Uporabniki/Prijava
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Prijava(string email, string geslo)
        {
            var uporabnik = await _context.Uporabnik
                .FirstOrDefaultAsync(u => u.Mail == email && u.Geslo == geslo);

            if (uporabnik != null)
            {
                // Tukaj lahko nastaviš session/cookie za prijavljenega uporabnika
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Nepravilni email ali geslo");
            return View();
        }
    }
}

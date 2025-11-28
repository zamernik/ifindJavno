using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using web.Data;
using web.Models;

namespace web.Controllers
{
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
            ViewData["OrganizatorId"] = new SelectList(_context.Uporabnik, "Id", "Geslo");
            return View();
        }

        // POST: Dogodki/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Naziv,Opis,DatumCas,OrganizatorId,KategorijaId")] Dogodek dogodek)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dogodek);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["KategorijaId"] = new SelectList(_context.Kategorija, "Id", "Naziv", dogodek.KategorijaId);
            ViewData["OrganizatorId"] = new SelectList(_context.Uporabnik, "Id", "Geslo", dogodek.OrganizatorId);
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

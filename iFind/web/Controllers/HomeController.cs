using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using web.Models;
using web.Data;
using Microsoft.EntityFrameworkCore;//za dostop do baze
using Microsoft.AspNetCore.Authorization;   //za avtorizacijo(pri prijavi na nek dogodek)

namespace web.Controllers;

public class HomeController : Controller
{
    private readonly iFindContext _context; //dostop do baze

    public HomeController(iFindContext context) 
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    //za zemljevid
    [HttpGet]
    public async Task<IActionResult> GetEvents()
    {
        var events = await _context.Dogodek
            .Include(d => d.Kategorija) //Za Kategorija.Naziv
            .Include(d => d.Lokacija)   //Za lat/lng
            .Where(d => d.Lokacija != null)//pini se drugače ne morjo izrisali
            .Select(d => new
            {
                d.Id, //potrebno vedet za udelezbo
                Naziv = d.Naziv,
                lat = d.Lokacija.Latitude, 
                lng = d.Lokacija.Longitude,
                DatumCas = d.DatumCas.ToString("dd.MM.yyyy HH:mm"),
                Kategorija = d.Kategorija.Naziv, 
                opis = d.Opis
            })
            .ToListAsync();

        return Json(events); 
    }

    // Preveri, ali je uporabnik že prijavljen na dogodek + vrne stanje gumba
[HttpGet]
public async Task<IActionResult> GetUdelezbaStatus(int dogodekId)
{
    if (!User.Identity.IsAuthenticated)
        return Json(new { prijavljen = false });

    var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

    var prijavljen = await _context.Udelezba
        .AnyAsync(u => u.UporabnikId == userId && u.DogodekId == dogodekId);

    return Json(new { prijavljen });
}

// Prijava / odjava (toggle)
[HttpPost]
[Authorize]   // obvezno prijavljen!
public async Task<IActionResult> ToggleUdelezba([FromBody] ToggleUdelezbaRequest request)
{
    try
    {
        // 1. Preveri, če je uporabnik sploh prijavljen
        if (!User.Identity.IsAuthenticated)
            return Json(new { uspesno = false, sporocilo = "Ni prijavljen" });

        // 2. Pridobi userId na 100% zanesljiv način
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Json(new { uspesno = false, sporocilo = "UserId ni najden" });

        int dogodekId = request.DogodekId;  // ← DODAJ TO

        // 3. Preveri, če dogodekId sploh pride
        if (dogodekId <= 0)
            return Json(new { uspesno = false, sporocilo = "Napačen dogodekId" });

        // 4. Poišči obstoječo udeležbo
        var obstoja = await _context.Udelezba
            .FirstOrDefaultAsync(u => u.UporabnikId == userId && u.DogodekId == dogodekId);

        if (obstoja != null)
        {
            _context.Udelezba.Remove(obstoja);
            await _context.SaveChangesAsync();
            return Json(new { uspesno = true, prijavljen = false });
        }
        else
        {
            _context.Udelezba.Add(new Udelezba
            {
                UporabnikId = userId,
                DogodekId = dogodekId,
                DatumPrijave = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();
            return Json(new { uspesno = true, prijavljen = true });
        }
    }
    catch (Exception ex)
    {
        return StatusCode(500, new { uspesno = false, sporocilo = ex.Message, stack = ex.StackTrace });
    }
}
}
/* 
public IActionResult GetEvents()
    {
        // ==== TESTNI PODATKI!!!
        var testEvents = new[]
        {
            new { Naziv = "Koncert Plestenjaka", lat = 46.0569m, lng = 14.5058m, DatumCas = "14.12.2025 19:00", Kategorija = "zabava", opis = "Velik koncert Jana Plestenjaka v Stožicah" },
            new { Naziv = "Pohod na Triglav", lat = 46.3781m, lng = 13.8365m, DatumCas = "20.07.2025 05:00", Kategorija = "šport", opis = "Jutranji vzpon na Triglav z vodnikom, start na dnu" },
            new { Naziv = "Pivo v Mariboru", lat = 46.5547m, lng = 15.6459m, DatumCas = "02.08.2026 20:30", Kategorija = "zabava", opis = "Sproščeno druženje ob Laških pivih v centru Maribora" },
            new { Naziv = "Kino v Kopru", lat = 45.5481m, lng = 13.7302m, DatumCas = "11.09.2026 21:00", Kategorija = "kultura", opis = "Večerna projekcija novega filma na prostem ob koprski plaži" }

        };
        return Json(events);
    }
*/

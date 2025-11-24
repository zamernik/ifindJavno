using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using web.Models;
using web.Data;

namespace web.Controllers;

public class HomeController : Controller
{
    
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

        return Json(testEvents);
    }
}

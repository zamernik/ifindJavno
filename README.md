POVEZAVA Z BAZO (JT):
Najprej sem ustvaril mvc aplikacijo(z osnovno mvc predlogo), ki sem jo najprej(po zgledu iz vaj) povezal z podatkovno bazo. Za povezavo sem naredil container v dockerju z najnovejšim imigom in določil geslo(MojGeslo123!).

V VS Code sem si naložil SQL Server extension(saj bo tako najbolj praktično, da boma imela bazo na istem mestu kot izvorno kodo). V aplikaciji sem pod appsettings dodal connectionString te povezave, v program.cs. Nato sem pričel z 
kreacijo tabel v mapi Models. Tabele(modeli) so naslednje: Uporabnik(osnovni podatki, je_administrator, je_uporabnik), Dogodek(osnovni podatki o dogodku), Kategorija(pod katero kategorijo dogodek spada), Lokacija(latitude in longitude dogodka), 
Udeležba(prikazuje kater uporabnik gre na kater dogodek, vsebuje tudi števec). Nato sem ustvaril mapo Data in v njej iFindContext, definiral tabele, in definiral kako bo izgledal zapis tabel v množini(npr. uporabnik -> uporabniki 
, saj je privzeto v angleščini -> uporabniks). zaradi tega ker ima tabela Udeležba sestavljen PK, sem to tudi zapisal tukaj, da bo ob kreaciji baze to dejansko tako določilo. Za lažje upravljanje sem za dogodke, kategorije,
in uporabnike kreiral tudi controllerje, za enostavnejšo implementacijo, ki jih boma po potrebi izbrisala ali dodala še kakšnega za drugo tabelo. Nato sem začel z migracijo. V prvi migraciji je EF Core za tabelo Udelezbe nastavil ON DELETE CASCADE 
na obeh FK (do Uporabniki in Dogodki), kar je v SQL Serverju povzročilo napako zaradi več cikličnih cascade poti. Rešitev: v OnModelCreating sem ročno nastavil .OnDelete(DeleteBehavior.NoAction) na relaciji 
Udelezba → Uporabnik, izbrisal staro bazo in mapo Migrations ter naredil čisto novo migracijo. Po tem baza deluje brez težav. Nato sem bazo (s pomočjo controllerjev, ki sem jih dodal) testiral; dodal sem novega uporabnika,
ter nekaj osnovnih kategorij. Zdaj sledi nadaljevanje ustvarjanja aplikacije- registracija uporabnika- avtentikacija/avtorizacija in nato izdelava home paga. 


PRIJAVA,REGISTRACIJA,CSS IN NEKKAJ MALENKOSTI(JZ):
V /Views/Shared/_Layout.cshtml sem dodal dve novi povezavi do strani Prijava in do strani Registracija, pravtako sem nastavil asp-controllerja za te dve povezavi, ki sta povezana na določen pogled. V tem primeru je Registracija povezana na Uporabniki/create.cshtml.

Ker bova uporabljala svoj lasten css, sem v folderju /wwwroot/css dodal datoteko: registracija.css, v katero sem napsial css za registracijo.

V pogled /view/uporabniki/create.html sem vključil css datoteko z ukazom @section styles{...}, da to deluje, sem moral v glavno cshtml datoteko torej _Layout.cshtml v <head> vključiti     @RenderSection("Styles", required: false).

Odstranil em, da lahko uporabnik sam določa datum in čas registracije, sej mi ni smiselno, naredil sem tako, da se vzame trenutni čas in trenutni datum. To sem dodal tudi v kontroler uporabnikiController. Ostalo nisem spreminjal v controllerjih.

Popravil sm controller za uporabnika, saj sem moral dodati prijavo uporabnika,ki predhodno ni bila implementirana,saj je code generator sam po sebi ne ustvari, nato sme v poglede dodal pogled Prijava,da e uporabnik lahko prijavi.

Nimava metode createdbifnotexists, glej video 3, cca 50. minuta in vstavljenih userjev cca 45mnuta.

Preko Nugeta sem dodal Microsoft.AspNetCore.Identity.EntityFrameworkCore in izbral kar najnovejso verzijo tj. 10.0 in enake verzije 
Microsoft.AspNetCore.Identity.EntityFrameworkCore.UI.
P.S. Dodal sem velik komentarjev, da ti bo jasno kaj sem delal.


DODAJANJE ZEMLJEVIDA Z DELUJOČIMI PINNI(JT)
Najprej sem si naložil git za lažje push/pullanje najinega projekta iz githuba. To je stvarilo nekaj novih filov znotraj projekta. Nato sem pričel z implementacijo zemljevida. Določil sem, da bo zemljevid kar na home strani, lahko pa se kasneje prestavi. 
V Home Controllerju sem dodal metodo getEvents- ki bo pridobila podatke o dogodku, iz tabel dogodek, lokacija, kategorija. Zaenkrat še organizator ne more vpisovati dogodkov, zato sem dal notri testne podatke, da bom lahko vseeno preveril pravilnost izpisa na zemljevidu. 
Potem sem začel z prikazom zemljevida na index.cshmtl, najprej sem mislil z implementacijo z APIjem GoogleMapsa, a sem ugottovil da je z leafletom. Na kratko- Prikaže zemljevid med footom in headerjev, ki sta narejena v layout.cshtml(zo mi je povzročalo kar nekaj težav), potem gre čez vse dogodke, ki se zdaj nahajajo v controllerju, jih glede na njihovo lat in lng doda na zemljevid, ob kliku nanj pa sproži pop-up z podatki o dogodku ter možnostjo udeležbe s klikom na gumb. Vse narejeno v js, dodal sem nekaj komentarjev za razumevanje. 







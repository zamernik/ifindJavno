IFIND

Člana ekipe:
63240336 Jan Tuhtar
63240357 Jan Zamernik
UVOD
Iskanje dogodkov na internetu je lahko zamudno, poleg tega pa vsi dogodki niso oglaševani na spletnih straneh ali plakatnih panojih. Z informacijskim sistemom iFind želimo ta problem rešiti tako, da bi omogočili enostavno objavljanje in iskanje dogodkov na interaktivnem zemljevidu.
Organizatorji bi imeli brezplačen dostop do aplikacije, kjer bi po prijavi lahko dodajali nove dogodke z opisom, lokacijo, datumom in kategorijo (npr. koncert, šport, delavnica). 
Uporabniki bi lahko na zemljevidu hitro našli vse dogodke, ki jih zanimajo in lahko potrdili udeležbo.
NAJINO DELO
POVEZAVA Z BAZO
Najprej sem ustvaril MVC aplikacijo z uporabo osnovne MVC predloge. Aplikacijo sem nato, po zgledu iz vaj, povezal s podatkovno bazo. Za vzpostavitev povezave sem uporabil Docker, kjer sem ustvaril container z najnovejšim SQL Server imageom in nastavil geslo za dostop (MojGeslo123!).
V okolju VS Code sem namestil razširitev SQL Server, saj je tako najbolj praktično imeti podatkovno bazo na istem mestu kot izvorno kodo. V aplikaciji sem v datoteko appsettings dodal ustrezen connection string, ki sem ga nato uporabil v datoteki Program.cs.
Nato sem začel s kreacijo tabel v mapi Models. Ustvarjeni modeli (tabele) so:
•	Uporabnik (osnovni podatki, atributa je_administrator in je_uporabnik),
•	Dogodek (osnovni podatki o dogodku),
•	Kategorija (kategorija, pod katero dogodek spada),
•	Lokacija (geografska širina in dolžina dogodka),
•	Udeležba (povezovalna tabela, ki prikazuje, kateri uporabnik se udeleži katerega dogodka, ter vsebuje tudi števec).
Po tem sem ustvaril mapo Data in v njej razred iFindContext. V njem sem definiral vse tabele ter nastavil poimenovanje tabel v množini (npr. Uporabnik → Uporabniki), saj je privzeto poimenovanje v angleščini (npr. Uporabnik → Uporabniks). Ker ima tabela Udeležba sestavljen primarni ključ, sem to ustrezno definiral v kontekstu, da se ob kreaciji baze pravilno vzpostavi.
Za lažje upravljanje sem za dogodke, kategorije in uporabnike ustvaril tudi controllerje, ki omogočajo enostavnejšo implementacijo. Te controllerje bom po potrebi kasneje odstranil ali pa dodal nove za ostale tabele.
Nato sem izvedel migracijo baze. Pri prvi migraciji je EF Core za tabelo Udeležba nastavil ON DELETE CASCADE na obeh tujih ključih (do tabel Uporabniki in Dogodki), kar je v SQL Serverju povzročilo napako zaradi več cikličnih cascade poti. Težavo sem rešil tako, da sem v metodi OnModelCreating ročno nastavil .OnDelete(DeleteBehavior.NoAction) na relaciji Udeležba → Uporabnik. Po tem sem izbrisal staro bazo in mapo Migrations ter ustvaril popolnoma novo migracijo. Po tej spremembi baza deluje brez težav.
Na koncu sem bazo testiral s pomočjo ustvarjenih controllerjev. Dodal sem novega uporabnika ter nekaj osnovnih kategorij. V naslednjem koraku sledi nadaljevanje razvoja aplikacije, in sicer implementacija registracije uporabnikov, avtentikacije in avtorizacije ter izdelava začetne (home) strani.
REGISTRACIJA IN PRIJAVA V APLIKACIJO
V datoteki /Views/Shared/_Layout.cshtml sem dodal dve novi navigacijski povezavi, in sicer do strani Prijava in Registracija. Za obe povezavi sem nastavil ustrezna asp-controller in asp-action atributa, ki kažeta na pripadajoče poglede. V tem primeru je stran Registracija povezana s pogledom Uporabniki/Create.cshtml.
Ker aplikacija uporablja lasten CSS, sem v mapi /wwwroot/css dodal novo datoteko registracija.css, v kateri je definiran slog za registracijsko stran. To CSS datoteko sem vključil v pogled /Views/Uporabniki/Create.cshtml z uporabo razdelka @section Styles { ... }. Da je bilo to omogočeno, sem moral v glavno predlogo _Layout.cshtml znotraj elementa <head> dodati klic @RenderSection("Styles", required: false).
Odstranil sem možnost, da uporabnik sam določa datum in čas registracije, saj to ni smiselno. Namesto tega se ob registraciji samodejno zapišeta trenutni datum in čas. To logiko sem implementiral v kontrolerju UporabnikiController. Ostalih delov controllerjev nisem spreminjal.
Popravil sem tudi kontroler za uporabnike, saj je bilo potrebno dodati funkcionalnost prijave uporabnika, ki predhodno ni bila implementirana. Omenjena funkcionalnost ni bila samodejno ustvarjena s pomočjo code generatorja. Poleg tega sem dodal še nov pogled Prijava, ki uporabniku omogoča prijavo v aplikacijo.
Preko orodja NuGet sem v projekt dodal paketa Microsoft.AspNetCore.Identity.EntityFrameworkCore in Microsoft.AspNetCore.Identity.EntityFrameworkCore.UI, pri čemer sem za oba izbral najnovejšo razpoložljivo različico, tj. verzijo 10.0.
<img width="945" height="447" alt="image" src="https://github.com/user-attachments/assets/3b18e96b-d840-4baf-89df-44b7268efecf" />

 
DODAJANJE ZEMLJEVIDA V APLIKACIJO
Najprej sem namestil orodje Git, da je omogočeno enostavno upravljanje projekta z uporabo push in pull operacij na GitHubu. Pri tem je Git v projekt dodal nekaj novih datotek, ki so potrebne za pravilno delovanje verzioniranja.
Nato sem začel z implementacijo zemljevida. Odločil sem se, da bo zemljevid zaenkrat umeščen na začetno (home) stran aplikacije, z možnostjo kasnejše prestavitve na drugo lokacijo v okviru aplikacije.
V HomeController sem dodal metodo GetEvents, katere naloga je pridobivanje podatkov o dogodkih iz tabel Dogodek, Lokacija in Kategorija. Ker organizatorji dogodkov v trenutni fazi še ne morejo vnašati dogodkov, sem v metodo dodal testne podatke. To mi omogoča preverjanje pravilnosti prikaza dogodkov na zemljevidu.
Sledila je implementacija prikaza zemljevida v pogledu Index.cshtml. Sprva sem razmišljal o uporabi Google Maps API-ja, vendar sem se na koncu odločil za knjižnico Leaflet. Zemljevid se prikaže med glavo (header) in nogo (footer), ki sta definirani v datoteki _Layout.cshtml, kar mi je povzročilo nekaj težav pri postavitvi.
Zemljevid nato obdela vse dogodke, ki so trenutno definirani v kontrolerju. Na podlagi geografskih koordinat (latitude in longitude) se za vsak dogodek doda oznaka (marker) na zemljevid. Ob kliku na oznako se prikaže pojavno okno (pop-up) s podatki o dogodku ter možnostjo udeležbe, ki je izvedena s klikom na gumb.
Celotna funkcionalnost zemljevida je implementirana v JavaScriptu, koda pa vsebuje tudi komentarje za lažje razumevanje delovanja.
<img width="945" height="481" alt="image" src="https://github.com/user-attachments/assets/759767d5-65b0-42f4-8964-669466761deb" />

AVTENTIKACIJA UPORABNIKOV, PREHOD NA IDENTITY SISTEM TER POVEZAVA NA AZURE
Ustvaril sem razred ApplicationUser, saj aplikacija uporablja ASP.NET Identity namesto lastne tabele uporabnikov. Razredu sem dodal osnovne atribute (ime, priimek ipd.) in ustvaril novo migracijo. Po tem sem izbrisal obstoječo bazo ter jo ponovno ustvaril z ukazom database update, s čimer so se samodejno ustvarile tudi vse Identity tabele.
Prilagodil sem Program.cs, da uporablja avtentikacijo in Razor Pages. S pomočjo code generatorja sem ustvaril Identity strani, ki se nahajajo v mapi /Areas, ter pridobil tudi _LayoutPartial, ki dinamično prikazuje možnosti glede na stanje prijave uporabnika.
Registracijske strani sem razširil tako, da uporabnik vnese ime, priimek in ostale zahtevane podatke. Registracija in prijava sta bili testirani in delujeta pravilno. Omogočene so tudi napredne varnostne funkcije, kot so močna gesla, validacije in podpora za dvofaktorsko avtentikacijo. V tabelo AspNetRoles sem dodal vloge administrator, organizator in uporabnik. Strani za prijavo in registracijo sem dodatno oblikoval s CSS.
Identity strani (registracija, prijava, upravljanje profila) sem prevedel v slovenščino ter prilagodil pozdravno besedilo. Odstranil sem nekatere nepotrebne Identity strani (npr. e-poštno potrjevanje in 2FA), ki jih trenutno ne uporabljamo. Ustvaril sem tudi ločeno datoteko Manage.css za urejanje uporabniškega profila.
Na Azure sem aktiviral študentsko naročnino ter ustvaril novo SQL podatkovno bazo in App Service v isti resource group (ifind.gr). Aplikacijo sem povezal z Azure SQL bazo preko novega connection stringa in z dotnet ef database update uspešno ustvaril tabele tudi v oblačnem okolju. Aplikacija se uspešno zažene in deluje.
Zaradi prehoda na ASP.NET Identity sem moral prilagoditi podatkovni model. V tabeli Dogodki sem spremenil stolpec OrganizatorId iz int v nvarchar(450) in ga ponovno povezal na AspNetUsers.Id. Ob tem sem odstranil stare indekse, preveril podatke ter zagotovil konsistenco.
Tabelo Udelezbe sem ponovno ustvaril tako, da uporablja UporabnikId (FK na AspNetUsers) in DogodekId (FK na Dogodki) s sestavljenim primarnim ključem, kar preprečuje podvojene prijave uporabnikov na iste dogodke.
Na koncu sem odpravil napake pri prevajanju, povezane z Razor pogledi, kjer se je napačno dostopalo do lastnosti Geslo v razredu ApplicationUser, ki ne obstaja in se ne sme prikazovati. Opozorila glede nullable tipov ne vplivajo na delovanje aplikacije, ključne napake pa so zdaj jasno identificirane in pripravljene za odpravo v naslednjem koraku.

DODAJANJE MOŽNOSTI VNAŠANAJA DOGODKA
V navigacijsko vrstico na vrhu strani (Views/Shared/_Layout.cshtml) sem dodal povezavo do kontrolerja Dogodki, ki je bil ustvarjen že v prejšnji fazi razvoja. Ker mora organizator ob vnosu dogodka določiti tudi njegovo kategorijo, sem preko kontrolerja za kategorije v podatkovno bazo dodal nekaj osnovnih kategorij z opisi. Te so organizatorju na voljo pri ustvarjanju dogodka, dodatno pa je vključena tudi kategorija Drugo za primere, ko obstoječe kategorije niso ustrezne.
Odstranil sem možnost ročnega vnosa OrganizatorId, saj je smiselno, da sistem ta podatek samodejno pridobi na podlagi trenutno prijavljenega uporabnika. Ostali podatki dogodka (naziv, opis, čas in kategorija) so ostali nespremenjeni glede na obstoječo implementacijo v kontrolerju.
Poleg osnovnih podatkov mora organizator ob vnosu dogodka določiti tudi lokacijo. Ker je tabela Lokacija v relaciji 1 : 1 s tabelo Dogodki in vsebuje tuji ključ na dogodek, je potrebno dogodek najprej ustvariti, šele nato pa shraniti pripadajočo lokacijo. To zaporedje je implementirano v kontrolerju.
Za boljšo uporabniško izkušnjo sem se odločil, da ročni vnos zemljepisne širine in dolžine ni primeren, zato sem lokacijo rešil z uporabo interaktivnega zemljevida. Organizator na zemljevidu izbere lokacijo s klikom, s čimer se samodejno shranita latitude in longitude izbranega pina. Trenutno se ti vrednosti še izpisujeta na zaslonu zgolj za namen preverjanja pravilnega delovanja, kasneje pa bo ta izpis odstranjen. Vizualna podoba vnosa še ni dokončno urejena, funkcionalnost pa deluje.
Zemljevid je implementiran podobno kot na začetni strani. Ob kliku se ustvari pin, ob ponovnem kliku drugje se prejšnji pin odstrani in zamenja z novim, vrednosti lat in lng pa se sproti posodabljajo. Ti podatki se v obrazec prenašajo preko skritih (hidden) polj.
Nato sem nadaljeval z razvojem v DogodekControllerju. Osnovno strukturo sem ohranil, dodal pa sem pridobivanje identifikatorja prijavljenega uporabnika z uporabo User.FindFirstValue. Če uporabnik ni prijavljen, se izpiše ustrezno obvestilo, čeprav do teh funkcionalnosti neprijavljeni uporabniki nimajo dostopa. Pridobljen uporabniški ID se skupaj z ostalimi podatki iz obrazca shrani v tabelo Dogodki, nato pa se v tabelo Lokacija shrani še lokacija, vezana na ustvarjeni dogodek.
Trenutno vnos dogodka še ne deluje pravilno, najverjetneje zaradi konflikta z ASP.NET Identity sistemom, saj je ta funkcionalnost pred integracijo Identity delovala brez težav. Funkcionalnosti, namenjene administratorju (urejanje, brisanje ipd.), so ostale nespremenjene.
<img width="945" height="574" alt="image" src="https://github.com/user-attachments/assets/95bd8719-47dc-4fb6-ad66-ba6340ee2bac" />
 
PRIDOBITEV DOGODKA IZ PODATKOVNE BAZE TER IZRIS NA ZEMLJEVIDU
V HomeControllerju je bila že prisotna tabela testEvents, ki je vsebovala testne podatke za preverjanje pravilnega izrisa in prikaza dogodkov na glavnem zemljevidu. Te podatke sem uporabljal izključno za testiranje.
Najprej sem v HomeController dodal dostop do podatkovne baze preko iFindContext. Obstoječo metodo GetEvents sem nato razširil tako, da podatke pridobiva neposredno iz tabel Dogodek, Kategorija in Lokacija. Dodal sem pogoj, da mora imeti dogodek definirano lokacijo, saj brez nje prikaz na zemljevidu ni mogoč. Iz navedenih tabel sem s poizvedbo pridobil vse potrebne podatke za izris dogodkov na zemljevidu.
Za preverjanje delovanja sem začasno preklopil connection string na lokalno podatkovno bazo, ker na Azure strežniku trenutno še ni vnešenih dogodkov. V lokalno bazo sem dodal testni dogodek z lokacijo in preveril njegov izpis na zemljevidu, ki je deloval pravilno.
Po uspešnem testiranju sem testne primere zakomentiral in jih premaknil na dno datoteke, kjer so shranjeni za morebitno nadaljnjo uporabo.
<img width="565" height="416" alt="image" src="https://github.com/user-attachments/assets/07f0f1a1-64a3-4cb5-8677-3a862ad47373" />
 
DODAJANJE MOŽNOSTI PREGLEDA NAD ORGANIZATORJEVIMI DOGODKI TER PREGLEDA PREDVIDENE UDELEŽBE
Bom dopisal(gumb Pridem in stran za organizatorjev pregled)
<img width="945" height="160" alt="image" src="https://github.com/user-attachments/assets/b573a0b2-632c-417b-b374-c86ea9bd4369" />

 
RAZVOJ ANDROID APLIKACIJE
Bom kratko opisal.
 <img width="235" height="520" alt="image" src="https://github.com/user-attachments/assets/5b154d40-e7b7-4ca5-9add-9a8efdd38982" />




 






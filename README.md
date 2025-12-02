Člana ekipe:
63240336 Jan Tuhtar
63240357 Jan Zamernik

Iskanje dogodkov na internetu je lahko zamudno, poleg tega pa vsi dogodki niso oglaševani na spletnih straneh ali plakatnih panojih. Z informacijskim sistemom iFind želimo ta problem rešiti tako, da bi omogočili enostavno objavljanje in iskanje dogodkov na interaktivnem zemljevidu.

Organizatorji bi imeli brezplačen dostop do aplikacije, kjer bi po prijavi lahko dodajali nove dogodke z opisom, lokacijo, datumom in kategorijo (npr. koncert, šport, delavnica). 

Uporabniki bi lahko na zemljevidu hitro našli vse dogodke, ki jih zanimajo in lahko potrdili udeležbo.


Najino delo:
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




AUTENTIKACIJA, AZURE, GITHUB ... (JZ)
Ustvaril sem razred ApplicationUser, saj bomo uporabljali Identity razširitev namesto lastne tabele uporabniki. Razredu ApplicationUser sem dodal nekaj atributov, kot so ime, priimek … (glej Models/ApplicationUser). Nato sem ustvaril novo migracijo z imenom ApplicationUser. Po tem sem izbrisal obstoječo podatkovno bazo in jo ponovno posodobil z ukazom database update, ki je ponovno izvedel migracije. S tem se je ustvarilo kar nekaj novih tabel, ki so posledica Identity implementacije.

Prilagodil sem program.cs, da uporablja avtentikacijo in Razor pages, ter generiral potrebne poglede. S code generatorjem sem ustvaril nove strani, ki so sedaj vidne pod /web/Areas. Pri tem generiranju se je v zavihku /Views ustvaril tudi _LayoutPartial, ki deduje od Identity. Vsebuje dva gumba, ki prikazujeta podatke glede na to, ali je uporabnik prijavljen ali ne.

Moral sem prilagoditi precej stvari v strani za registracijo. Dodal sem, da mora uporabnik vnesti ime, priimek in ostale potrebne podatke, da registracija deluje. Dodal sem vse atribute, ki jih mora uporabnik izpolniti, da se uspešno registrira. Vse skupaj sem testiral in deluje. Omogočena je napredna avtentikacija, torej močna gesla, dvojna avtentikacija, preverjanje sintaktičnih napak itd.

V tabelo AspNetRoles sem dodal tri vloge: administrator, organizator in uporabnik. Na koncu sem novim stranem za registracijo in prijavo dodal še CSS.

Prevedel sem ustvarjene strani za registracijo, prijavo in urejanje osebnih podatkov v slovenščino ter dodal, da se namesto »Hello email…« izpiše »Pozdravljen Ime…«.

Pobrisal sem nekaj strani, ki sva jih dobila z Identity, saj jih trenutno ne potrebujeva, npr. dvojna avtentikacija in potrditev preko e-pošte.

Napisal sem Manage.css, v katerem se hrani CSS za uporabniški profil, saj registracija.css ni ustrezal. Poleg tega sem prevedel vse strani, ki se tičejo urejanja uporabniškega profila.

Na spletni strani Azure sem se prijavil preko študentskega e-maila in aktiviral Azure naročnino. V Azure sem ustvaril novo SQL podatkovno bazo z naslednjimi podatki:

Resource group: ifind.gr

Ime podatkovne baze: ifind-db

Server name: ifind-db

Lokacija: (Europe) Germany West

Administrator: su-ifind

Geslo: Mojegeslo123!

Service tier: Basic (do 2 GB podatkov)

V lokalno podatkovno bazo sem moral dodati novo povezavo s strežnikom ifind-database.database.windows.net. Aplikacijo sem povezal s spletnim strežnikom tako, da sem dodal nov connection string z zgornjimi podatki ter spremenil ime v program.cs. Z ukazom dotnet ef database update sem posodobil podatkovno bazo, tako da so se tabele ustvarile tudi na Azure. Nato sem pognal build in run aplikacije.

Ustvaril sem tudi nov App Service v Azure, ki sem ga dodelil isti resource group kot podatkovno bazo, torej ifind.gr. Aplikacijo sem poimenoval ifind-si in nastavil strežnik na Germany West Central, enako kot za DB strežnik, ter uporabil Free plan.

V tabeli dbo.Dogodki sem najprej odstranil obstoječi tuji ključ, ki je povezal stolpec OrganizatorId s tabelo dbo.Uporabniki, ker smo prešli na uporabo ASP.NET Identity sistema, kjer se uporabniki hranijo v tabeli dbo.AspNetUsers z identifikatorji tipa nvarchar. Zaradi tega sem spremenil podatkovni tip stolpca OrganizatorId iz int v nvarchar(450), da je kompatibilen z novimi identifikatorji. Pred spremembo sem odstranil indeks, ki je bil odvisen od tega stolpca, saj bi sicer ALTER TABLE ne uspel. Nato sem preveril in posodobil ali odstranil vse vrednosti OrganizatorId, ki niso obstajale v tabeli AspNetUsers, da ne bi prišlo do konflikta z novim tujem ključem. Po čiščenju podatkov sem dodal nov FOREIGN KEY, ki zdaj povezuje OrganizatorId z AspNetUsers.Id, kar omogoča pravilno referenciranje uporabnikov iz nove identitetne sheme. Na koncu sem po potrebi izbrisal vse vrstice iz tabele Dogodki, da sem zagotovil konsistenco podatkov in možnost novih vnosov v skladu z novo strukturo.

Tabelo Udelezbe sem najprej izbrisal, da sem odstranil obstoječe tuje in primarne ključe, nato pa sem ustvaril novo tabelo z stolpci UporabnikId tipa NVARCHAR(450), ki je tuji ključ na AspNetUsers.Id, DogodekId tipa INT, ki je tuji ključ na Dogodki.Id, in DatumPrijave tipa DATETIME2 za beleženje časa prijave. Primarni ključ sem določil kot kombinacijo UporabnikId in DogodekId, da preprečim podvajanje prijav istega uporabnika na isti dogodek, sprememba pa zagotavlja skladnost s podatkovnim modelom ASP.NET Identity in omogoča učinkovito upravljanje prijav uporabnikov na dogodke.
Najprej sem odpravil napako v modelu Dogodek, kjer sem popravil definicije lastnosti in odstranil morebitne nepravilnosti. Nato sem ponovno pognal dotnet build in dobil nova opozorila ter tri ključne napake. Te napake so se nanašale na Razor poglede, kjer je koda dostopala do lastnosti Geslo v modelu ApplicationUser, ki pa v resnici ne obstaja. Identificiral sem problematične datoteke (Delete.cshtml, Details.cshtml in Index.cshtml) ter razumel, da moram iz pogledov odstraniti prikaz lastnosti Geslo, ker ta ni del modela in je ne bi smel nikoli prikazovati. Ugotovil sem tudi, da so nekatera opozorila povezana z nullable tipi, vendar ta ne preprečujejo delovanja. Glavne napake zdaj izvirajo iz napačnih referenc na ApplicationUser.Geslo, ki jih bom popravil v naslednjem koraku.

VNAŠANJE DOGODKA(JT):
V navigacijsko vrstico na vrhu strani(views/shared/layout) sem dodal povezavo do dogodki controllerja, ki sem ga že predhodno naredi. Ker mora organizator ob vnosu dogodka zabeležiti tudi njegovo kategorijo, sem preko kontrolerja za kategorijo z opisi vnesel nekaj osnovnih kategorij. Organizator jih ima tako pri vnosu dogodka na izbiro, če ne ustreza nobeni kategoriji sem dodal drugo.  
Izbrisal sem možnost vnosa organizatorId(saj je smiselno da tega sistem ob vnosu pridobi sam glede na prijavljenega uporabnika), ostalo sem pustil tako kot je bilo narejeno v controllerju(naziv, opis, čas, kategorija dogodka).
Poleg tega pa mora organizator v tabelo vpisati tudi lokacijo dogodka. Problem je, ker lokacija nosi FK na Dogodek(1:1 povezava), zato je treba dogodek kreirati pred lokacijo(sprogramiram kasneje v controllerju).  
Da je vnos lokacije uporabniško prijaznejši(ročno vpisovanje latitude in longitude ne bi bilo estetsko) sem se odločil da to rešim preko implementacije zemljevida, kjer organizator določi pin in se avtomatsko shrani lat in lng tega pina. Na zaslonu se trenutno lat in lng izpišeta, zgolj zaradi tega da preverjam če se vrednosti pravilno shranita v spremenljivko, kar lahko kasneje odstranima, saj je za uporabnika nepotrebno. Estetsko stran še ni lepo urejena, vpisovanje podatkov deluje.
Zemljevid na kratko- implementiran podobno kot na glavni strani, ob kliku se pojavi pin, shranita se lat in lng(definirana kot hidden inputa za razliko od drugih), ob kliku drugam se prejšni pin izbriše, pojavi nov, vrednosti se posodobita.

Nato sem začel z delom v Dogodek Controllerju. Osnovno strukturo sem pustil enako, dodal pa sem: User.findFirstValu- za pridobivanje id-ja uporabnika, sicer naj izpiše da ni prijavljen(kar niti ne bo potrebno glede na to da neprijavljeni ne bodo imeli dostopa). V dogodek torej shranimo ta id in druge pridobljene informacije iz forma, dogodek tako kreiramo. nato pridobimo še lat in lng, ki jo skupaj z id-jem dogodka shranimo v tabelo Lokacija. Vnos najverjetneje zaradi konflikta z identity(vnos dogodka je namreč pred tem deloval) tabelo trenurno ne deluje. Ostale funkcije, ki so namenjene administratorju(edit, delete...) sem pustil nespremenjene. 

PRIDOBITEV in IZRIS DOGODKA(JT):
V HomeControllerju že imama tabelo testEvents, a so podatki tukaj testni, da sem prej preverjal pravilen izris in izpis glavnega zemljevida. Home controllerju sem najprej dodal dosto do baze(ifind context). Pod že definirano metodo GetEvents sem v njih dodal tabele Dogodek, Kategorija, Lokacija. Dodal sem pogoj, da mora biti posamezen dogodek v pb imeti definirano lokacijo(ker sicer izris na zemljevidu ni mogoč), nato pa sem iz teh treh tabel z sql stavkom pridobil zahtevane vrednosti, ki se bodo vnesle v tabelo. 
Za preverjanje sem connection string spremenil na lokalno bazo(na azure serverju ni vpisanih dogodkov), vpisal nek dogodek, mu dodal lokacijo in preveril izpis na zemljevidu, ki je bil pravilen. testni primere sem zakomentiral in jih dal na dno datoteke, če bodo še uporabljeni. 

GIT, GITHUB(JT):
V projekt sem dodal .gitignore, da ne boma pull/pushalla nepotrebnih datotek v najinem repositoryu- lepša urejenost. Gizhub repository sma imela že narejen, vmes sma ga ob javni objavi aplikacije duplicirala in spremenila. Git z potrebnimi mapami(clear, git, main) sem ustvaril že predhodno ob začetku projekta. Ob spremembi repositorya je bila potrebna sprememba lokacije za push in pull.  Vaje6- pull request in branchanje!



 






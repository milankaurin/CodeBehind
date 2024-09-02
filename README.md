# Projekat: Simulacija Grupne faze i eliminacione faze Olimpijskih igara
Ovaj projekat simulira grupnu fazu i eliminacionu fazu košarkaškog turnira prema pravilima koja uključuju FIBA rang liste, bodovanje, i eliminacionu strukturu.

# Arhitektura Projekta
Projekt sadrži sledeće glavne klase:

Program.cs:

Sadrži Main metodu koja pokreće simulaciju turnira.
Učitava podatke o grupama iz groups.json fajla.
Simulira sve mečeve grupne faze koristeći "round-robin" metod.
Rangira timove u grupama i formira šešire za eliminacionu fazu.
Simulira eliminacionu fazu i prikazuje rezultate.

Turnir.cs:

Definiše klasu Turnir koja sadrži listu timova za svaku grupu (A, B, i C).

Utakmica.cs:

Definiše klasu Utakmica koja simulira mečeve između dva tima.
Izračunava verovatnoću pobede na osnovu FIBA rang liste.
Generiše nasumične rezultate na osnovu verovatnoće pobede istovremenu uzimajući formu timova kao faktor. Forma se ažurira nakon svake utakmice.

Tim.cs:

Definiše klasu Tim koja sadrži informacije o timu, uključujući ime tima, ISO kod, FIBA rang, broj bodova, postignute i primljene poene, i razne statistike.
Pruža metodu izracunajKosRazliku() za izračunavanje koš razlike tima.

# Glavne Funkcionalnosti
Simulacija mečeva grupne faze: Svi timovi igraju međusobno tri meča u grupnoj fazi. **Verovatnoća pobede zavisi od razlike u FIBA rang listi. Koševi zavise takođe i od forme tima.**
Rangiranje timova u grupi: Timovi se rangiraju na osnovu bodova, a u slučaju istog broja bodova koriste se dodatni kriterijumi poput međusobnog susreta i koš razlike.
Formiranje Šešira i Eliminacione Faze: Timovi se raspoređuju u šešire na osnovu njihovog plasmana u grupi. Eliminaciona faza uključuje četvrtfinale, polufinale i finale.
Prikaz Rezultata: Prikaz rezultata svih mečeva grupne faze i eliminacione faze, uključujući konačne rang liste i osvajače medalja.
Statistike i Model
**Projekat koristi nasumičnu generaciju rezultata baziranu na verovatnoći pobede koja je u korelaciji sa razlikom u FIBA rang pozicijama timova.
Model omogućava fleksibilnost i neizvesnost u rezultatima, a istovremeno poštuje činjenicu da bolji timovi po pravilu imaju veću verovatnoću pobede.**

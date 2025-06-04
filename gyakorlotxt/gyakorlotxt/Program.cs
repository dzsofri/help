using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    // Ez a lista tárolja az összes személyt az alkalmazás futása alatt
    static List<Szemely> szemelyek = new List<Szemely>();

    // Ez a fájlnév, amiből betöltjük és amibe mentjük az adatokat
    static string fajlNev = "adatok.txt";

    // Ez az osztály tárol egy személy adatait (név, kor, város)
    class Szemely
    {
        public string Nev { get; set; }
        public int Kor { get; set; }
        public string Varos { get; set; }

        public Szemely(string nev, int kor, string varos)
        {
            Nev = nev;
            Kor = kor;
            Varos = varos;
        }

        // Ez a metódus szépen formázott szöveget ad vissza a személy adatairól
        public string Kiir()
        {
            return $"Név: {Nev}, Kor: {Kor}, Város: {Varos}";
        }

        // Ez a metódus megmondja, hogyan alakítsuk szöveggé a személyt, amikor fájlba írunk
        public override string ToString()
        {
            return $"{Nev};{Kor};{Varos}";
        }
    }

    static void Main()
    {
        // Betöltjük az adatokat a fájlból a szemelyek listába
        BetoltAdatok();

        // Ez az örökös menü ciklus, amíg a felhasználó nem lép ki
        while (true)
        {
            Console.WriteLine("\n--- MENÜ ---");
            Console.WriteLine("1. Listázás");
            Console.WriteLine("2. Hozzáadás");
            Console.WriteLine("3. Törlés");
            Console.WriteLine("4. Mentés másik fájlba");
            Console.WriteLine("5. Kilépés");
            Console.Write("Választás: ");
            string valasz = Console.ReadLine();

            switch (valasz)
            {
                case "1": Listaz(); break;    // Kilistázza az összes személyt
                case "2": Hozzaad(); break;   // Új személy hozzáadása
                case "3": Torol(); break;     // Személy törlése név alapján
                case "4": Ment(); break;      // Az aktuális lista mentése egy új fájlba
                case "5": return;             // Kilép a programból
                default: Console.WriteLine("Érvénytelen választás."); break;
            }
        }
    }

    static void BetoltAdatok()
    {
        // Ellenőrizzük, hogy létezik-e a fájl, ha nem, figyelmeztetünk és kilépünk
        if (!File.Exists(fajlNev))
        {
            Console.WriteLine("Fájl nem található, új fájl lesz létrehozva.");
            return;
        }
     
        // Beolvassuk az összes sort a fájlból
        var sorok = File.ReadAllLines(fajlNev);

        // Minden sort feldolgozunk
        foreach (var sor in sorok)
        {
            // A sort pontosvessző mentén szétvágjuk (név;kor;város)
            var mezok = sor.Split(';');

            // Új személy objektumot hozunk létre és hozzáadjuk a listához
            szemelyek.Add(new Szemely(mezok[0], Convert.ToInt32(mezok[1]), mezok[2]));
        }
    }

    static void Listaz()
    {
        // Ha nincs adat, jelezzük
        if (szemelyek.Count == 0)
        {
            Console.WriteLine("Nincs adat.");
            return;
        }

        Console.WriteLine("\n--- Adatok ---");
        // Kiírjuk az összes személy adatait szépen formázva
        foreach (var szemely in szemelyek)
        {
            Console.WriteLine(szemely.Kiir());
        }
    }

    static void Hozzaad()
    {
        // Bekérjük a nevet
        Console.Write("Név: ");
        string nev = Console.ReadLine();

        // Bekérjük a kort, és ellenőrizzük, hogy szám-e
        Console.Write("Kor: ");
        if (!int.TryParse(Console.ReadLine(), out int kor))
        {
            Console.WriteLine("Hibás kor!");
            return;
        }

        // Bekérjük a várost
        Console.Write("Város: ");
        string varos = Console.ReadLine();

        // Létrehozunk egy új Szemely objektumot a megadott adatokkal
        var uj = new Szemely(nev, kor, varos);

        // Hozzáadjuk a listához
        szemelyek.Add(uj);

        /*
        File.AppendAllText:
        - Ez egy beépített metódus, ami hozzáfűz egy szöveget egy fájl végéhez.
        - Ha a fájl nem létezik, létrehozza azt.
        - Az Environment.NewLine egy új sort jelent (operációs rendszer szerint \n vagy \r\n).
        Így az új személy adata új sorba kerül a fájlban.
        */
        File.AppendAllText(fajlNev, Environment.NewLine + uj.ToString());

        Console.WriteLine("Hozzáadva.");
    }

    static void Torol()
    {
        // Bekérjük a törlendő személy nevét
        Console.Write("Törlendő név: ");
        string nev = Console.ReadLine();

        /*
        FirstOrDefault:
        - Megkeresi az első olyan személyt a listában, akinek a neve megegyezik a megadott névvel,
          függetlenül a kis- és nagybetűktől (OrdinalIgnoreCase).
        - Ha nincs ilyen személy, null-t ad vissza.
        */
        var szemely = szemelyek.FirstOrDefault(s => s.Nev.Equals(nev, StringComparison.OrdinalIgnoreCase));

        if (szemely != null)
        {
            // Töröljük a személyt a listából
            szemelyek.Remove(szemely);

            /*
            File.WriteAllLines:
            - Ez felülírja a fájlt a jelenlegi személyek listájával.
            - Minden személy adata egy új sorba kerül, a ToString() metódus szerint formázva.
            Így a fájlban is frissülnek az adatok a törlés után.
            */
            File.WriteAllLines(fajlNev, szemelyek.Select(s => s.ToString()));

            Console.WriteLine("Törölve.");
        }
        else
        {
            Console.WriteLine("Nincs ilyen nevű személy.");
        }
    }

    static void Ment()
    {
        // Az aktuális személyek listáját egy új fájlba mentjük (pl. biztonsági mentés)
        string ujFajl = "kimentett_adatok.txt";
        File.WriteAllLines(ujFajl, szemelyek.Select(s => s.ToString()));
        Console.WriteLine($"Mentve ide: {ujFajl}");
    }
}

//Tester sortowników
//©WJL 2023

using System.Collections.Concurrent;
using System.Diagnostics;
using System.Numerics;

namespace Sortowniki;

public class CentralnaKlasa
{
    private static readonly List<SrodowiskoTestowe> srodowiska = new()
    {
        new(new Bombel()),
        new(new Gnom())
    };
    private static readonly int[] dlugosciTablic = new int[]
    {
        100, 500, 1_000, 2_000, 5_000, 10_000, 20_000
    };

    public static void Main()
    {
        Parallel.ForEach(srodowiska, srodowisko =>
        {
            Parallel.ForEach(dlugosciTablic, dlugosc =>
            {
                var tablica = ObslugaTablic.UtworzLosowaTablice(dlugosc);

                Stopwatch zegar = Stopwatch.StartNew();

                srodowisko.TestowanySortownik.Sortuj(tablica);

                zegar.Stop();

                WynikIteracji wynik = new()
                {
                    Srednia = zegar.Elapsed,
                    Mediana = zegar.Elapsed,
                    OdchylenieStandardowe = new TimeSpan(0),
                    Wariancja = new TimeSpan(0)
                };

                srodowisko.WynikiDlaDlugosci[dlugosc] = wynik;
            });
        });

        foreach (var srodowisko in srodowiska)
        {
            Console.WriteLine($"wyniki dla {srodowisko.TestowanySortownik.NazwaWyswietlana}:");
            foreach (var (dlugosc, wynik) in srodowisko.WynikiDlaDlugosci.OrderBy(k => k.Key))
            {
                Console.WriteLine($"{dlugosc,10:# ###} elementów: {wynik.Srednia.TotalMilliseconds,9:# ##0.000}ms");
            }
            Console.WriteLine();
        }

        Console.WriteLine("Naciśnij dowolny klawisz by zamknąć program...");
        Console.ReadKey();
    }
}

public abstract class BazaSortownikow
{
    public abstract string NazwaWyswietlana { get; }
    public abstract void Sortuj(int[] tablica);
}

public class Bombel : BazaSortownikow
{
    public override string NazwaWyswietlana => "Sortowanie bąbelowe";

    public override void Sortuj(int[] tablica)
    {
        Bombelkuj(tablica);
    }

    public static void Bombelkuj(int[] tablica)
    {
        bool flaga;
        do
        {
            flaga = false;
            for (uint i = 0; i < tablica.Length - 1; i++)
            {
                if (tablica[i] > tablica[i + 1])
                {
                    (tablica[i], tablica[i + 1]) = (tablica[i + 1], tablica[i]);
                    flaga = true;
                }
            }
        } while (flaga);
    }
}

public class Gnom : BazaSortownikow
{
    public override string NazwaWyswietlana => "Sortowanie gnoma";

    public override void Sortuj(int[] tablica)
    {
        Gnomuj(tablica);
    }

    public static void Gnomuj(int[] tablica)
    {
        uint i = 0;
        while (i < tablica.Length - 1)
        {
            if (tablica[i] > tablica[i + 1])
            {
                (tablica[i], tablica[i + 1]) = (tablica[i + 1], tablica[i]);
                if (i > 0)
                    i--;
            }
            else
                i++;
        }
    }
}

public class Bogosort : BazaSortownikow
{
    private readonly Random RNG = new();

    public override string NazwaWyswietlana => "Bogosort";

    public override void Sortuj(int[] tablica)
    {
        Bogosortuj(tablica);
    }

    public void Bogosortuj(int[] tablica)
    {
        bool gotowe;

        while (true)
        {
            gotowe = true;

            for (uint i = 1; i < tablica.Length; i++)
            {
                if (tablica[i - 1] > tablica[i])
                {
                    gotowe = false;
                    break;
                }
            }

            if (gotowe)
                break;

            for (uint i = 0; i < tablica.Length; i++)
            {
                int losowyIndeks = RNG.Next(tablica.Length);
                (tablica[i], tablica[losowyIndeks]) = (tablica[losowyIndeks], tablica[i]);
            }
        }
    }
}

public record struct WynikIteracji
{
    public TimeSpan Srednia { get; set; }
    public TimeSpan Mediana { get; set; }
    public TimeSpan Wariancja { get; set; }
    public TimeSpan OdchylenieStandardowe { get; set; }
}

static class ObslugaTablic
{
    private static readonly Random randomizer = new();

    public static void WypiszTablice(int[] tablica)
    {
        for (uint i = 0; i < tablica.Length; i++)
            Console.WriteLine(tablica[i]);
    }

    public static void ZapelnijLosowo(int[] tablica, int max = int.MaxValue)
    {
        for (int i = 0; i < tablica.Length; i++)
            tablica[i] = randomizer.Next(max);
    }

    public static int[] UtworzLosowaTablice(int dlugosc, int max = int.MaxValue)
    {
        var tablica = new int[dlugosc];
        ZapelnijLosowo(tablica, max);
        return tablica;
    }

    public static TPewienTypLiczbowybezZnaku Mediana<TPewienTypLiczbowybezZnaku>(TPewienTypLiczbowybezZnaku[] tab)
        where TPewienTypLiczbowybezZnaku : IUnsignedNumber<TPewienTypLiczbowybezZnaku> //oblicza medianę 
    {                                                                                //pól tablicy 
        int pul = tab.Length / 2;                                                    //dowolnego typu
        TPewienTypLiczbowybezZnaku suma;                                              //przy użyciu interfejsów
                                                                                      //uogólnionych typów
        if (tab.Length % 2 != 0)                                                     //liczbowych bez znaku
            return tab[pul + 1];

        suma = tab[pul] + tab[pul + 1];
        TPewienTypLiczbowybezZnaku dwa = TPewienTypLiczbowybezZnaku.One + TPewienTypLiczbowybezZnaku.One;
        return suma / dwa;
    }
}

public class SrodowiskoTestowe
{
    public BazaSortownikow TestowanySortownik { get; }
    public ConcurrentDictionary<int, WynikIteracji> WynikiDlaDlugosci { get; } = new();

    public SrodowiskoTestowe(BazaSortownikow testowanySortownik)
    {
        TestowanySortownik = testowanySortownik;
    }
}


/*
Testy:
a algorytmów
    po d długości listy
        po i iteracji
            1 wynik z każdej iteracji (timespan)
        zagregowany wynik z każdej długości (WynikIteracji)
    wynik testu dla każdej długości tablicy (Dictionary<int,WynikIteracji>??)
 */

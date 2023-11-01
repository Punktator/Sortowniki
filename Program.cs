//Tester sortowników
//WL 2023

using System.Diagnostics;
using System.Numerics;

namespace Sortowniki;

public class Program
{
    public const ushort iteracjewTescie = 2500;

    int [] probnaTablica;
    Stopwatch unizegar = new();
    protected static readonly Random randomizer = new();
    public WynikTestu[] wyniki = new WynikTestu[iteracjewTescie];
    private readonly List<Baza_Sortownikow> listaSortownikow = new()
    {
        new Bombel(),
        new Bogosort(),
        new Gnom()
    };

    public static void Main()
    {
        Program sortowniki = new();

        Console.WriteLine("Podaj rozmiar tablicy: ");
        int n = int.Parse(Console.ReadLine());
        sortowniki.probnaTablica = new int[n];
        
        ZapelnijLosowo(sortowniki.probnaTablica);

        WypiszTablice(sortowniki.probnaTablica);

        Console.WriteLine();

        Console.WriteLine("Sortowanie...");
        sortowniki.unizegar = Stopwatch.StartNew();
        sortowniki.listaSortownikow[0].Sortuj(sortowniki.probnaTablica);
        sortowniki.unizegar.Stop();
        TimeSpan tBomblowania = sortowniki.unizegar.Elapsed;

        WypiszTablice(sortowniki.probnaTablica);

        Console.WriteLine();

        Console.Write("T = ");
        Console.WriteLine(tBomblowania);


        ZapelnijLosowo(sortowniki.probnaTablica);

        Console.WriteLine();

        foreach (var sortownik in sortowniki.listaSortownikow)
        {
            for (uint i = 3; i < iteracjewTescie; i++)
            {
                sortowniki.probnaTablica = new int[i];
                ZapelnijLosowo(sortowniki.probnaTablica);

                sortowniki.unizegar.Restart();
                sortownik.Sortuj(sortowniki.probnaTablica);
                sortowniki.unizegar.Stop();

                sortowniki.wyniki[i] = new WynikTestu()
                {
                    Srednia = (ulong)sortowniki.unizegar.Elapsed.TotalNanoseconds,
                    Mediana = (ulong)sortowniki.unizegar.Elapsed.TotalNanoseconds,
                    OdchylenieStandardowe = 0,
                    Wariancja = 0,
                };
            }
        }


        Console.WriteLine("Naciśnij dowolny klawisz by zamknąć program...");
        Console.ReadKey();
    }

    public static void WypiszTablice(int[] tablica)
    {
        for (uint i = 0; i < tablica.Length; i++)
        {
            Console.WriteLine(tablica[i]);
        }
    }

    public static void ZapelnijLosowo(int[] tablica, int max = int.MaxValue)
    {
        for (int i = 0; i < tablica.Length; i++) tablica[i] = randomizer.Next(max);
    }

    public static jakisTypLiczbowybezZnaku Mediana <jakisTypLiczbowybezZnaku> (jakisTypLiczbowybezZnaku[] tab) 
        where jakisTypLiczbowybezZnaku : IUnsignedNumber<jakisTypLiczbowybezZnaku> //oblicza medianę 
    {                                                                              //pól tablicy 
        int pul = tab.Length / 2;                                                  //dowolnego typu
        jakisTypLiczbowybezZnaku suma;                                             //przy użyciu interfejsów
                                                                                   //uogólnionych typów
        if (tab.Length % 2 != 0)                                                   //liczbowych bez znaku
            return tab[pul+1];
        suma = tab[pul] + tab[pul+1];
        jakisTypLiczbowybezZnaku dwa = jakisTypLiczbowybezZnaku.One + jakisTypLiczbowybezZnaku.One;
        return suma / dwa;
    }
}

public abstract class Baza_Sortownikow
{
    public abstract void Sortuj(int[] tablica);
}

public class Bombel : Baza_Sortownikow //o, polimorfizm
{
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

public class Gnom : Baza_Sortownikow
{
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
                if (i > 0) i--;
            }
            else i++;
        }
    }
}

public class Bogosort : Baza_Sortownikow
{
    private readonly Random RNG = new();

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

            if (gotowe) break;

            for (uint i = 0; i < tablica.Length; i++)
            {
                int losowyIndeks = RNG.Next(tablica.Length);
                (tablica[i], tablica[losowyIndeks]) = (tablica[losowyIndeks], tablica[i]);
            }
        }
    }
}

public record struct WynikTestu
{
    public ulong Srednia { get; set; }
    public ulong Mediana { get; set; }
    public ulong Wariancja { get; set; }
    public ulong OdchylenieStandardowe { get; set; }
}

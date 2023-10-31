// See: https//aka.ms/new-console-template for more information
using System.Diagnostics;

namespace Sortowniki;

public class Program
{
    public const ushort iteracjeWTescie = 2500;

    Stopwatch unizegar = new();
    private static readonly Random randomizer = new();
    private int[] probnaTablica;
    public WynikTestu[] wyniki = new WynikTestu[iteracjeWTescie];
    private readonly List<BazaSortownikow> listaSortownikow = new()
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

        //Sortowniki.zapelnijLosowo();
        ZapelnijLosowo(sortowniki.probnaTablica);

        sortowniki.WypiszTablice();

        Console.WriteLine();

        Console.WriteLine("Sortowanie...");
        //Console.WriteLine();
        //Console.WriteLine(Sortowniki.tablica[1]);
        sortowniki.unizegar = Stopwatch.StartNew();
        //Sortowniki.bombelkuj();
        sortowniki.unizegar.Stop();
        var tBomblowania = sortowniki.unizegar.Elapsed;

        sortowniki.WypiszTablice();

        Console.WriteLine();

        Console.Write("T = ");
        Console.WriteLine(tBomblowania);


        ZapelnijLosowo(sortowniki.probnaTablica);

        //Console.WriteLine("Szybkosortowanie...");

        //Sortowniki.unizegar.Restart();
        //Sortowniki.szybkoSortuj();
        //Sortowniki.unizegar.Stop();

        //TimeSpan tSzybkoSortowania = Sortowniki.unizegar.Elapsed;

        //Sortowniki.wypiszTablice();
        //Console.Write("T = ");
        //Console.WriteLine(tSzybkoSortowania);

        //Console.WriteLine();
        Console.WriteLine();

        for (uint i = 3; i < iteracjeWTescie; i++)
        {
            sortowniki.probnaTablica = new int[i];
            ZapelnijLosowo(sortowniki.probnaTablica);

            sortowniki.unizegar.Restart();
            //Sortowniki.bombelkuj();
            sortowniki.unizegar.Stop();

            //Console.Write(i);
            //Console.Write("  ");
            //Console.WriteLine(Sortowniki.unizegar.Elapsed.TotalNanoseconds);

            sortowniki.wyniki[i] = new WynikTestu()
            {
                Średnia = (ulong)sortowniki.unizegar.Elapsed.TotalNanoseconds,
                Mediana = (ulong)sortowniki.unizegar.Elapsed.TotalNanoseconds,
                OdchylenieStandardowe = 0,
                Wariancja = 0,
            };
        }
        //Console.WriteLine("      [ns]");

        //Sortowniki.bogosortuj();
        //Sortowniki.wypiszTablice();

        Console.WriteLine();
        ZapelnijLosowo(sortowniki.probnaTablica);
        Console.WriteLine("Sortowanie...");

        //Sortowniki.gnomuj();

        Console.WriteLine();

        sortowniki.WypiszTablice();
        //Console.WriteLine();

        //Sortowniki.zapelnijLosowo();

        //Console.WriteLine();
        //Console.WriteLine(wyniki);

        Console.WriteLine("Naciśnij dowolny klawisz by zamknąć program...");
        Console.ReadKey();
    }

    public void WypiszTablice()
    {
        for (uint i = 0; i < probnaTablica.Length; i++)
        {
            Console.WriteLine(probnaTablica[i]);
        }
    }

    public static void ZapelnijLosowo(int[] tablica, int max = int.MaxValue)
    {
        for (int item = 0; item < tablica.Length; item++)
            tablica[item] = randomizer.Next(max);
    }

}

public abstract class BazaSortownikow
{
    public abstract void Sortuj(int[] tablica);
}

public class Bombel : BazaSortownikow
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

public class Gnom : BazaSortownikow
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

public class Bogosort : BazaSortownikow
{
    private readonly Random rng = new();

    public override void Sortuj(int[] tablica)
    {
        Bogosortuj(tablica);
    }

    public void Bogosortuj(int[] tablica)
    {
        while (true)
        {
            bool gotowe = true;

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
                int losowyIndeks = rng.Next(tablica.Length);
                (tablica[i], tablica[losowyIndeks]) = (tablica[losowyIndeks], tablica[i]);
            }
        }
    }

}

public record struct WynikTestu
{
    public ulong Średnia { get; set; }
    public ulong Mediana { get; set; }
    public ulong Wariancja { get; set; }
    public ulong OdchylenieStandardowe { get; set; }
}

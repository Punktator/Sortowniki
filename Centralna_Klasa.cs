//Tester sortowników
//WL 2023

using System.Diagnostics;
using System.Numerics;

namespace Sortowniki;

public class Centralna_Klasa
{
    public const ushort iteracjewTescie = 2500;

    Stopwatch unizegar = new();
    protected static readonly Random randomizer = new();
    public Wynik_Testu[] wyniki = new Wynik_Testu[iteracjewTescie];
    private readonly List<Baza_Sortownikow> listaSortownikow = new()
    {
        new Bombel(),
        new Bogosort(),
        new Gnom()
    };

    public static void Main()
    {
        Obsluga_Tablic inferfejsSortowniczy;
        var sortowniki = new Centralna_Klasa();

        Console.WriteLine("Podaj rozmiar tablicy: ");
        int n = int.Parse(Console.ReadLine());
        inferfejsSortowniczy = new Obsluga_Tablic(n);
        
        inferfejsSortowniczy.ZapelnijLosowo(inferfejsSortowniczy.tablica);

        inferfejsSortowniczy.WypiszTablice(inferfejsSortowniczy.tablica);

        Console.WriteLine();

        Console.WriteLine("Sortowanie...");
        sortowniki.unizegar = Stopwatch.StartNew();
        sortowniki.listaSortownikow[0].Sortuj(inferfejsSortowniczy.tablica);
        sortowniki.unizegar.Stop();
        TimeSpan tBomblowania = sortowniki.unizegar.Elapsed;

        inferfejsSortowniczy.WypiszTablice(inferfejsSortowniczy.tablica);

        Console.WriteLine();

        Console.Write("T = ");
        Console.WriteLine(tBomblowania);


        inferfejsSortowniczy.ZapelnijLosowo(inferfejsSortowniczy.tablica);

        Console.WriteLine();

        foreach (var sortownik in sortowniki.listaSortownikow)
        {
            for (uint i = 3; i < iteracjewTescie; i++)
            {
                inferfejsSortowniczy.tablica = new int[i];
                inferfejsSortowniczy.ZapelnijLosowo(inferfejsSortowniczy.tablica);

                sortowniki.unizegar.Restart();
                sortownik.Sortuj(inferfejsSortowniczy.tablica);
                sortowniki.unizegar.Stop();

                sortowniki.wyniki[i] = new Wynik_Testu()
                {
                    Srednia = Convert.ToUInt64(sortowniki.unizegar.Elapsed.TotalNanoseconds),
                    Mediana = Convert.ToUInt64(sortowniki.unizegar.Elapsed.TotalNanoseconds),
                    OdchylenieStandardowe = 0,
                    Wariancja = 0,
                };
            }
        }


        Console.WriteLine("Naciśnij dowolny klawisz by zamknąć program...");
        Console.ReadKey();
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

public record struct Wynik_Testu
{
    public ulong Srednia { get; set; }
    public ulong Mediana { get; set; }
    public ulong Wariancja { get; set; }
    public ulong OdchylenieStandardowe { get; set; }
}

class Obsluga_Tablic
{
    protected static readonly Random randomizer = new();
    public int[] tablica;

    public void WypiszTablice(int[] tablica)
    {
        for (uint i = 0; i < tablica.Length; i++)
        {
            Console.WriteLine(tablica[i]);
        }
    }

    public void ZapelnijLosowo(int[] tablica, int max = int.MaxValue)
    {
        for (int i = 0; i < tablica.Length; i++) tablica[i] = randomizer.Next(max);
    }

    public Obsluga_Tablic(int rozmiarTablicy) 
    {
        tablica = new int[rozmiarTablicy];
        ZapelnijLosowo(tablica);
    }
    
    public static pewienTypLiczbowybezZnaku Mediana<pewienTypLiczbowybezZnaku>(pewienTypLiczbowybezZnaku[] tab)
        where pewienTypLiczbowybezZnaku : IUnsignedNumber<pewienTypLiczbowybezZnaku> //oblicza medianę 
    {                                                                                //pól tablicy 
        int pul = tab.Length / 2;                                                    //dowolnego typu
        pewienTypLiczbowybezZnaku suma;                                              //przy użyciu interfejsów
                                                                                     //uogólnionych typów
        if (tab.Length % 2 != 0)                                                     //liczbowych bez znaku
            return tab[pul + 1];
        suma = tab[pul] + tab[pul + 1];
        pewienTypLiczbowybezZnaku dwa = pewienTypLiczbowybezZnaku.One + pewienTypLiczbowybezZnaku.One;
        return suma / dwa;
    }
}
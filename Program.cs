//Tester sortowników
//WL 2023

using System.Diagnostics;
using System.Numerics;
using System.Text;

namespace Sortowniki;

public class Centralna_Klasa
{
    public const ushort iteracjewTescie = 2500;

    protected static Stopwatch unizegar = new();
    public Wynik_Testu[] wyniki = new Wynik_Testu[iteracjewTescie];
    public static readonly Dictionary<string, Baza_Sortownikow> slownikSortownikuw = new()
    {
        {new Bombel().nazwa, new Bombel() },
        {new Gnom().nazwa, new Gnom()},
        {new SzybkoSort().nazwa, new SzybkoSort()},
        {new Bogosort().nazwa, new Bogosort()}
    };
    
    public static void Main()
    {
        Obsluga_Tablic inferfejsSortowniczy;

        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.WriteLine("Podaj rozmiar tablicy: ");
        Console.ResetColor();
        int n = int.Parse(Console.ReadLine());
        inferfejsSortowniczy = new Obsluga_Tablic(n);

        //inferfejsSortowniczy.WypiszTablice(inferfejsSortowniczy.tablica);

        Console.WriteLine();

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("Sortowanie...");
        Console.ResetColor();
        
        unizegar = Stopwatch.StartNew();
        slownikSortownikuw["Szybkie(medianowe)"].Sortuj(inferfejsSortowniczy.tablica);
        unizegar.Stop();
        TimeSpan tBomblowania = unizegar.Elapsed;

        Console.WriteLine(inferfejsSortowniczy.ToString(inferfejsSortowniczy.tablica));

        Console.WriteLine();

        Console.Write("T = ");
        Console.WriteLine(tBomblowania);


        Console.WriteLine();

        /*foreach (var sortownik in slownikSortownikuw)
        {
            for (uint i = 3; i < iteracjewTescie; i++)
            {
                inferfejsSortowniczy = new Obsluga_Tablic((int)i);

                unizegar.Restart();
                sortownik.Value.Sortuj(inferfejsSortowniczy.tablica);
                unizegar.Stop();

                sortowniki.wyniki[i] = new Wynik_Testu()
                {
                    Srednia = Convert.ToUInt64(unizegar.Elapsed.TotalNanoseconds),
                    Mediana = Convert.ToUInt64(unizegar.Elapsed.TotalNanoseconds),
                    OdchylenieStandardowe = 0,
                    Wariancja = 0,
                };
            }
        }*/


        Koniec();
    }

    private static void Koniec()
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Naciśnij dowolny klawisz by zamknąć program...");
        Console.ResetColor();
        Console.ReadKey();
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

    public string ToString(int[] tablica)
    {
        StringBuilder bufor = new();
        for (uint i = 0; i < tablica.Length; i++)
        {
            bufor.Append(tablica[i]);
            bufor.Append('\n');
        }
        return bufor.ToString();
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

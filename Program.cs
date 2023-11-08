//Tester sortowników
//WL 2023

using System.Diagnostics;
using System.Globalization;
using System.Numerics;
using System.Text;

namespace Sortowniki;

public class Centralna_Klasa
{
    public const uint ITERACJE_W_TESCIE = 2500;

    protected static Stopwatch unizegar = new();
    public Wynik_Testu[] wyniki = new Wynik_Testu[ITERACJE_W_TESCIE];
    public static readonly Dictionary<string, Baza_Sortownikow> slownikSortownikuw = new()
    {
        {new Bombel().nazwa, new Bombel() },
        {new Gnom().nazwa, new Gnom()},
        {new SzybkoSort().nazwa, new SzybkoSort()},
        {new Wstawianie().nazwa, new Wstawianie ()},
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

        Console.WriteLine(Obsluga_Tablic.ToString(inferfejsSortowniczy.tablica));

        Console.WriteLine();

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("Sortowanie...");
        Console.ResetColor();
        
        unizegar = Stopwatch.StartNew();
        slownikSortownikuw["Sortowanie przez wstawianie"].Sortuj(inferfejsSortowniczy.tablica);
        unizegar.Stop();
        TimeSpan tBomblowania = unizegar.Elapsed;

        Console.WriteLine(Obsluga_Tablic.ToString(inferfejsSortowniczy.tablica));

        Console.WriteLine();

        Console.Write("T = ");
        Console.WriteLine(tBomblowania);


        Console.WriteLine();

        /*foreach (var sortownik in slownikSortownikuw)
        {
            for (uint i = 3; i < ITERACJE_W_TESCIE; i++)
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


        KoniecProgramu();
    }

    private static void KoniecProgramu()
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

    public static string ToString(int[] tablica)
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
    
    public static pewienTypLiczbowyzeZnakiem Mediana<pewienTypLiczbowyzeZnakiem>(pewienTypLiczbowyzeZnakiem[] tab)
        where pewienTypLiczbowyzeZnakiem : ISignedNumber<pewienTypLiczbowyzeZnakiem> //oblicza medianę 
    {                                                                                //pól tablicy 
        int pul = tab.Length / 2;                                                    //dowolnego typu
        pewienTypLiczbowyzeZnakiem suma;                                              //przy użyciu interfejsów
                                                                                     //uogólnionych typów
        if (tab.Length % 2 != 0)                                                     //liczbowych bez znaku
            return tab[pul + 1];
        suma = tab[pul] + tab[pul + 1];
        pewienTypLiczbowyzeZnakiem dwa = pewienTypLiczbowyzeZnakiem.One + pewienTypLiczbowyzeZnakiem.One;
        return suma / dwa;
    }

//    public static pewienTypLiczbowyzeZnakiem Srednia<pewienTypliczbowyzeZnakiem>(pewienTypliczbowyzeZnakiem[] tab)
//        where pewienTypliczbowyzeZnakiem : ISignedNumber<pewienTypliczbowyzeZnakiem>
//    {
//        pewienTypliczbowyzeZnakiem suma = pewienTypliczbowyzeZnakiem.One;
//        pewienTypliczbowyzeZnakiem d = pewienTypliczbowyzeZnakiem.TryParse(tab.Length)
        
           
//    }
    
    public ulong SredniaArytmetycznaEle(ulong[] tablica)
    {
        ulong suma = 0;
        ulong srednia;

        for (uint i = 0; i<tablica.Length; i++)
            suma += tablica[i];

        srednia = suma / Convert.ToUInt64(tablica.Length);

        return srednia;
    }

    public ulong Wariancja(ulong[] tablica)
    {
        ulong czynnik;
        ulong srednia = SredniaArytmetycznaEle(tablica);
        ulong suma = 0;

        for (uint i = 0; i<tablica.Length; i++)
        {
            czynnik = tablica[i] - srednia;
            czynnik *= czynnik;
            suma += czynnik;
        }

        return suma / Convert.ToUInt64(tablica.LongLength);
    }
}
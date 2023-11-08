namespace Sortowniki;

public abstract class Baza_Sortownikow
{
    public abstract void Sortuj(int[] tablica);
    public abstract string nazwa { get; }
}

public class Bombel : Baza_Sortownikow //o, polimorfizm
{
    public override string nazwa => "Bąbelkowe";

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
    public override string nazwa => "Gnomowe";

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

public class SzybkoSort : Baza_Sortownikow
{
    public override string nazwa => "Szybkie(medianowe)";

    protected static void SzybkoSortuj1(int[] tab, int lewy, int prawy)
    {
        if (prawy <= lewy) return;

        int i = lewy - 1;
        int j = prawy + 1;
        int pivot = tab[(lewy + prawy) / 2]; //wybieramy punkt odniesienia

        while (true)
        {
            //szukam elementu większego lub równego pivot stojącego po prawej stronie wartości pivot
            while (pivot > tab[++i]) ;

            //szukam elementu mniejszego lub równego pivot stojąceg po lewej stronie wartości pivot
            while (pivot < tab[--j]) ;

            //jeśli liczniki się nie minęły to zamień elementy ze sobą stojące po niewłaściwej stronie pivot
            if (i <= j)
                (tab[i], tab[j]) = (tab[j], tab[i]);
            else break;
        }

        if (j > lewy)
            SzybkoSortuj1(tab, lewy, j);
        if (i < prawy)
            SzybkoSortuj1(tab, i, prawy);
    }

    public override void Sortuj(int[] tab)
    {
        SzybkoSortuj2(tab, 0, tab.Length - 1);  //lew = 0, a praw to ostatni indeks tablicy
    }

    protected static void SzybkoSortuj2(int[] tab, int lew, int praw)
    {
        if (lew >= praw) return;

        int IndeksPivota = Podziel(tab, lew, praw); // Podział tablicy względem pivotu
        SzybkoSortuj2(tab, lew, IndeksPivota - 1); // Rekurencyjne sortowanie lewej części
        SzybkoSortuj2(tab, IndeksPivota + 1, praw); // Rekurencyjne sortowanie prawej części
    }

    // wybiera pivot i przestawia elementy w odpowiednich miejscach
    private static int Podziel(int[] tab, int lew, int praw)
    {
        int pivot = tab[praw]; // Wybieramy pivot (zwykle ostatni element)
        int i = lew - 1;

        for (int j = lew; j < praw; j++)
        {
            if (tab[j] < pivot)
            {
                i++;
                (tab[i], tab[j]) = (tab[j], tab[i]);
            }
        }
        // Przestawienie pivotu na właściwe miejsce
        (tab[i + 1], tab[praw]) = (tab[praw], tab[i + 1]); 
        return i + 1;
    }
}

public class Wstawianie : Baza_Sortownikow
{
    public override string nazwa => "Sortowanie przez wstawianie";

    public void SortujWstawiajonc(int[] tablica) //nie działa
    {
        int i;
        for (int n = 1; n < tablica.Length; n++)
        {
            int a = tablica[n];
            for (i = n - 1; i >= 0 && tablica[i] > a; i--)
                tablica[i+1] = tablica[i];
            
            tablica[i + 1] = a;
        }
    }

    public override void Sortuj(int[] tablica)
    {
        SortujWstawiajonc(tablica);
    }
}

public class Bogosort : Baza_Sortownikow
{
    public override string nazwa => "Bogosort";
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
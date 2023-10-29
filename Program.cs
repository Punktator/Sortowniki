// See :https//aka.ms/new-console-template for more information
using System.Diagnostics;
using System.Threading;

namespace BenchmarkDotNet
{
    //[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single)]
    public class Program
    {
        public Int32[] tablica;
        public Int32 n;
        Stopwatch unizegar = new();
        /*readonly*/
        public Random randomizer = new Random();
        public object lista_sortownikow = new List<object> ();
        

        static void Main()
        {
            var Sortowniki = new Program();

            Console.WriteLine("Podaj rozmiar tablicy: ");
            Sortowniki.n = Int32.Parse(Console.ReadLine());

            Sortowniki.tablica = new Int32[Sortowniki.n];

            Sortowniki.zapelnijLosowo();

            Sortowniki.wypiszTablice();

            Console.WriteLine();

            Console.WriteLine("Sortowanie...");
            //Console.WriteLine();
            //Console.WriteLine(Sortowniki.tablica[1]);
            Sortowniki.unizegar = Stopwatch.StartNew();
            //Sortowniki.bombelkuj();
            Sortowniki.unizegar.Stop();
            var tBomblowania = Sortowniki.unizegar.Elapsed;

            Sortowniki.wypiszTablice();

            Console.WriteLine();

            Console.Write("T = ");
            Console.WriteLine(tBomblowania);


            Sortowniki.zapelnijLosowo();

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

            for (uint i = 3; i < Baza_Sortownikow.testy; i++)
            {
                Sortowniki.tablica = new int[i];
                Sortowniki.zapelnijLosowo();

                Sortowniki.unizegar.Restart();
                //Sortowniki.bombelkuj();
                Sortowniki.unizegar.Stop();

                //Console.Write(i);
                //Console.Write("  ");
                //Console.WriteLine(Sortowniki.unizegar.Elapsed.TotalNanoseconds);

                Baza_Sortownikow.wyniki[i] = Sortowniki.unizegar.Elapsed.TotalNanoseconds;
            }
            //Console.WriteLine("      [ns]");

            //Sortowniki.bogosortuj();
            //Sortowniki.wypiszTablice();

            Console.WriteLine();
            Sortowniki.zapelnijLosowo();
            Console.WriteLine("Sortowanie...");

            //Sortowniki.gnomuj();

            Console.WriteLine();

            Sortowniki.wypiszTablice();
            //Console.WriteLine();

            //Sortowniki.zapelnijLosowo();

            //Console.WriteLine();
            //Console.WriteLine(wyniki);

            Console.ReadKey();
        }

        void zapelnijLosowo(int max = Int32.MaxValue)
        {
            for (int item = 0; item < tablica.Length; item++) tablica[item] = randomizer.Next(max);
        }

        public void zamien(ref int a, ref int b)
        {
            int c;

            c = a;
            a = b;
            b = c;
        }

        public void wypiszTablice()
        {
            UInt32 i = 0;

            while (i < n)
            {
                Console.WriteLine(tablica[i]);
                i++;
            }
        }

        
        public void szybkosortuj()
        {
            uint pivot = Convert.ToUInt32 (tablica.Length / 2);
            uint i;
            uint j = Convert.ToUInt32(tablica.Length - 1);

            for (i=0; i < pivot; i++) { }

        }

        void szybkoSortuj(int lewy = 0)
        {
            int prawy = tablica.Length - 1;

            if (prawy <= lewy) return;

            int i = lewy - 1;
            int j = prawy + 1,
            pivot = tablica[(lewy + prawy) / 2]; //wybieramy punkt odniesienia

            while (true)
            {
                //szukam elementu wiekszego lub rownego piwot stojacego
                //po prawej stronie wartosci pivot
                while (pivot > tablica[++i]);

                //szukam elementu mniejszego lub rownego pivot stojacego
                //po lewej stronie wartosci pivot
                while (pivot < tablica[--j]) ;

                //jesli liczniki sie nie minely to zamień elementy ze soba
                //stojace po niewlasciwej stronie elementu pivot
                if (i <= j)
                    //funkcja swap zamienia wartosciami tab[i] z tab[j]
                    zamien(ref tablica[i], ref tablica[j]);
                else
                    break;
            }

            if (j > lewy)
                szybkoSortuj(lewy);
            if (i < prawy)
                szybkoSortuj(i);
        }

        public void wypelnijListe()
        {
            //lista_sortownikow.
        }
    }

    class Baza_Sortownikow
    {
        public const ushort testy = 2500;
        public static double[] wyniki = new double[testy];

        public virtual void sortuj()
        {
        }

        protected Program Sortowniki = new Program();
        
    }

    class Bombel : Baza_Sortownikow
    {
        public override void sortuj()
        {
            bombelkuj();
        }

        public void bombelkuj()
        {
            bool flaga;
            do
            {
                flaga = false;
                for (UInt32 i = 0; i < Sortowniki.tablica.Length - 1; i++)
                {
                    if (Sortowniki.tablica[i] > Sortowniki.tablica[i + 1])
                    {
                        Sortowniki.zamien(ref Sortowniki.tablica[i], ref Sortowniki.tablica[i + 1]);
                        flaga = true;
                    }
                }
            } while (flaga);
        }
    }

    class Gnom : Baza_Sortownikow
    {
        public override void sortuj()
        {
            gnomuj();
        }

        public void gnomuj()
        {
            uint i = 0;
            while (i < Sortowniki.tablica.Length - 1)
            {
                if (Sortowniki.tablica[i] > Sortowniki.tablica[i + 1])
                {
                    (Sortowniki.tablica[i], Sortowniki.tablica[i + 1]) = (Sortowniki.tablica[i + 1], Sortowniki.tablica[i]);
                    if (i > 0) i--;
                }
                else i++;
            }
        }
    }

    class Bogosort : Baza_Sortownikow
    { 
        public override void sortuj() 
        {
            bogosortuj();
        }

        public void bogosortuj()
        {
            while (true)
            {
                bool gotowe = true;

                for (uint i = 1; i < Sortowniki.n; i++)
                {
                    if (Sortowniki.tablica[i - 1] > Sortowniki.tablica[i])
                    {
                        gotowe = false;
                        break;
                    }
                }

                if (gotowe) break;

                for (uint i = 0; i < Sortowniki.n; i++)
                {
                    Sortowniki.zamien(ref Sortowniki.tablica[i], ref Sortowniki.tablica[Sortowniki.randomizer.Next(Sortowniki.n)]);
                    
                }
            }
        }

    }
}
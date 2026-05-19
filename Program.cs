using System;
using System.IO;

namespace AmebaJatek
{
    class Program
    {
        static string[,] tabla = null;
        static int meret = 0;
        static string kovetkezoJatekos = "X";
        static int lepesekSzama = 0;

        static int lejatszottMeccsek = 0;
        static int osszesLepes = 0;

        static void Main(string[] args)
        {
            bool fut = true;
            Console.Clear();

            while (fut)
            {
                Console.WriteLine("====== AMŐBA ======");
                Console.WriteLine("1. Új játék");
                Console.WriteLine("2. Játék mentése");
                Console.WriteLine("3. Játék betöltése");
                Console.WriteLine("4. X lépés");
                Console.WriteLine("5. O lépés");
                Console.WriteLine("6. Statisztika");
                Console.WriteLine("0. Kilépés");
                Console.WriteLine("==============================");
                Console.Write("Kérem válasszon a menüpontok közül (0-6): ");

                string valasztas = Console.ReadLine();
                Console.WriteLine();

                switch (valasztas)
                {
                    case "1": UjJatek(); break;
                    case "2": JatekMentese(); break;
                    case "3": JatekBetoltese(); break;
                    case "4": Lepes("X"); break;
                    case "5": Lepes("O"); break;
                    case "6": MutasdStatisztika(); break;
                    case "0":
                        Console.WriteLine("Köszönöm a játékot! Viszlát!");
                        fut = false;
                        break;
                    default:
                        Console.WriteLine("Érvénytelen választás, kérlek 0 és 6 között válassz!");
                        break;
                }

                if (fut)
                {
                    Console.WriteLine("\nNyomj meg egy gombot a menühöz való visszatéréshez...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }

        static void TablaRajzolas()
        {
            if (tabla == null)
            {
                Console.WriteLine("Nincs aktív játék. Kezdj egy új játékot!");
                return;
            }

            Console.Write("\n    ");
            for (int i = 0; i < meret; i++)
            {
                int oszlopSzam = i + 1;
                if (oszlopSzam < 10)
                {
                    Console.Write("0" + oszlopSzam + " ");
                }
                else
                {
                    Console.Write(oszlopSzam + " ");
                }
            }
            Console.WriteLine("\n   " + new string('-', meret * 3));

            for (int i = 0; i < meret; i++)
            {
                int sorSzam = i + 1;
                if (sorSzam < 10)
                {
                    Console.Write("0" + sorSzam + " | ");
                }
                else
                {
                    Console.Write(sorSzam + " | ");
                }

                for (int j = 0; j < meret; j++)
                {
                    Console.Write(tabla[i, j] + "  ");
                }
                Console.WriteLine();
            }
        }

        static void UjJatek()
        {
            while (true)
            {
                try
                {
                    Console.Write("Adja meg a tábla méretét,legalább 3: ");
                    meret = int.Parse(Console.ReadLine());

                    if (meret >= 3)
                    {
                        break;
                    }
                    Console.WriteLine("A tábla mérete legalább 3-as kell legyen!");
                }
                catch
                {
                    Console.WriteLine("Hiba! Kérlek egy érvényes egész számot adj meg!");
                }
            }

            tabla = new string[meret, meret];
            for (int i = 0; i < meret; i++)
            {
                for (int j = 0; j < meret; j++)
                {
                    tabla[i, j] = ".";
                }
            }

            kovetkezoJatekos = "X";
            lepesekSzama = 0;
            lejatszottMeccsek++;

            Console.WriteLine($"\nÚj {meret}x{meret}-es játék létrehozva! 'X' kezd.");
            TablaRajzolas();
        }

        static void JatekMentese()
        {
            if (tabla == null)
            {
                Console.WriteLine("Nincs mit menteni, nincs aktív játék!");
                return;
            }

            try
            {
                using (StreamWriter sw = new StreamWriter("amoba_mentes.txt"))
                {
                    sw.WriteLine(meret);
                    sw.WriteLine(kovetkezoJatekos);
                    sw.WriteLine(lepesekSzama);

                    sw.WriteLine(lejatszottMeccsek);
                    sw.WriteLine(osszesLepes);

                    for (int i = 0; i < meret; i++)
                    {
                        for (int j = 0; j < meret; j++)
                        {
                            sw.Write(tabla[i, j]);
                        }
                        sw.WriteLine();
                    }
                }
                Console.WriteLine("Játék sikeresen mentve az 'amoba_mentes.txt' fájlba!");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Hiba történt a mentés során: {e.Message}");
            }
        }

        static void JatekBetoltese()
        {
            if (!File.Exists("amoba_mentes.txt"))
            {
                Console.WriteLine("Nem található mentett állás (amoba_mentes.txt)!");
                return;
            }

            try
            {
                using (StreamReader sr = new StreamReader("amoba_mentes.txt"))
                {
                    meret = int.Parse(sr.ReadLine());
                    kovetkezoJatekos = sr.ReadLine();
                    lepesekSzama = int.Parse(sr.ReadLine());

                    lejatszottMeccsek = int.Parse(sr.ReadLine());
                    osszesLepes = int.Parse(sr.ReadLine());

                    tabla = new string[meret, meret];
                    for (int i = 0; i < meret; i++)
                    {
                        string sor = sr.ReadLine();
                        for (int j = 0; j < meret; j++)
                        {
                            tabla[i, j] = sor[j].ToString();
                        }
                    }
                }
                Console.WriteLine("Játék sikeresen betöltve!");
                TablaRajzolas();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Hiba történt a betöltés során: {e.Message}");
            }
        }

        static void Lepes(string jatekos)
        {
            if (tabla == null)
            {
                Console.WriteLine("Előbb indíts egy új játékot vagy tölts be egyet!");
                return;
            }

            if (kovetkezoJatekos != jatekos)
            {
                Console.WriteLine($"Nem az '{jatekos}' játékos következik! Most '{kovetkezoJatekos}' jön.");
                return;
            }

            TablaRajzolas();
            Console.WriteLine($"\n--- {jatekos} lépése ---");
            int sor = 0, oszlop = 0;

            while (true)
            {
                try
                {
                    Console.Write($"Adja meg a sort (1-{meret}): ");
                    sor = int.Parse(Console.ReadLine());

                    Console.Write($"Adja meg az oszlopot (1-{meret}): ");
                    oszlop = int.Parse(Console.ReadLine());

                    sor--; oszlop--;

                    if (sor >= 0 && sor < meret && oszlop >= 0 && oszlop < meret)
                    {
                        if (tabla[sor, oszlop] == ".")
                        {
                            break;
                        }
                        Console.WriteLine("Ez a hely már foglalt!");
                    }
                    else
                    {
                        Console.WriteLine("A koordináta kívül esik a táblán!");
                    }
                }
                catch
                {
                    Console.WriteLine("számokat adj meg.");
                }
            }

            tabla[sor, oszlop] = jatekos;
            lepesekSzama++;
            osszesLepes++;

            Console.Clear();
            Console.WriteLine("Sikeres lépés!");
            TablaRajzolas();

            if (lepesekSzama == meret * meret)
            {
                Console.WriteLine("\nJÁTÉK VÉGE! Betelt a tábla.");
                tabla = null;
                return;
            }

            if (jatekos == "X")
            {
                kovetkezoJatekos = "O";
            }
            else
            {
                kovetkezoJatekos = "X";
            }
        }

        static void MutasdStatisztika()
        {
            Console.WriteLine("========== STATISZTIKA ==========");
            Console.WriteLine($"Lejátszott/Elkezdett meccsek száma:  {lejatszottMeccsek}");
            Console.WriteLine($"A program során megtett összes lépés: {osszesLepes}");
            Console.WriteLine("=================================");
        }
    }
}
using PudelkoNamespace.Enums;
using PudelkoNamespace.PudelkoLib;
using System.Globalization;
using System.Security.Cryptography;

namespace ConsoleApp1
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //dla pudełka o wymiarach kolejno 2.5, 9.321 i 0.1, ToString("mm") zwraca napis "2500 mm × 9321 mm × 100 mm"
               
                CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");


                List<Pudelko> list = new();

                list.Add(new Pudelko(0.03, 0.01, 0.01,UnitOfMeasure.meter));
                list.Add(new Pudelko(0.005, 0.03, 0.02, UnitOfMeasure.meter));
                


                
                foreach(Pudelko p in list)
                {
                    Console.WriteLine($"{p}, obj: {p.Objetosc}, pole: {p.Pole}, sumakrawedzi: {p.SumaKrawedzi}.");
                }
                list.Sort(CompareBox);
                Console.WriteLine();
                foreach (Pudelko p in list)
                {
                    Console.WriteLine($"{p}, obj: {p.Objetosc}, pole: {p.Pole}, sumakrawedzi: {p.SumaKrawedzi}.");
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
        }

        static int CompareBox(Pudelko x, Pudelko y)
        {
            if (x.Objetosc > y.Objetosc)
                return 1;
            else if (x.Objetosc < y.Objetosc)
                return -1;
            else
            {
                if(x.Pole > y.Pole) return 1;
                else if(x.Pole < y.Pole) return -1;
                else
                {
                    if (x.SumaKrawedzi > y.SumaKrawedzi) return 1;
                    else if (x.SumaKrawedzi < y.SumaKrawedzi) return -1;
                    else return 0;
                }
            }
        }
    }
}
using PudelkoNamespace.Enums;
using PudelkoNamespace.PudelkoLib;
using System.Globalization;
using System.Security.Cryptography;
using static PudelkoNamespace.PudelkoLib.Pudelko;

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

                Pudelko pudlo = new();

                List<Pudelko> list = new();
                list.Add(new Pudelko(5, 3, 2, UnitOfMeasure.meter));
                list.Add(new Pudelko(0.03, 0.01, 0.01,UnitOfMeasure.meter));
                
                


                
                foreach(Pudelko p in list)
                {
                    Console.WriteLine($"{p}, obj: {p.Objetosc}, pole: {p.Pole}, sumakrawedzi: {p.SumaKrawedzi}.");
                }

                list.Sort(new Pudelko());

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

        
    }
}
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

                Pudelko abc = new Pudelko(1.025, 1.23, 2.33);
                Pudelko abc1 = new Pudelko(1.23, 2.33, 1.025);
                Console.WriteLine(abc.Objetosc);
                Console.WriteLine(abc.Pole);

                Console.WriteLine(abc.A);
                Console.WriteLine(abc.B);

                Console.WriteLine(abc.C);

                string abcd = "2.69999";
                Console.WriteLine(Convert.ToDouble(abcd));

                Console.WriteLine(abc == abc1);


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
        }
    }
}
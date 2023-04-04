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
                //dla pudełka o wymiarach kolejno 2.5, 9.321 i 0.1, ToString("cm") zwraca napis "250.0 cm × 932.1 cm × 10.0 cm"

                CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");

                Pudelko abc = new Pudelko(2.5, 9.321, 0.1);
                Console.WriteLine(abc.ToString("cm"));

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
        }
    }
}
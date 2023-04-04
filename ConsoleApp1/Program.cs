using PudelkoNamespace.Enums;
using PudelkoNamespace.PudelkoLib;

namespace ConsoleApp1
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //dla pudełka o wymiarach kolejno 2.5, 9.321 i 0.1, ToString("m") zwraca napis "2.500 m × 9.321 m × 0.100 m"


                Pudelko abc = new Pudelko(2.5, 9.321, 0.1);
                Console.WriteLine(abc.ToString("m"));

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
        }
    }
}
﻿using PudelkoNamespace.Enums;
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


                //[DataRow(100, 240, 360, 3600, 1000, 2400)]
               // [DataRow(25.5, 27.4, 75.3, 255, 274, 753)]
                Pudelko abc = new (7, 7, 4, UnitOfMeasure.meter);
                Pudelko abc1 = new (5, 2, 8, UnitOfMeasure.meter);

                

                var pudelko = Pudelko.Parse("1 mm x 1 m x 1 mm");
                System.Console.WriteLine(Pudelko.Kompresuj(pudelko));

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
        }
    }
}
using PudelkoNamespace.Enums;
using System.Collections;
using System.Collections.Immutable;
using System.Data.Common;
using System.Drawing;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;

namespace PudelkoNamespace.PudelkoLib
{

    public sealed class Pudelko : IFormattable, IEquatable<Pudelko>, IEnumerable<double>
    {
        private double _a; 
        private double _b; 
        private double _c;

        //indexer
        public double this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return A;
                    case 1:
                        return B;
                    case 2:
                        return C;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }

        public  double A
        {
            get 
            { 
                return ReturnMeters(_a,Measure); 
            }
        }
        public  double B 
        { 
            get 
            {
                return ReturnMeters(_b, Measure);
            }
        }
        public  double C 
        { 
            get 
            {
                return ReturnMeters(_c, Measure);
            }
        }

        public double Objetosc
        {
            get
            {
                return Math.Round(_c * _b * _a, 9);
            }
        }

        public double Pole
        {
            get
            {
                return Math.Round((_a * _b * 2)+ (_c * _b * 2) + (_c * _a * 2), 6);
            }
        }

        public UnitOfMeasure Measure { get; set; }

        public Pudelko(double a, double b, double c, UnitOfMeasure type)
        {
            if (!Enum.IsDefined(typeof(UnitOfMeasure), type))
            {
                 throw new FormatException("Invalid type for MeasureType enum.");
            }

            Measure = type;

            if (ReturnMeters(a, type) <= 0 || ReturnMeters(b, type) <= 0 || ReturnMeters(c, type) <= 0)
            {
                throw new ArgumentOutOfRangeException("Dimensions of the box must be positive!");
            }

            if (ReturnMeters(a,type) > 10 || ReturnMeters(b, type) > 10 || ReturnMeters(c, type) > 10) {
                throw new ArgumentOutOfRangeException("Box is too big! Max: 10x10x10 Meters.");
            }

            _a = a;
            _b = b;
            _c = c;
        }

        public Pudelko(double A, double B, double C) : this(A, B, C, UnitOfMeasure.meter)
        {
        }

        public Pudelko(double A, double B, UnitOfMeasure type = UnitOfMeasure.meter) : this(A,B,10, type)
        {
            if (type == UnitOfMeasure.meter) _c = _c / 100;

            if (type == UnitOfMeasure.milimeter) _c = _c * 10;
        }

        public Pudelko(double A, double B) : this(A, B, 10, UnitOfMeasure.meter)
        {
             _c= _c/100;
        }

        public Pudelko(double A) : this(A, 10, 10, UnitOfMeasure.meter)
        {
            _c = _c / 100;
            _b = _b / 100;
        }

        public Pudelko(double A, UnitOfMeasure type = UnitOfMeasure.centimeter) : this(A, 10, 10, type)
        {
            if (type == UnitOfMeasure.meter)
            {
                _c = _c / 100;
                _b = _b / 100;
            }
            else if (type == UnitOfMeasure.milimeter)
            {
                _c = _c * 10;
                _b = _b * 10;
            }
        }

        public Pudelko(UnitOfMeasure type = UnitOfMeasure.centimeter) : this(10, 10, 10, type)
        {
            if (type == UnitOfMeasure.meter)
            {
                _c = _c / 100;
                _b = _b / 100;
                _a = _a / 100;
                
            }
            else if (type == UnitOfMeasure.milimeter)
            {
                _c = _c * 10;
                _b = _b * 10;
                _a = _a * 10;
            }
        }

        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        // reprezentacja tekstowa obiektu i przeciążenia
        public override string ToString()
        {
            if(Measure == UnitOfMeasure.centimeter)
            {
                return string.Format($"{A * 100:F3} cm × {B * 100:F3} cm × {C * 100:F3} cm");

            }
            else if (Measure == UnitOfMeasure.milimeter)
            {
              return  string.Format($"{A* 1000:F3} mm × {B* 1000 :F3} mm × {C * 1000:F3} mm");

            }
            else if (Measure == UnitOfMeasure.meter)
            {
                return string.Format($"{A:F3} m × {B:F3} m × {C:F3} m");
            }
            else
            {
                throw new FormatException("Bad format, avaible formats: 'mm', 'cm', 'm'.");
            }
        }

        public string ToString(string format)
        {
            if (format == "cm")
            {
                return string.Format($"{A * 100:F1} cm × {B * 100:F1} cm × {C * 100:F1} cm");

            }
            else if (format == "mm")
            {
                return string.Format($"{A * 1000:F0} mm × {B * 1000:F0} mm × {C * 1000:F0} mm");

            }
            else if (format == "m")
            {
                return string.Format($"{A:F3} m × {B:F3} m × {C:F3} m");

            }
            else if(format == null)
            {
                return string.Format($"{A:F3} m × {B:F3} m × {C:F3} m");
            }
            else
            {
                throw new FormatException("Bad format, avaible formats: 'mm', 'cm', 'm'.");
            }
        }

        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            if (string.IsNullOrEmpty(format)) format = "m";
            if (formatProvider == null) formatProvider = CultureInfo.CurrentCulture;

            switch (format.ToUpperInvariant())
            {      
                case "mm":
                    return string.Format($"{A * 1000:F0} mm × {B * 1000:F0} mm × {C * 1000:F0} mm");
                case "cm":
                    return string.Format($"{A * 100:F1} cm × {B * 100:F1} cm × {C * 100:F1} cm");
                case "m":
                    return string.Format($"{A:F3} m × {B:F3} m × {C:F3} m");
                default:
                    return ToString();
            }
        }

        // funkcja pomocnicza zwracajaca metry
        public double ReturnMeters(double value, UnitOfMeasure m)
        {
            if (m == UnitOfMeasure.meter)
            {
                return Math.Round(value,14);
            }
            else if (m == UnitOfMeasure.milimeter)
            {
                if (value < 1) return 0;

                return Math.Round(value / 1000,14);
            }
            else if (m == UnitOfMeasure.centimeter)
            {
                if (value < 0.1) return 0;

                return Math.Round(value / 100, 14);
            }
            else throw new FormatException("Invalid type for MeasureType enum.");
        }

        // operator równości
        public bool Equals(Pudelko? other)
        {
            if (other == null) return false;

            double[] first = new double[3];
            double[] second = new double[3];

            first[0] = this.A;
            first[1] = this.B; 
            first[2] = this.C;

            second[0] =  other.A;
            second[1] =  other.B;
            second[2] =  other.C;

            Console.WriteLine($"{first[0]}, {second[0]}");
            Console.WriteLine($"{first[1]}, {second[1]}");
            Console.WriteLine($"{first[2]}, {second[2]}");

            Array.Sort(first);
            Array.Sort(second);

            //Console.WriteLine($"{first[0]}, {second[0]}");


            if (first[0] == second[0] && first[1] == second[1] && first[2] == second[2]) return true;
            else return false;
        }

        public IEnumerator<double> GetEnumerator()
        {
            yield return A;
            yield return B;
            yield return C;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static bool operator ==(Pudelko? a, Pudelko? b)
        {
            if (a is null || b is null) return false; 
            return a.Equals(b);
        }

        public static bool operator !=(Pudelko? a, Pudelko? b)
        {
            if (a is null || b is null) return false;
            return !a.Equals(b);
        }

        // operator dodawania dwóch pudełek
        public static Pudelko operator +(Pudelko a, Pudelko b)
        {
            double objetosc = 2000000000000;
            double x = 0, y = 0, z = 0;

            // A: a i b, B: a:0 i b:1
            if (Math.Max(a[0], a[1]) >= Math.Max(b[0], b[1]) && Math.Min(a[0], a[1]) >= Math.Min(b[0], b[1]))
            {
                if((a[0] * a[1] * (a[2] + b[2])) < objetosc)
                {
                    objetosc = a[0] * a[1] * (a[2] + b[2]);
                    x = a[0]; y = a[1]; z = a[2] + b[2];
                    Console.WriteLine($"{x}, {y}, {z},  {objetosc}");
                }
            }

            // A: a i b, B: a:0 i c:2
            if (Math.Max(a[0], a[1]) >= Math.Max(b[0], b[2]) && Math.Min(a[0], a[1]) >= Math.Min(b[0], b[2]))
            {
                if ((a[0] * a[1] * (a[2] + b[1])) < objetosc)
                {
                    objetosc = a[0] * a[1] * (a[2] + b[1]);
                    x = a[0]; y = a[1]; z = a[2] + b[1];
                    Console.WriteLine($"{x}, {y}, {z},  {objetosc}");
                }
            }

            // A: a i b, B: b:1 i c:2
            if (Math.Max(a[0], a[1]) >= Math.Max(b[1], b[2]) && Math.Min(a[0], a[1]) >= Math.Min(b[1], b[2]))
            {
                if ((a[0] * a[1] * (a[2] + b[0])) < objetosc)
                {
                    objetosc = a[0] * a[1] * (a[2] + b[0]);
                    x = a[0]; y = a[1]; z = a[2] + b[0];
                    Console.WriteLine($"{x}, {y}, {z},  {objetosc}");
                }
            }

            // A: b i c, B: a:0 i b:1
            if (Math.Max(a[1], a[2]) >= Math.Max(b[0], b[1]) && Math.Min(a[1], a[2]) >= Math.Min(b[0], b[1]))
            {
                if ((a[1] * a[2] * (a[0] + b[2])) < objetosc)
                {
                    objetosc = a[1] * a[2] * (a[0] + b[2]);
                    x = a[1]; y = a[2]; z = a[0] + b[2];
                    Console.WriteLine($"{x}, {y}, {z},  {objetosc}");
                }
            }

            // A: b i c, B: a:0 i c:2
            if (Math.Max(a[1], a[2]) >= Math.Max(b[0], b[2]) && Math.Min(a[1], a[2]) >= Math.Min(b[0], b[2]))
            {
                if ((a[1] * a[2] * (a[0] + b[1])) < objetosc)
                {
                    objetosc = a[1] * a[2] * (a[0] + b[1]);
                    x = a[1]; y = a[2]; z = a[0] + b[1];
                    Console.WriteLine($"{x}, {y}, {z},  {objetosc}");
                }
            }

            // A: b i c, B: b:1 i c:2
            if (Math.Max(a[1], a[2]) >= Math.Max(b[1], b[2]) && Math.Min(a[1], a[2]) >= Math.Min(b[1], b[2]))
            {
                if ((a[1] * a[2] * (a[0] + b[0])) < objetosc)
                {
                    objetosc = a[1] * a[2] * (a[0] + b[0]);
                    x = a[1]; y = a[2]; z = a[0] + b[0];
                    Console.WriteLine($"{x}, {y}, {z}, {objetosc}");
                }
            }

            // A: a i c, B: a:0 i b:1
            if (Math.Max(a[0], a[2]) >= Math.Max(b[0], b[1]) && Math.Min(a[0], a[2]) >= Math.Min(b[0], b[1]))
            {
                if ((a[0] * a[2] * (a[1] + b[2])) < objetosc)
                {
                    objetosc = a[0] * a[2] * (a[1] + b[2]);
                    x = a[0]; y = a[2]; z = a[1] + b[2];
                    Console.WriteLine($"{x}, {y}, {z}, {objetosc}");
                }
            }
            // A: a i c, B: a:0 i c:2
            if (Math.Max(a[0], a[2]) >= Math.Max(b[0], b[2]) && Math.Min(a[0], a[2]) >= Math.Min(b[0], b[2]))
            {
                if ((a[0] * a[2] * (a[1] + b[1])) < objetosc)
                {
                    objetosc = a[0] * a[2] * (a[1] + b[1]);
                    x = a[0]; y = a[2]; z = a[1] + b[1];
                    Console.WriteLine($"{x}, {y}, {z}, {objetosc}");
                }
            }

            // A: a i c, B: b:1 i c:2
            if (Math.Max(a[0], a[2]) >= Math.Max(b[1], b[2]) && Math.Min(a[0], a[2]) >= Math.Min(b[1], b[2]))
            {
                if ((a[0] * a[2] * (a[1] + b[0])) < objetosc)
                {
                    objetosc = a[0] * a[2] * (a[1] + b[0]);
                    x = a[0]; y = a[2]; z = a[1] + b[0];
                    Console.WriteLine($"{x}, {y}, {z}, {objetosc}");
                }
            }
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            // B: a i b, A: a:0 i b:1
            if (Math.Max(b[0], b[1]) >= Math.Max(a[0], a[1]) && Math.Min(b[0], b[1]) >= Math.Min(a[0], a[1]))
            {
                if ((b[0] * b[1] * (b[2] + a[2])) < objetosc)
                {
                    objetosc = b[0] * b[1] * (b[2] + a[2]);
                    x = b[0]; y = b[1]; z = b[2] + a[2];
                    Console.WriteLine($"{x}, {y}, {z},  {objetosc}");
                }
            }

            // B: a i b, A: a:0 i c:2
            if (Math.Max(b[0], b[1]) >= Math.Max(a[0], a[2]) && Math.Min(b[0], b[1]) >= Math.Min(a[0], a[2]))
            {
                if ((b[0] * b[1] * (b[2] + a[1])) < objetosc)
                {
                    objetosc = b[0] * b[1] * (b[2] + a[1]);
                    x = b[0]; y = b[1]; z = b[2] + a[1];
                    Console.WriteLine($"{x}, {y}, {z},  {objetosc}");
                }
            }

            // B: a i b, A: b:1 i c:2
            if (Math.Max(b[0], b[1]) >= Math.Max(a[1], a[2]) && Math.Min(b[0], b[1]) >= Math.Min(a[1], a[2]))
            {
                if ((b[0] * b[1] * (b[2] + a[0])) < objetosc)
                {
                    objetosc = b[0] * b[1] * (b[2] + a[0]);
                    x = b[0]; y = b[1]; z = b[2] + a[0];
                    Console.WriteLine($"{x}, {y}, {z},  {objetosc}");
                }
            }
            
            // B: b i c, A: a:0 i b:1
            if (Math.Max(b[1], b[2]) >= Math.Max(a[0], a[1]) && Math.Min(b[1], b[2]) >= Math.Min(a[0], a[1]))
            {
                if ((b[1] * b[2] * (b[0] + a[2])) < objetosc)
                {
                    objetosc = b[1] * b[2] * (b[0] + a[2]);
                    x = b[1]; y = b[2]; z = b[0] + a[2];
                    Console.WriteLine($"{x}, {y}, {z},  {objetosc}");
                }
            }
            // B: b i c, A: a:0 i c:2
            if (Math.Max(b[1], b[2]) >= Math.Max(a[0], a[2]) && Math.Min(b[1], b[2]) >= Math.Min(a[0], a[2]))
            {
                if ((b[1] * b[2] * (b[0] + a[1])) < objetosc)
                {
                    objetosc = b[1] * b[2] * (b[0] + a[1]);
                    x = b[1]; y = b[2]; z = b[0] + a[1];
                    Console.WriteLine($"{x}, {y}, {z},  {objetosc}");
                }
            }
            // B: b i c, A: b:1 i c:2
            if (Math.Max(b[1], b[2]) >= Math.Max(a[1], a[2]) && Math.Min(b[1], b[2]) >= Math.Min(a[1], a[2]))
            {
                if ((b[1] * b[2] * (b[0] + a[0])) < objetosc)
                {
                    objetosc = b[1] * b[2] * (b[0] + a[0]);
                    x = b[1]; y = b[2]; z = b[0] + a[0];
                    Console.WriteLine($"{x}, {y}, {z}, {objetosc}");
                }
            }

            // B: a i c, A: a:0 i b:1
            if (Math.Max(b[0], b[2]) >= Math.Max(a[0], a[1]) && Math.Min(b[0], b[2]) >= Math.Min(a[0], a[1]))
            {
                if ((b[0] * b[2] * (b[1] + a[2])) < objetosc)
                {
                    objetosc = b[0] * b[2] * (b[1] + a[2]);
                    x = b[0]; y = b[2]; z = b[1] + a[2];
                    Console.WriteLine($"{x}, {y}, {z}, {objetosc}");
                }
            }
            // B: a i c, A: a:0 i c:2
            if (Math.Max(b[0], b[2]) >= Math.Max(a[0], a[2]) && Math.Min(b[0], b[2]) >= Math.Min(a[0], a[2]))
            {
                if ((b[0] * b[2] * (b[1] + a[1])) < objetosc)
                {
                    objetosc = b[0] * b[2] * (b[1] + a[1]);
                    x = b[0]; y = b[2]; z = b[1] + a[1];
                    Console.WriteLine($"{x}, {y}, {z}, {objetosc}");
                }
            }
            // B: a i c, A: b:1 i c:2
            if (Math.Max(b[0], b[2]) >= Math.Max(a[1], a[2]) && Math.Min(b[0], b[2]) >= Math.Min(a[1], a[2]))
            {
                if ((b[0] * b[2] * (b[1] + a[0])) < objetosc)
                {
                    objetosc = b[0] * b[2] * (b[1] + a[0]);
                    x = b[0]; y = b[2]; z = b[1] + a[0];
                    Console.WriteLine($"{x}, {y}, {z}, {objetosc}");
                }
            }

            return new Pudelko(x,y,z);
        }

        public static explicit operator double[](Pudelko obj) => new double[] { obj.A, obj.B, obj.C };

        public static implicit operator Pudelko((int x, int y, int z) tuple) => new Pudelko(tuple.x, tuple.y, tuple.z, UnitOfMeasure.milimeter);

        public static Pudelko Parse(string text)
        {
            var values = text.Split(' ', 'x', ' ');
            Console.WriteLine(values.Length);            
            string[] valuesTemp = new string[6];

            int i = 0;
            foreach(var val in values)
            {
                if (!string.IsNullOrWhiteSpace(val))
                {
                    valuesTemp[i++] = val;
                }
            }

            if (valuesTemp.Length != 6)
            {
                throw new ArgumentException("Invalid text format", nameof(text));
            }

            double.TryParse(valuesTemp[0], out double a);
            double.TryParse(valuesTemp[2], out double b);
            double.TryParse(valuesTemp[4], out double c);

            string unitA = valuesTemp[1];
            string unitB = valuesTemp[3];
            string unitC = valuesTemp[5];

            return new Pudelko(
                a * ParseUnit(unitA),
                b * ParseUnit(unitB),
                c * ParseUnit(unitC)
            );
        }

        private static double ParseUnit(string unit)
        {
            switch (unit)
            {
                case "m":
                    return 1.0;
                case "cm":
                    return 0.01;
                case "mm":
                    return 0.001;
                default:
                    throw new ArgumentException($"Unknown unit: {unit}");
            }
        }
    }
}
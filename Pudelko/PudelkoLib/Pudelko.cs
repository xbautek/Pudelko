﻿using PudelkoNamespace.Enums;
using System.Collections;
using System.Collections.Immutable;
using System.Data.Common;
using System.Drawing;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;

namespace PudelkoNamespace.PudelkoLib
{

    public sealed class Pudelko : IFormattable, IEquatable<Pudelko>, IEnumerable<double>, IComparer<Pudelko>
    {
        private readonly double _a; 
        private readonly double _b; 
        private readonly double _c;

        //indexer
        public double this[int index]
        {
            get
            {
                return index switch
                {
                    0 => A,
                    1 => B,
                    2 => C,
                    _ => throw new IndexOutOfRangeException(),
                };
            }
        }

        public  double A => ReturnMeters(_a, Measure);
        public  double B => ReturnMeters(_b, Measure);
        public  double C => ReturnMeters(_c, Measure);
        public UnitOfMeasure Measure { get; set; }

        public double Objetosc => Math.Round(A * B * C, 9);
        public double Pole => Math.Round((A * B * 2) + (A * C * 2) + (B * C * 2), 6);
        public double SumaKrawedzi => A+B+C;

        

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
                throw new ArgumentOutOfRangeException("Box is too big! Maximum dimensions: 10x10x10 Meters.");
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
            if (type == UnitOfMeasure.meter) _c /= 100;

            if (type == UnitOfMeasure.milimeter) _c *= 10;
        }

        public Pudelko(double A, double B) : this(A, B, 10, UnitOfMeasure.meter)
        {
             _c /= 100;
        }

        public Pudelko(double A) : this(A, 10, 10, UnitOfMeasure.meter)
        {
            _c /= 100;
            _b /= 100;
        }

        public Pudelko(double A, UnitOfMeasure type = UnitOfMeasure.centimeter) : this(A, 10, 10, type)
        {
            if (type == UnitOfMeasure.meter)
            {
                _c /= 100;
                _b /= 100;
            }
            else if (type == UnitOfMeasure.milimeter)
            {
                _c *=  10;
                _b *= 10;
            }
        }

        public Pudelko(UnitOfMeasure type = UnitOfMeasure.centimeter) : this(10, 10, 10, type)
        {
            if (type == UnitOfMeasure.meter)
            {
                _c /= 100;
                _b /= 100;
                _a /= 100;
                
            }
            else if (type == UnitOfMeasure.milimeter)
            {
                _c *= 10;
                _b *= 10;
                _a *= 10;
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
            _ = formatProvider ?? CultureInfo.CurrentCulture;

            return format.ToUpperInvariant() switch
            {
                "mm" => string.Format($"{A * 1000:F0} mm × {B * 1000:F0} mm × {C * 1000:F0} mm"),
                "cm" => string.Format($"{A * 100:F1} cm × {B * 100:F1} cm × {C * 100:F1} cm"),
                "m" => string.Format($"{A:F3} m × {B:F3} m × {C:F3} m"),
                _ => ToString(),
            };
        }

        // funkcja pomocnicza zwracajaca metry
        public static double ReturnMeters(double value, UnitOfMeasure m)
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
                }
            }

            // A: a i b, B: a:0 i c:2
            if (Math.Max(a[0], a[1]) >= Math.Max(b[0], b[2]) && Math.Min(a[0], a[1]) >= Math.Min(b[0], b[2]))
            {
                if ((a[0] * a[1] * (a[2] + b[1])) < objetosc)
                {
                    objetosc = a[0] * a[1] * (a[2] + b[1]);
                    x = a[0]; y = a[1]; z = a[2] + b[1];
                }
            }

            // A: a i b, B: b:1 i c:2
            if (Math.Max(a[0], a[1]) >= Math.Max(b[1], b[2]) && Math.Min(a[0], a[1]) >= Math.Min(b[1], b[2]))
            {
                if ((a[0] * a[1] * (a[2] + b[0])) < objetosc)
                {
                    objetosc = a[0] * a[1] * (a[2] + b[0]);
                    x = a[0]; y = a[1]; z = a[2] + b[0];
                }
            }

            // A: b i c, B: a:0 i b:1
            if (Math.Max(a[1], a[2]) >= Math.Max(b[0], b[1]) && Math.Min(a[1], a[2]) >= Math.Min(b[0], b[1]))
            {
                if ((a[1] * a[2] * (a[0] + b[2])) < objetosc)
                {
                    objetosc = a[1] * a[2] * (a[0] + b[2]);
                    x = a[1]; y = a[2]; z = a[0] + b[2];
                }
            }

            // A: b i c, B: a:0 i c:2
            if (Math.Max(a[1], a[2]) >= Math.Max(b[0], b[2]) && Math.Min(a[1], a[2]) >= Math.Min(b[0], b[2]))
            {
                if ((a[1] * a[2] * (a[0] + b[1])) < objetosc)
                {
                    objetosc = a[1] * a[2] * (a[0] + b[1]);
                    x = a[1]; y = a[2]; z = a[0] + b[1];
                }
            }

            // A: b i c, B: b:1 i c:2
            if (Math.Max(a[1], a[2]) >= Math.Max(b[1], b[2]) && Math.Min(a[1], a[2]) >= Math.Min(b[1], b[2]))
            {
                if ((a[1] * a[2] * (a[0] + b[0])) < objetosc)
                {
                    objetosc = a[1] * a[2] * (a[0] + b[0]);
                    x = a[1]; y = a[2]; z = a[0] + b[0];
                }
            }

            // A: a i c, B: a:0 i b:1
            if (Math.Max(a[0], a[2]) >= Math.Max(b[0], b[1]) && Math.Min(a[0], a[2]) >= Math.Min(b[0], b[1]))
            {
                if ((a[0] * a[2] * (a[1] + b[2])) < objetosc)
                {
                    objetosc = a[0] * a[2] * (a[1] + b[2]);
                    x = a[0]; y = a[2]; z = a[1] + b[2];
                }
            }
            // A: a i c, B: a:0 i c:2
            if (Math.Max(a[0], a[2]) >= Math.Max(b[0], b[2]) && Math.Min(a[0], a[2]) >= Math.Min(b[0], b[2]))
            {
                if ((a[0] * a[2] * (a[1] + b[1])) < objetosc)
                {
                    objetosc = a[0] * a[2] * (a[1] + b[1]);
                    x = a[0]; y = a[2]; z = a[1] + b[1];
                }
            }

            // A: a i c, B: b:1 i c:2
            if (Math.Max(a[0], a[2]) >= Math.Max(b[1], b[2]) && Math.Min(a[0], a[2]) >= Math.Min(b[1], b[2]))
            {
                if ((a[0] * a[2] * (a[1] + b[0])) < objetosc)
                {
                    objetosc = a[0] * a[2] * (a[1] + b[0]);
                    x = a[0]; y = a[2]; z = a[1] + b[0];
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
                }
            }

            // B: a i b, A: a:0 i c:2
            if (Math.Max(b[0], b[1]) >= Math.Max(a[0], a[2]) && Math.Min(b[0], b[1]) >= Math.Min(a[0], a[2]))
            {
                if ((b[0] * b[1] * (b[2] + a[1])) < objetosc)
                {
                    objetosc = b[0] * b[1] * (b[2] + a[1]);
                    x = b[0]; y = b[1]; z = b[2] + a[1];
                }
            }

            // B: a i b, A: b:1 i c:2
            if (Math.Max(b[0], b[1]) >= Math.Max(a[1], a[2]) && Math.Min(b[0], b[1]) >= Math.Min(a[1], a[2]))
            {
                if ((b[0] * b[1] * (b[2] + a[0])) < objetosc)
                {
                    objetosc = b[0] * b[1] * (b[2] + a[0]);
                    x = b[0]; y = b[1]; z = b[2] + a[0];
                }
            }
            
            // B: b i c, A: a:0 i b:1
            if (Math.Max(b[1], b[2]) >= Math.Max(a[0], a[1]) && Math.Min(b[1], b[2]) >= Math.Min(a[0], a[1]))
            {
                if ((b[1] * b[2] * (b[0] + a[2])) < objetosc)
                {
                    objetosc = b[1] * b[2] * (b[0] + a[2]);
                    x = b[1]; y = b[2]; z = b[0] + a[2];
                }
            }
            // B: b i c, A: a:0 i c:2
            if (Math.Max(b[1], b[2]) >= Math.Max(a[0], a[2]) && Math.Min(b[1], b[2]) >= Math.Min(a[0], a[2]))
            {
                if ((b[1] * b[2] * (b[0] + a[1])) < objetosc)
                {
                    objetosc = b[1] * b[2] * (b[0] + a[1]);
                    x = b[1]; y = b[2]; z = b[0] + a[1];
                }
            }
            // B: b i c, A: b:1 i c:2
            if (Math.Max(b[1], b[2]) >= Math.Max(a[1], a[2]) && Math.Min(b[1], b[2]) >= Math.Min(a[1], a[2]))
            {
                if ((b[1] * b[2] * (b[0] + a[0])) < objetosc)
                {
                    objetosc = b[1] * b[2] * (b[0] + a[0]);
                    x = b[1]; y = b[2]; z = b[0] + a[0];
                }
            }

            // B: a i c, A: a:0 i b:1
            if (Math.Max(b[0], b[2]) >= Math.Max(a[0], a[1]) && Math.Min(b[0], b[2]) >= Math.Min(a[0], a[1]))
            {
                if ((b[0] * b[2] * (b[1] + a[2])) < objetosc)
                {
                    objetosc = b[0] * b[2] * (b[1] + a[2]);
                    x = b[0]; y = b[2]; z = b[1] + a[2];
                }
            }
            // B: a i c, A: a:0 i c:2
            if (Math.Max(b[0], b[2]) >= Math.Max(a[0], a[2]) && Math.Min(b[0], b[2]) >= Math.Min(a[0], a[2]))
            {
                if ((b[0] * b[2] * (b[1] + a[1])) < objetosc)
                {
                    objetosc = b[0] * b[2] * (b[1] + a[1]);
                    x = b[0]; y = b[2]; z = b[1] + a[1];
                }
            }
            // B: a i c, A: b:1 i c:2
            if (Math.Max(b[0], b[2]) >= Math.Max(a[1], a[2]) && Math.Min(b[0], b[2]) >= Math.Min(a[1], a[2]))
            {
                if ((b[0] * b[2] * (b[1] + a[0])) < objetosc)
                {
                    objetosc = b[0] * b[2] * (b[1] + a[0]);
                    x = b[0]; y = b[2]; z = b[1] + a[0];
                }
            }

            return new Pudelko(x,y,z);
        }

        public static explicit operator double[](Pudelko obj) => new double[] { obj.A, obj.B, obj.C };

        public static implicit operator Pudelko((int x, int y, int z) tuple) => new (tuple.x, tuple.y, tuple.z, UnitOfMeasure.milimeter);

        public static Pudelko Parse(string text)
        {
            var values = text.Split(' ', 'x');
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
            return unit switch
            {
                "m" => 1.0,
                "cm" => 0.01,
                "mm" => 0.001,
                _ => throw new ArgumentException($"Unknown unit: {unit}"),
            };
        }

        public static Pudelko Kompresuj(Pudelko p)
        {         
            double dimension = Math.Pow(p.Objetosc, (double)1/3);
            return new Pudelko(dimension, dimension, dimension);
        }

        public  int CompareBox(Pudelko x, Pudelko y)
        {
            if (x.Objetosc > y.Objetosc)
                return 1;
            else if (x.Objetosc < y.Objetosc)
                return -1;
            else
            {
                if (x.Pole > y.Pole) return 1;
                else if (x.Pole < y.Pole) return -1;
                else
                {
                    if (x.SumaKrawedzi > y.SumaKrawedzi) return 1;
                    else if (x.SumaKrawedzi < y.SumaKrawedzi) return -1;
                    else return 0;
                }
            }
        }

        int IComparer<Pudelko>.Compare(Pudelko? x, Pudelko? y)
        {
            if (x.Objetosc > y.Objetosc)
                return 1;
            else if (x.Objetosc < y.Objetosc)
                return -1;
            else
            {
                if (x.Pole > y.Pole) return 1;
                else if (x.Pole < y.Pole) return -1;
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
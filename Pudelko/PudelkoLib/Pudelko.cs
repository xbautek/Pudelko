﻿using PudelkoNamespace.Enums;
using System.Drawing;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;

namespace PudelkoNamespace.PudelkoLib
{

    public sealed class Pudelko : IFormattable
    {
        private double _a; 
        private double _b; 
        private double _c;

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

        public UnitOfMeasure Measure { get; set; }

        public Pudelko(double a, double b, double c, UnitOfMeasure type)
        {
            if (!Enum.IsDefined(typeof(UnitOfMeasure), type))
            {
                 throw new FormatException("Invalid type for MeasureType enum.");
            }
            Measure = type;

            if (A < 0 || B < 0 || C < 0)
            {
                throw new ArgumentOutOfRangeException("Dimensions of the box must be positive!");
            }

            if (type == UnitOfMeasure.meter && (A > 10 || B > 10 || C > 10)) {
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

        public override string ToString()
        {
            if(Measure == UnitOfMeasure.centimeter)
            {
                return string.Format($"{A * 100:F3} cm × {B * 100:F3} cm × {C * 100:F3} cm");

            }
            else if (Measure == UnitOfMeasure.milimeter)
            {
              return  string.Format($"{A* 1000 : F3} mm × {B* 1000 :F3} mm × {C * 1000:F3} mm");

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
                    throw new FormatException("Bad format, avaible formats: 'mm', 'cm', 'm'.");
            }
        }

        public double ReturnMeters(double value, UnitOfMeasure m)
        {
            if (m == UnitOfMeasure.meter)
            {
                return value;
            }
            else if (m == UnitOfMeasure.milimeter)
            {
                return value / 1000;
            }
            else if (m == UnitOfMeasure.centimeter)
            {
                return value / 100;
            }
            else throw new FormatException("Invalid type for MeasureType enum.");
        }

    }
}
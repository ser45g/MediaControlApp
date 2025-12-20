using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MediaControlApp.Domain.Models.Media.ValueObjects
{
    public class Rating
    {
        public static readonly float LOW_RATING = 0;
        public static readonly float HIGH_RATING = 10;
        public double Value {  get; }
       
        public Rating(double value) {
            if (!IsValid(value)) { 
                throw new ArgumentOutOfRangeException(nameof(Value));
            }
            Value = value;
        }

        public static bool IsValid(double value)
        {
            return !(value < LOW_RATING || value > HIGH_RATING);
        }

        public override bool Equals(object? obj)
        {
            return obj is Rating other &&
                   StringComparer.Ordinal.Equals(Value, other.Value);
        }

        public override int GetHashCode()
        {
            return StringComparer.Ordinal.GetHashCode(Value);
        }
    }
}

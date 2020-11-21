using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoundryCore
{
    public static class Extensions
    {
        public static String Blank(this String me)
        {
            return String.Empty;
        }
        public static T Blank<T>(this T me)
        {
            var tot = typeof(T);
            return tot.IsValueType
              ? default(T)
              : (T)Activator.CreateInstance(tot)
              ;
        }
    }

    class FoBase
    {
        public virtual string AsString() { return "FoBase"; }

        public override string ToString()
        {
            return AsString();
        }
    }
    class FoProperty<T> : FoBase
    {
        public T Value { get; set; }

        public FoProperty(T value = default)
        {
            this.Value = value;
        }

        public override string AsString()
        {
            return Value.ToString();
        }
    }

    class FoCollection<T> : FoProperty<T>
    {
        public new List<T> Value { get; set; }

        public FoCollection(T[] value = default) 
        {
            Value = value == default ? new List<T>() :new List<T>(value);
        }

        public override string AsString()
        {
            return string.Join(",", Value); 
        }
    }
}

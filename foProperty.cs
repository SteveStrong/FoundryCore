using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoundryCore
{
    interface  IFoProperty {
        public string Name { get; set; }
    }
    
    class FoProperty<T> : FoBase, IFoProperty
    {
        public T Value { get; set; }
        public string Name { get; set; }
    
        public FoProperty(T value = default)
        {
            this.Name = "mike";
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
            Value = value == default ? new List<T>() : new List<T>(value);
        }

        public override string AsString()
        {
            return string.Join(",", Value); 
        }
    }
}

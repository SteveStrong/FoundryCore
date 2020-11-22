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
        private object _value { get; set; }
        public T Value { get => (T)Convert.ChangeType(_value, typeof(T)); }
        public string Name { get; set; }
    
        public FoProperty(string name, T value = default)
        {
            Name = name;
            _value = value;
        }

        public override string AsString()
        {
            return Value.ToString();
        }
    }

    class FoCollection<T> : FoProperty<T>
    {
        public new List<T> Value { get; set; }

        public FoCollection(string name, T[] value = default) : base(name)
        {
            Value = value == default ? new List<T>() : new List<T>(value);
        }

        public override string AsString()
        {
            return string.Join(",", Value); 
        }
    }
}

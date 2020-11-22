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
        public string Name { get; set; }
        private object _value { get; set; }
        public T Value
        {
            set {
                _value = value;
            }
            get
            {
                if ( _formula != null ) {
                    _value = _formula();
                }
                return _value != null ? (T)Convert.ChangeType(_value, typeof(T)) : default(T);
            }
        }
        private Func<T> _formula { get; set; }
        public Func<T> Formula
        {
            set {
                _formula = value;
            }
        }
    
        public FoProperty(string name, object value = default)
        {
            Name = name;
            _value = value;
        }

        public FoProperty(string name, Func<T> formula)
        {
            Name = name;
            _formula = formula;
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

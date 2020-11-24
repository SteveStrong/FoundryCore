using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoundryCore
{
    public interface  IFoProperty {
        public string MyName { get; set; }
    }
    
    public class FoProperty<T> : FoBase, IFoProperty
    {
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
            MyName = name;
            _value = value;
        }

        public FoProperty(string name, Func<T> formula)
        {
            MyName = name;
            _formula = formula;
        }

        public override string AsString()
        {
            return Value.ToString();
        }
    }

    
}

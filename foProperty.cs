using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

// https://github.com/ashmind/expressive
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

        public Boolean HasFormula
        {
            get {
                return _formula != null;
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

        public override void WriteAsJsonStart(Utf8JsonWriter writer)
        {
            base.WriteAsJsonStart(writer);
            writer.WriteString("Value",Value.ToString());
            if ( this.HasFormula ) {
                writer.WriteString("Formula",this._formula.ToString());
            }
        }
        public override void WriteAsJsonEnd(Utf8JsonWriter writer)
        {
            base.WriteAsJsonEnd(writer);
        } 
    }

    
}

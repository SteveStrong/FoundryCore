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
    
    public class FoProperty : FoBase
    {
    }

    public class FoProperty<T> : FoProperty, IFoProperty
    {
        private dynamic _value { get; set; }
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

        public Boolean HasValue
        {
            get {
                return _value != null;
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
            var value = Value != null ? Value.ToString() : "";
            return $"{MyName}: {value}";
        }

        public override void WriteAsJsonStart(Utf8JsonWriter writer, WritingDetails spec)
        {
            base.WriteAsJsonStart(writer,spec);
            if ( this.HasValue ) {
                writer.WriteString(nameof(Value),Value.ToString() );
            }

            if (spec == WritingDetails.DETAILS) { 
                if ( this.HasFormula ) {
                    writer.WriteString(nameof(Formula),this._formula.ToString());
                }
            }
        }
        public override void WriteAsJsonEnd(Utf8JsonWriter writer, WritingDetails spec)
        {
            base.WriteAsJsonEnd(writer,spec);
        } 
    }

    
}

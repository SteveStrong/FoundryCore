using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FoundryCore
{

    public class FoBase
    {
        public FoBase Parent { get; set; }
        public string MyName { get; set; }
        public string MyType { 
            get {
                var type = this.GetType();
                var name = type.Name.Replace("\u00601", "");
                if ( type.IsGenericType ) {
                    var arg = type.GetGenericArguments(); //[0];
                    return $"{name}::{arg[0].Name}";
                }
                return name; 
            } 
        }
        public bool HasParent() {
            return Parent != null;
        }
        public bool SetParent(FoBase parent) {
            if ( !HasParent() ){
                Parent = parent;
                return true;
            }
            return false;
        }
        public virtual string AsString() { return "FoBase"; }

        public override string ToString()
        {
            return AsString();
        }
        public virtual void WriteAsJSON(Utf8JsonWriter writer) {
            writer.WriteStartObject();
            writer.WriteString("MyName",this.MyName);
            writer.WriteString("date", DateTimeOffset.UtcNow);
            writer.WriteNumber("temp", 42);
            writer.WriteEndObject();
        }
    }
    
}

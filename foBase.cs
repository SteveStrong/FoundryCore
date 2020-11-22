using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FoundryCore
{

    class FoBase
    {
        public FoBase Parent { get; set; }
        public string MyName { get; set; }
        public string MyType { 
            get {
                var type = this.GetType();
                var arg = type.GetGenericArguments(); //[0];
                if ( arg.Length > 0 ) {
                    return $"{type.Name} :: {arg[0].Name}";
                }
                return type.Name; 
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
    }
    
}

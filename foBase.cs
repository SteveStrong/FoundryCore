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
        public string Name { get; set; }
        public bool HasParent() {
            return Parent != null;
        }
        public virtual string AsString() { return "FoBase"; }

        public override string ToString()
        {
            return AsString();
        }
    }
    
}

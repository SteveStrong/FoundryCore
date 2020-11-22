using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace FoundryCore
{

    class FoComponent : FoBase
    {

        //[JsonInclude]
        public string Name { get; set; }
        public IFoProperty propx { get; set; }

        public object Extra { get => new { Amount = 108, Message = "Hello" };  }

        //https://www.thecodebuzz.com/system-text-json-create-dictionary-converter-json-serialization/
        public Dictionary<string, object> Properties = new Dictionary<string, object>();

  
        public FoComponent Add(string key, IFoProperty obj) {
            Properties.Add(key, obj);
            return this;
        }
    }


}

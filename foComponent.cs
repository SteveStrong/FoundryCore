using System;
using System.Collections.Generic;
using System.Reflection;
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

        public FoComponent(string name)
        {
            this.Name = name;
        }
        public FoComponent Add(string key, object obj) {
            Properties.Add(key, obj);
            return this;
        }

        public FoComponent Add(object obj) {
            Type type = obj.GetType();
            PropertyInfo pinfo = type.GetProperty("Name");
            Properties.Add(pinfo.GetValue(obj).ToString(), obj);
            return this;
        }

        public FoComponent AddList(object[] value)
        {
            foreach(var obj in value) {
                this.Add(obj);
            }
            return this;
        }
    }


}

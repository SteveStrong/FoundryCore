using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace FoundryCore
{

    class FoComponentManager : FoBase
    {
        //https://www.thecodebuzz.com/system-text-json-create-dictionary-converter-json-serialization/
        private Dictionary<string, object> _components = new Dictionary<string, object>();

        public Dictionary<string, object> Manager { 
            get { 
                return this._components;
                }
        }

        public FoComponentManager(FoBase parent)
        {
            Parent = parent;
        }

        public object find(string key) {
            return _components[key];
        }
        public FoComponentManager Add(string key, object obj) {
            _components.Add(key, obj);
            return this;
        }

        public FoComponentManager Add(object obj) {
            Type type = obj.GetType();
            PropertyInfo pinfo = type.GetProperty("Name");
            _components.Add(pinfo.GetValue(obj).ToString(), obj);
            return this;
        }

        public FoComponentManager AddList(object[] value)
        {
            foreach(var obj in value) {
                ((FoBase)obj).SetParent(this.Parent);
                this.Add(obj);
            }
            return this;
        }
    }


}

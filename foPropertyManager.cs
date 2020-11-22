using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace FoundryCore
{

    class FoPropertyManager : FoBase
    {
        //https://www.thecodebuzz.com/system-text-json-create-dictionary-converter-json-serialization/
        private Dictionary<string, object> _properties = new Dictionary<string, object>();

        public Dictionary<string, object> Manager { 
            get { 
                return this._properties;
                }
        }

        public FoPropertyManager(FoBase parent)
        {
            Parent = parent;
        }

        public List<T> AsList<T>(){
            var result = new List<T>();
            _properties.Values.ToList().ForEach( item => {
                result.Add((T)item);
            });
            return result;
        }
        public object find(string key) {
            return _properties[key];
        }
        public FoPropertyManager Add(string key, object obj) {
            ((FoBase)obj).SetParent(this.Parent);
            _properties.Add(key, obj);
            return this;
        }

        public FoPropertyManager Add(object obj) {
            Type type = obj.GetType();
            PropertyInfo pinfo = type.GetProperty("MyName");
            _properties.Add(pinfo.GetValue(obj).ToString(), obj);
            return this;
        }

        public FoPropertyManager AddList(object[] value)
        {
            foreach(var obj in value) {
                this.Add(obj);
            }
            return this;
        }
    }


}

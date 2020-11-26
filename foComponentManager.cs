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
        public override void WriteAsJson(Utf8JsonWriter writer)
        {
            if (this._components?.Count == 0) return;
            base.WriteAsJson(writer);
        }
        public override void WriteAsJsonStart(Utf8JsonWriter writer)
        {
            if ( this._components?.Count == 0 ) return;
            
            writer.WriteStartObject("Subcomponents");
            _components.Values.ToList().ForEach( item => {
                var child = item as FoBase;
                //writer.WriteStartObject(child.MyName);
                //writer.WriteString("MyName",child.MyName);
                //writer.WriteString("Guid", child.UniqueID.ToString());
                //writer.WriteString("MyType", child.MyType.ToString());
                ((FoBase)item).WriteAsJson(writer);
                //writer.WriteEndObject();
            });

        }
        public override void WriteAsJsonEnd(Utf8JsonWriter writer)
        {
            writer.WriteEndObject();
        } 

        public List<T> AsList<T>(){
            var result = new List<T>();
            _components.Values.ToList().ForEach( item => {
                result.Add((T)item);
            });
            return result;
        }
        public object find(string key) {
            return _components[key];
        }
        public FoComponentManager Add(string key, object obj) {
             ((FoBase)obj).SetParent(this.Parent);
            _components.Add(key, obj);
            return this;
        }

        public FoComponentManager Add(object obj) {
            Type type = obj.GetType();
            PropertyInfo pinfo = type.GetProperty("MyName");
            _components.Add(pinfo.GetValue(obj).ToString(), obj);
            return this;
        }

        public FoComponentManager AddList(object[] value)
        {
            foreach(var obj in value) {
               
                this.Add(obj);
            }
            return this;
        }
    }


}

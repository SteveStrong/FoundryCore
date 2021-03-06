﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace FoundryCore
{

    public class FoPropertyManager : FoBase
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

        public override void WriteAsJson(Utf8JsonWriter writer)
        {
            if (this._properties?.Count == 0) return;
            base.WriteAsJson(writer);
        }
        public override void WriteAsJsonStart(Utf8JsonWriter writer)
        {
            if ( this._properties?.Count == 0 ) return;
            
            writer.WriteStartObject("Properties");
            _properties.Values.ToList().ForEach( item => {
                ((FoBase)item).WriteAsJson(writer);
            });

        }
        public override void WriteAsJsonEnd(Utf8JsonWriter writer)
        {
            writer.WriteEndObject();
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

        public FoPropertyManager Add(dynamic obj) {
            Type type = obj.GetType();
            PropertyInfo pinfo = type.GetProperty("MyName");
            _properties.Add(pinfo.GetValue(obj).ToString(), obj);
            return this;
        }

        public FoPropertyManager AddList(dynamic[] value)
        {
            foreach(var obj in value) {
                obj.SetParent(this.Parent);
                this.Add(obj);
            }
            return this;
        }
    }


}

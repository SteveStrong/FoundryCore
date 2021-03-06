﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FoundryCore
{
    public class ApprenticeObject
    {
        public static bool ReportException(Exception eException)
        {
            //ThrowApprenticeAbort(eException);
            //return ReportException(eException, "");
            return true;
        }
    }
    public class FoBase
    { 
		private System.Guid _GUID = System.Guid.Empty;
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
        public virtual System.Guid UniqueID
		{
			get
			{
				if ( _GUID == System.Guid.Empty )
					_GUID = System.Guid.NewGuid();

				return _GUID;
			}
			set
			{
				if ( _GUID == System.Guid.Empty )
					_GUID = value;
			}
		}
 	
        public virtual string AsString() { return "FoBase"; }

        public override string ToString()
        {
            return AsString();
        }
        public virtual void WriteAsJson(Utf8JsonWriter writer)
        {
            WriteAsJsonStart(writer);
            WriteAsJsonEnd(writer);
        }

        public virtual void WriteAsJsonStart(Utf8JsonWriter writer) {
            writer.WriteStartObject(this.MyName);
            writer.WriteString("MyName",this.MyName);
            writer.WriteString("Guid", UniqueID.ToString());
            writer.WriteString("MyType", MyType.ToString());
            if ( this.HasParent()){
                writer.WriteBoolean("HasParent", this.HasParent());
                writer.WriteString("ParentGUID", this.Parent.UniqueID);
            }
        }
        public virtual void WriteAsJsonEnd(Utf8JsonWriter writer) {;
            writer.WriteEndObject();
        }
        public virtual void ReadFromJSON(JsonElement body) {
            var name = body.GetProperty("MyName").ToString();
            var guid = body.GetProperty("Guid").ToString();
            var type = body.GetProperty("MyType").ToString();
        }
    }
    
}

using System;
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
    public enum WritingDetails 
	{
		CLONE,
        SIMPLE,
        DETAILS,
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
        public virtual void WriteAsJson(Utf8JsonWriter writer, WritingDetails spec)
        {
            WriteAsJsonStart(writer,spec);
            WriteAsJsonEnd(writer,spec);
        }

        public virtual void WriteAsJsonStart(Utf8JsonWriter writer, WritingDetails spec) {
            writer.WriteStartObject(this.MyName);
            writer.WriteString(nameof(MyName),this.MyName);
            if ( spec == WritingDetails.DETAILS ) {
                writer.WriteString(nameof(Guid), UniqueID.ToString());
                writer.WriteString(nameof(MyType), MyType.ToString());
            }
            // if ( this.HasParent()){
            //     writer.WriteBoolean("HasParent", this.HasParent());
            //     writer.WriteString("ParentGUID", this.Parent.UniqueID);
            // }
        }
        public virtual void WriteAsJsonEnd(Utf8JsonWriter writer, WritingDetails spec) {
            writer.WriteEndObject();
        }
        public virtual void ReadFromJSON(JsonElement body) {
            var name = body.GetProperty(nameof(MyName)).ToString();
            var guid = body.GetProperty(nameof(Guid)).ToString();
            var type = body.GetProperty(nameof(MyType)).ToString();
        }
    }
    
}

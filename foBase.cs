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

//https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-how-to?pivots=dotnet-5-0
        // static JsonSerializerOptions options = new()
        // {
        //     //ReferenceHandler = ReferenceHandler.Preserve,
        //     IncludeFields = true,
        //     WriteIndented = true
        // };
        public DateTime CreatedDate = new DateTime(2013, 1, 20, 0, 0, 0, DateTimeKind.Utc);
        // public virtual void asJson(object source) {
        //     var json = JsonSerializer.Serialize(source, options);
        //     Console.WriteLine($" json:- {json}");
        // }
        public virtual string AsString() { return "FoBase"; }

        public override string ToString()
        {
            return AsString();
        }
    }
    
}

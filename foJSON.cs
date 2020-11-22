using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FoundryCore
{

    public static class JSONExtensions
    {

//https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-how-to?pivots=dotnet-5-0
        static JsonSerializerOptions options = new()
        {
            //ReferenceHandler = ReferenceHandler.Preserve,
            IgnoreNullValues = true,
            IgnoreReadOnlyProperties = true,
            IncludeFields = true,
            WriteIndented = true
        };
        public static string toJson(this object source) {
            var json = JsonSerializer.Serialize(source, options);
            // Console.WriteLine($"{json}");
            return json;
        }
    }
    
}

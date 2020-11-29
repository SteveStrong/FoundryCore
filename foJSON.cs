using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;

namespace FoundryCore
{
    
    public static class JSONExtensions
    {

//https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-how-to?pivots=dotnet-5-0

        public static string toJson(this object source) {
            JsonSerializerOptions options = new()
            {
                //ReferenceHandler = ReferenceHandler.Preserve,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                IgnoreNullValues = true,
                IgnoreReadOnlyProperties = false,
                IncludeFields = true,
                WriteIndented = true
            };
            //options.Converters.Add(new FoPropertyManagerConverter());
            var json = JsonSerializer.Serialize(source, options);
            return json;
        }


        public static string AsJson(this FoBase source, string name = "") {
            var options = new JsonWriterOptions {
                Indented = true
            };

            using var stream = new MemoryStream();
            using (var writer = new Utf8JsonWriter(stream,options))
            {
                writer.WriteStartObject();
                writer.WriteStartObject(name == String.Empty ? "Model": name);
                source.WriteAsJson(writer);
                writer.WriteEndObject();
                writer.WriteEndObject();
            }

            string json = Encoding.ASCII.GetString(stream.ToArray());
            return json;
        }

        public static void saveToFile(this FoBase source, string path) {
           System.IO.File.WriteAllText(path, source.AsJson());
        }

        public static JsonElement readFromFile(string path) {
            var jsonString = System.IO.File.ReadAllText(path);
            using(var doc = JsonDocument.Parse(jsonString)) {
                return doc.RootElement.Clone();
            }
        }

        
    }
    
}

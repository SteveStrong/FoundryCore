using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FoundryCore
{

using System;
using System.Text.Json;
using System.Text.Json.Serialization;


    //public class FoPropertyManagerConverter: JsonConverter<object>
    //{
    //    public override object Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    //    {
    //        if (reader.TokenType == JsonTokenType.True)
    //        {
    //            return true;
    //        }

    //        if (reader.TokenType == JsonTokenType.False)
    //        {
    //            return false;
    //        }

    //        if (reader.TokenType == JsonTokenType.Number)
    //        {
    //            if (reader.TryGetInt64(out long l))
    //            {
    //                return l;
    //            }

    //            return reader.GetDouble();
    //        }

    //        if (reader.TokenType == JsonTokenType.String)
    //        {
    //            if (reader.TryGetDateTime(out DateTime datetime))
    //            {
    //                return datetime;
    //            }

    //            return reader.GetString();
    //        }

    //        // Use JsonElement as fallback.
    //        // Newtonsoft uses JArray or JObject.
    //        using JsonDocument document = JsonDocument.ParseValue(ref reader);
    //        return document.RootElement.Clone();
    //    }

    //    public override void Write(Utf8JsonWriter writer, object manager, JsonSerializerOptions options)
    //    {
    //        writer.WriteStartObject();
    //        writer.WriteString("helloooooo", "steveie");
    //        writer.WriteString("typexxx", manager.GetType().Name);

    //        if ( manager is FoPropertyManager) {

    //            writer.WriteString("vvvvv", ((FoPropertyManager)manager).MyName);
    //        }

    //        writer.WriteEndObject();
    //    }
    //}


    //public class FoPropertyManagerConverter : JsonConverter<FoPropertyManager>
    //{
 

    //    public override bool CanConvert(Type typeToConvert) =>
    //        typeof(FoPropertyManager).IsAssignableFrom(typeToConvert);

    //    public override FoPropertyManager Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    //    {
    //        if (reader.TokenType != JsonTokenType.StartObject)
    //        {
    //            throw new JsonException();
    //        }

    //        reader.Read();
    //        if (reader.TokenType != JsonTokenType.PropertyName)
    //        {
    //            throw new JsonException();
    //        }

    //        //string propertyName = reader.GetString();
    //        //if (propertyName != "TypeDiscriminator")
    //        //{
    //        //    throw new JsonException();
    //        //}

    //        reader.Read();
    //        if (reader.TokenType != JsonTokenType.Number)
    //        {
    //            throw new JsonException();
    //        }

    //        //TypeDiscriminator typeDiscriminator = (TypeDiscriminator)reader.GetInt32();
    //        //Person person = typeDiscriminator switch
    //        //{
    //        //    TypeDiscriminator.Customer => new Customer(),
    //        //    TypeDiscriminator.Employee => new Employee(),
    //        //    _ => throw new JsonException()
    //        //};

    //        var person = new FoPropertyManager(null);
    //        while (reader.Read())
    //        {
    //            if (reader.TokenType == JsonTokenType.EndObject)
    //            {
    //                return person;
    //            }

    //            //if (reader.TokenType == JsonTokenType.PropertyName)
    //            //{
    //            //    propertyName = reader.GetString();
    //            //    reader.Read();
    //            //    switch (propertyName)
    //            //    {
    //            //        case "CreditLimit":
    //            //            decimal creditLimit = reader.GetDecimal();
    //            //            ((Customer)person).CreditLimit = creditLimit;
    //            //            break;
    //            //        case "OfficeNumber":
    //            //            string officeNumber = reader.GetString();
    //            //            ((Employee)person).OfficeNumber = officeNumber;
    //            //            break;
    //            //        case "Name":
    //            //            string name = reader.GetString();
    //            //            person.Name = name;
    //            //            break;
    //            //    }
    //            //}
    //        }

    //        throw new JsonException();
    //    }

    //    public override void Write(Utf8JsonWriter writer, FoPropertyManager manager, JsonSerializerOptions options)
    //    {
    //        writer.WriteStartObject();

    //        //if (person is Customer customer)
    //        //{
    //        //    writer.WriteNumber("TypeDiscriminator", (int)TypeDiscriminator.Customer);
    //        //    writer.WriteNumber("CreditLimit", customer.CreditLimit);
    //        //}
    //        //else if (person is Employee employee)
    //        //{
    //        //    writer.WriteNumber("TypeDiscriminator", (int)TypeDiscriminator.Employee);
    //        //    writer.WriteString("OfficeNumber", employee.OfficeNumber);
    //        //}

    //        writer.WriteString("MytttttttName", manager.MyName);
    //        //writer.WriteStartArray(manager.Manager);
    //        //writer.WriteEndArray(manager.Manager);
    //        writer.WriteEndObject();
    //    }
    //}
   
    
    public static class JSONExtensions
    {

//https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-how-to?pivots=dotnet-5-0

        public static string toJson(this object source) {
            JsonSerializerOptions options = new()
            {
                //ReferenceHandler = ReferenceHandler.Preserve,
                IgnoreNullValues = true,
                IgnoreReadOnlyProperties = false,
                IncludeFields = true,
                WriteIndented = true
            };
            //options.Converters.Add(new FoPropertyManagerConverter());
            var json = JsonSerializer.Serialize(source, options);
            return json;
        }

        public static void exportAsJson(this FoBase source, string path) {
            var options = new JsonWriterOptions {
                Indented = true
            };

            using (var stream = new System.IO.MemoryStream())
            {
                using (var writer = new Utf8JsonWriter(stream, options))
                {
                    source.WriteAsJSON(writer);
                }

                string json = Encoding.UTF8.GetString(stream.ToArray());
                Console.WriteLine(json);
                System.IO.File.WriteAllText(path, json);
            }
        }

        public static void saveToFile(this object source, string path) {
           System.IO.File.WriteAllText(path, source.toJson());
        }

        public static JsonElement readFromFile(string path) {
            var jsonString = System.IO.File.ReadAllText(path);
            using(var doc = JsonDocument.Parse(jsonString)) {
                return doc.RootElement.Clone();
            }
        }

        
    }
    
}

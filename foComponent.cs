using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace FoundryCore
{
    public interface  IFoComponent {
        public string MyName { get; set; }
        public T Reference<T>(string name) where T : FoProperty;
    }
    public class FoComponent : FoBase, IFoComponent
    {
        public FoPropertyManager Properties { get; set; }
        public FoComponentManager Subcomponents { get; set; }

        //public object Extra { get => new { Amount = 108, Message = "Hello" };  }

        public FoComponent()
        {
            Properties = new FoPropertyManager(this);
            Subcomponents = new FoComponentManager(this);
        }

        public FoComponent(string name): this()
        {
            this.MyName = name;
        }
        public FoComponent(string name, IFoProperty[] props): this(name)
        {
            Properties.AddArray(props);
        }

        public FoComponent(string name, IFoComponent[] comps): this(name)
        {
            Subcomponents.AddArray(comps);
        }

        public FoComponent Self { get { return this; } }

        public double SumOver(string name){
            double result = 0;
            var list = Subcomponents.AsList<FoComponent>().Select(x => x.Reference<FoProperty<double>>(name).Value );
            result = list.Sum();
            return result;
        }

        public object Reference(string name){
            return Properties.find(name);
        }

        public T Reference<T>(string name) where T: FoProperty{
            return (T)Properties.find(name);
        }

        public override void WriteAsJsonStart(Utf8JsonWriter writer, WritingDetails spec) {
            base.WriteAsJsonStart(writer,spec);

            //properties first
            Properties?.WriteAsJson(writer,spec);
            //components first
            Subcomponents?.WriteAsJson(writer,spec);
         }
        public override void WriteAsJsonEnd(Utf8JsonWriter writer, WritingDetails spec) {
            base.WriteAsJsonEnd(writer,spec);
        }
        public  void WnnnriteAsJSON(Utf8JsonWriter writer) {
            //base.WriteAsJSON(writer,spec);
            writer.WriteStartObject("ccc");

                writer.WriteNumber("age", 15);
                writer.WriteString("date", DateTime.Now);
                writer.WriteString("first", "John");
                writer.WriteString("last", "Smith");

                writer.WriteStartArray("phoneNumbers");
                writer.WriteStringValue("425-000-1212");
                writer.WriteStringValue("425-000-1213");
                writer.WriteEndArray();

            writer.WriteEndObject();
            // writer.WriteStartArray("Properties");
            // writer.WriteEndArray();
            // writer.WriteEndObject();

            // writer.WriteStartObject();


            writer.WriteStartObject("address");
            writer.WriteString("street", "1 Microsoft Way");
            writer.WriteString("city", "Redmond");
            writer.WriteNumber("zip", 98052);
            writer.WriteEndObject();

            writer.WriteStartArray("ExtraArray");
            var extraData = new[] { 1, 2, 3, 4, 5, 6, 7 };
            for (var i = 0; i < extraData.Length; i++)
            {
                writer.WriteNumberValue(extraData[i]);
            }
            writer.WriteEndArray();

            //writer.WriteEndObject();

        }
    }


}

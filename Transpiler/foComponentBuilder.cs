using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;


namespace FoundryCore
{
    public interface  IFoComponentBuilder {
        public IFoComponentBuilder SetName(string name);
        public IFoComponentBuilder AddProperty<T>(string name);
        public FoComponent Build();
     }
    public class FoComponentBuilder : IFoComponentBuilder
    {
        private string _name;
        private List<FoProperty> _props = new List<FoProperty>();
        public static IFoComponentBuilder Start() {
            var builder = new FoComponentBuilder();
            return (IFoComponentBuilder)builder;
        }
        public IFoComponentBuilder SetName(string name){
            this._name = name;
            return (IFoComponentBuilder)this;
        }

        public IFoComponentBuilder AddProperty<T>(string name){
            _props.Add(new FoProperty<T>(name));
            return (IFoComponentBuilder)this;
        }

        public FoComponent Build(){
            var result = new FoComponent(this._name);
            result.Properties.AddArray(this._props.ToArray());
            return result;
        }
        
    }
}
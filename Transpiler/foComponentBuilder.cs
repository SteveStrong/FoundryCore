using System.Collections.Generic;
using System.Diagnostics;

namespace FoundryCore
{
    public interface  IFoComponentBuilder {
        public IFoComponentBuilder SetName(string name);
        public IFoComponentBuilder AddProperty<T>(string name, object value=default);
        public IFoComponentBuilder AddProperty(FoProperty property);
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

        public IFoComponentBuilder AddProperty<T>(string name, object value = default) {
            _props.Add(new FoProperty<T>(name, value));
            return (IFoComponentBuilder)this;
        }

        public IFoComponentBuilder AddProperty(FoProperty property) {
            _props.Add(property);
            return (IFoComponentBuilder)this;
        }
        public FoComponent Build(){
            var result = new FoComponent(this._name);
            result.Properties.AddArray(this._props.ToArray());
            return result;
        }

        private string GetDebuggerDisplay()
        {
            return ToString();
        }
    }
}
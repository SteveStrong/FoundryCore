using System.Collections.Generic;


namespace FoundryCore
{
    public interface  IFoTemplateBuilder {
        public IFoTemplateBuilder SetName(string name);
        public IFoTemplateBuilder AddProperty(FoProperty property);
        public T Apply<T>(T target) where T: FoComponent;
    }
    public class FoTemplateBuilder : IFoTemplateBuilder
    {
        private string _name;
        private List<FoProperty> _props = new List<FoProperty>();

        public static IFoTemplateBuilder Start() {
            var builder = new FoTemplateBuilder();
            return (IFoTemplateBuilder)builder;
        }
        public IFoTemplateBuilder SetName(string name){
            this._name = name;
            return (IFoTemplateBuilder)this;
        }

        public IFoTemplateBuilder AddProperty(FoProperty property) {
            _props.Add(property);
            return (IFoTemplateBuilder)this;
        }
        public T Apply<T>(T target) where T: FoComponent {
            target.MyName = _name;
            target.Properties.AddArray(this._props.ToArray());
            return target;
        }
        
    }
}
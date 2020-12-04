using System.Collections.Generic;


namespace FoundryCore
{
    public interface  IFoTemplateBuilder {
        public IFoTemplateBuilder SetName(string name);
        public FoBase Apply(FoBase target);
     }
    public class FoTemplateBuilder : IFoTemplateBuilder
    {
        private string _name;

        public static IFoTemplateBuilder Start() {
            var builder = new FoTemplateBuilder();
            return (IFoTemplateBuilder)builder;
        }
        public IFoTemplateBuilder SetName(string name){
            this._name = name;
            return (IFoTemplateBuilder)this;
        }

        public FoBase Apply(FoBase target) {
            return target;
        }
        
    }
}
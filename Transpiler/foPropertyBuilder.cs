using System.Collections.Generic;


namespace FoundryCore
{
    public interface  IFoPropertyBuilder {
        public IFoPropertyBuilder SetName(string name);
        public IFoPropertyBuilder SetValue(object value);
        public FoProperty<T> Build<T>();
     }
    public class FoPropertyBuilder : IFoPropertyBuilder
    {
        private string _name;
        private object _value;

        public static IFoPropertyBuilder Start() {
            var builder = new FoPropertyBuilder();
            return (IFoPropertyBuilder)builder;
        }
        public IFoPropertyBuilder SetName(string name){
            this._name = name;
            return (IFoPropertyBuilder)this;
        }

        public IFoPropertyBuilder SetValue(object value) {
            this._value = value;
            return (IFoPropertyBuilder)this;
        }
        public FoProperty<T> Build<T>(){
            var result = new FoProperty<T>(this._name, this._value);
            return result;
        }
        
    }
}
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace FoundryCore
{
    interface  IFoComponent {
        public string MyName { get; set; }
        public T Reference<T>(string name);
    }
    class FoComponent : FoBase, IFoComponent
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
            Properties.AddList(props);
        }

        public FoComponent(string name, IFoComponent[] comps): this(name)
        {
            Subcomponents.AddList(comps);
        }
        
        public double SumOver(string name){
            double result = 0;
            var list = Subcomponents.AsList<FoComponent>().Select(x => x.Reference<FoProperty<double>>(name).Value );
            result = list.Sum();
            return result;
        }

        public object Reference(string name){
            return Properties.find(name);
        }

        public T Reference<T>(string name){
            return (T)Properties.find(name);
        }
       
    }


}

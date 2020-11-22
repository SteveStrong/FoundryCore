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
        public string Name { get; set; }
    }
    class FoComponent : FoBase, IFoComponent
    {
        public FoPropertyManager Properties { get; set; }

        public object Extra { get => new { Amount = 108, Message = "Hello" };  }

        public FoComponent(string name)
        {
            Properties = new FoPropertyManager(this);
            this.Name = name;
        }
        public FoComponent(string name, IFoProperty[] props): this(name)
        {
            Properties.AddList(props);
        }
        

        public object Reference(string name){
            return Properties.find(name);
        }

        public T Reference<T>(string name){
            return (T)Properties.find(name);
        }
       
    }


}

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace FoundryCore
{
    public interface  IFoComponentBuilder {
        public IFoComponentBuilder SetName(string name);
        public FoComponent Build();
     }
    public class FoComponentBuilder : IFoComponentBuilder
    {
        private string name;
        public static IFoComponentBuilder Start() {
            var builder = new FoComponentBuilder();
            return (IFoComponentBuilder)builder;
        }
        public IFoComponentBuilder SetName(string name){
            this.name = name;
            return (IFoComponentBuilder)this;
        }

        public FoComponent Build(){
            var result = new FoComponent(this.name);
            return result;
        }
        
    }
}
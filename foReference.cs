using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

// https://github.com/ashmind/expressive
namespace FoundryCore
{
    public class FoReference : FoBase
    {
        public FoReference(string name)
        {
            MyName = name;
        }

        public T GetValue<T>(FoComponent context){
            var found = context.Reference<FoProperty<T>>(MyName);
            return found != null ? found.Value : default(T);
        }
    }
}
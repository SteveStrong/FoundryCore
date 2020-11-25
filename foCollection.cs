
    
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoundryCore
{
    public class FoCollection<T> : FoProperty<T>
    {
        public new List<T> Value { get; set; }

        public int Count
        {
            get { return Value.Count; }
        }
        public T this[int index]
        {
            get { return Value?.Count > 0 ? Value[index] : default(T); }
            set { Value[index] = value; }
        }

        public FoCollection(string name, T[] value = default) : base(name)
        {
            Value = value == default ? new List<T>() : new List<T>(value);
        }

        public override string AsString()
        {
            return string.Join(",", Value);
        }

        public T AddNoDuplicate(T item) {
            var found = Value.Find(x => x.Equals(item));
            if ( found == null ) {
                Value.Add(item);
            }
            return item;
        }
    }
}
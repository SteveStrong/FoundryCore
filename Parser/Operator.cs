using System;
using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace FoundryCore
{
    public class Operator
    {
        public string Name { get; set; }
        public List<Operator> Children = new List<Operator>();
        public Operator(string name)
        {
            Name = name;
        }       
        public void AddChildObject(Operator child) 
        {
            Children.Add(child);
        }
        public virtual string Decompile() {
            return Name;
        }

        public virtual string AsCSharp() {
            return Name;
        }
    }
    public class Constant : Operator
    {
        public Constant(string name) : base(name)
        {
        }
        public Constant(string name, object value) : base(name)
        {
        }
    }

    public class Literal : Constant
    {
        public Literal(string name, object value) : base(name,value)
        {
        }
    }

    public class Set : Operator
    {
        public Set(string name) : base(name)
        {
        }
    }

    public class ValidValues : Operator
    {
        public ValidValues(string name) : base(name)
        {
        }
    }

    public class FunctionOperator : Operator
    {
        public FunctionOperator(string name) : base(name)
        {
        }
    }
    public class BinaryOperator : Operator
    {
        public BinaryOperator(string name) : base(name)
        {
        }

        public override string Decompile() {
            var LHS = this.Children[0];
            var RHS = this.Children[1];
            var result = $"{LHS.Decompile()} {Name} {RHS.Decompile()}";
            return result;
        }

        public override string AsCSharp() {
            var LHS = this.Children[0];
            var RHS = this.Children[1];
            var result = $"{LHS.AsCSharp()} {Name} {RHS.AsCSharp()}";
            return result;
        }
    }

    public class GroupOperator : Operator
    {
        public GroupOperator(string name) : base(name)
        {
        }
        public override string Decompile() {
            var LHS = this.Children[0];
            var result = $"( {LHS.Decompile()} )";
            return result;
        }

        
        public override string AsCSharp() {
            var LHS = this.Children[0];
            var result = $"( {LHS.AsCSharp()} )";
            return result;
        }
    }

    public class LogicalBinaryOperator : Operator
    {
        public LogicalBinaryOperator(string name) : base(name)
        {
        }
    }

    public class BranchOperator : Operator
    {
        public BranchOperator(string name) : base(name)
        {
        }
    }

    public class SwitchOperator : Operator
    {
        public SwitchOperator(string name) : base(name)
        {
        }
    }

     public class ReferenceOperator : Operator
    {
        public ReferenceOperator(string name) : base(name)
        {
        }
    }   
    public class FieldReference : ReferenceOperator
    {
        public FieldReference(string name) : base(name)
        {
        }
    }

    public class IndexReference : ReferenceOperator
    {
        public IndexReference(string name) : base(name)
        {
        }
    }
    public class ComponentReference : ReferenceOperator
    {
        public ComponentReference(string name) : base(name)
        {
        }
    } 
    public class ValueReference : ReferenceOperator
    {
        public ValueReference(string name) : base(name)
        {
        }
    } 
    
    public class ValueAtReference : ValueReference
    {
        public ValueAtReference(string name) : base(name)
        {
        }
        public ValueAtReference(string name, char cControl) : base(name)
        {
        }
    }  

    public class ComponentAtReference : ComponentReference
    {
        public ComponentAtReference(string name) : base(name)
        {
        }
        public ComponentAtReference(string name, char cControl) : base(name)
        {
        }
    }
}
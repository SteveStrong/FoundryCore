namespace FoundryCore
{
    public class Operator
    {
        public Operator(string name)
        {
        }       
        public void AddChildObject(Operator child) 
        {
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
    }

    public class GroupOperator : Operator
    {
        public GroupOperator(string name) : base(name)
        {
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
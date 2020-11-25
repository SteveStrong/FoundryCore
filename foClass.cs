using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FoundryCore
{

    public class FoClass : FoBase
    {
        private Type _ConstructionType = null;
        private long _InstanceCount = 0;


		private FoCollection<FoClass> _SuperClass { get; set; }
        private FoCollection<FoClass> _SubClass { get; set; }

		public FoClass() :base()
		{
			_SuperClass = new FoCollection<FoClass>("SuperClass");
			_SubClass = new FoCollection<FoClass>("SubClass");
		}
		public FoClass(string name) :this()
		{
            this.MyName = name;
        }
        public FoCollection<FoClass> Supers { get { return _SuperClass; }}
        public FoCollection<FoClass> Subs { get { return _SubClass; }}

        public Type ApprenticeType(object oObject)
		{
			if ( oObject.GetType().IsSubclassOf(typeof(Type)) )
				return ApprenticeType(oObject as Type);
			else
				return ApprenticeType(oObject.ToString(),GetType());
		}

		public Type ApprenticeType(Assembly oAssembly, string sName)
		{
			Type[] oTypes = oAssembly.GetExportedTypes();

			foreach (Type oType in oTypes)
			{
				if (oType.Name.Equals(sName))
					return oType;
				if (oType.FullName.Equals(sName))
					return oType;
			}
			return null;
		}
        public Type ApprenticeType(string sName, Type oBaseType)
		{
			if ( sName == null || sName.Length == 0 )
				return null;

			Type oaType = Type.GetType(sName);
			if ( oaType != null )
				return oaType;

			Assembly oAssembly = System.Reflection.Assembly.GetAssembly(oBaseType);
			oaType = ApprenticeType(oAssembly,sName);
			if ( oaType == null )
			{
				oAssembly = System.Reflection.Assembly.GetExecutingAssembly();
				oaType = ApprenticeType(oAssembly,sName);
			}
			return oaType;
		}

        public bool HasConstructionType(Type oType)
		{
			Type oMyType = ConstructionType;
			return (oMyType == oType || oMyType.IsSubclassOf(oType));
		}
		private Type DetermineConstruction(Type DefaultType)
		{			
			if (_ConstructionType != null)
				return _ConstructionType;

			Type oType = ApprenticeType(MyName);
			if (oType != null && oType.IsClass && oType.IsSubclassOf(DefaultType) )
				return oType;

			FoClass oClass = _SuperClass[0] as FoClass; 

			if ( oClass == null ) 
				return DefaultType;
			else 
				oType = oClass.DetermineConstruction(DefaultType);

			if ( _SuperClass.Count > 1 )
				foreach(FoClass oSuper in _SuperClass.Value)
				{
					if ( oSuper == oClass )
						continue;

					Type oNewType = oSuper.DetermineConstruction(DefaultType);
					oType = oNewType.IsSubclassOf(oType) ? oNewType : oType;
				}

			return oType;

		}

        public virtual bool AddInheritFrom(FoClass superClass)
		{
			if ( superClass == null || superClass == this )
				return false;

			_SuperClass.AddNoDuplicate(superClass);
			superClass.Subs.AddNoDuplicate(this);
			return true;
		}

        public virtual Type DefaultConstruction
		{
			get 
			{ 
				return typeof(FoComponent);
			}
		}
        public virtual Type ConstructionType
		{
			get
			{
				if ( _ConstructionType == null ) 
					_ConstructionType = DetermineConstruction(DefaultConstruction);

				return _ConstructionType;
			}
			set
			{
				_ConstructionType = value;
			}
		}

        public object Create(string sType)
		{
			Type oType = ApprenticeType(sType);
			if ( oType != null )
				return Create(oType);
			else
				return null;
		}
        public object Create(string sAssembly, string sType)
		{
			object[] oParams = {};
			return Activator.CreateInstance(sAssembly,sType,oParams);
		}
        public object Create(string sName, Type oType)
		{
			object[] oParams = { sName };
			return Activator.CreateInstance(oType,oParams);
		}
		public object Create(Type oType)
		{
			object[] oParams = {};
			return Activator.CreateInstance(oType,oParams);
		}


		public string InstanceName(string sName = default)
		{
			_InstanceCount++;
			if ( sName.Length > 0 )
				return sName.CreateInternalName();

            var count4 = _InstanceCount.ToString().PadLeft(4, '0');
            return $"{GetType().Name} {count4}".CreateInternalName();
		}
        public FoBase ConstructInstanceCore(string sName)
		{
			string sInstanceName = InstanceName(sName);

            object[] oParams = { sInstanceName };
            FoBase oObject = Activator.CreateInstance(ConstructionType,oParams) as FoBase;

			return oObject;
		}

    }
}
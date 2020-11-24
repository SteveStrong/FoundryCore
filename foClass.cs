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


		private FoCollection<FoClass> _oSuperClasses { get; set; }
        private FoCollection<FoClass> _oSubClasses { get; set; }

		public FoClass() :base()
		{
			_oSuperClasses = new FoCollection<FoClass>("SuperClass");
			_oSubClasses = new FoCollection<FoClass>("SubClass");
		}

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
		private Type DetermineConstruction(Type oDefaultType)
		{			
			if (_ConstructionType != null)
				return _ConstructionType;

			Type oType = ApprenticeType(MyName);
			if (oType != null && oType.IsClass && oType.IsSubclassOf(oDefaultType) )
				return oType;

			ClassObject oClass = m_oSuperClasses.First as ClassObject; 

			if ( oClass == null ) 
				return oDefaultType;
			else 
				oType = oClass.DetermineConstruction(oDefaultType);

			if ( m_oSuperClasses.Count > 1 )
				foreach(ClassObject oSuper in m_oSuperClasses)
				{
					if ( oSuper == oClass )
						continue;

					Type oNewType = oSuper.DetermineConstruction(oDefaultType);
					oType = oNewType.IsSubclassOf(oType) ? oNewType : oType;
				}

			return oType;

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
        public FoBase ConstructInstanceCore(string sName, FoBase oTarget, int iIndex)
		{
			string sInstanceName = InstanceName(sName);
			InstanceObject oObject = null;
			try 
			{
				object[] oParams = { sInstanceName };
				oObject = Activator.CreateInstance(ConstructionType,oParams) as FoBase;
				oParams = null;

				oObject.MyIndex = iIndex;
				oObject.InstanceID = _InstanceCount;
				if ( sName.Length > 0 )  //modified in 6.3x to keep shape names real
					oObject.CustomName = sName;
			}
			catch(Exception e)
			{
				//ApprenticeObject.ReportException(e);
				return null;
			}


			if (oTarget != null) 
			{
				//oObject.Status.IsBuilding = true;
				oObject.Status.UserSpecified = Status.UserSpecified ? true : (sName.Length == 0);
				oTarget.AddChildObject(oObject);
				oObject.AfterInstanceParented();
			}

			return oObject;
		}

    }
}
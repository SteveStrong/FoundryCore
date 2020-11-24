
using System.Collections;

namespace FoundryCore
{

	public class StatusBitArray
	{

		private BitArray _Status;

		private enum StatusBit 
		{
			Invisible,
			Enabled,
			LocalMembersOnly,
			Dynamic,
			BadReference,
			MetaKnowledge,
			QueueTrigger,
			Temporary,
			Purging,
			ValueOverTyped,
			ExistenceSuspect,
			ValueSuspect,
			UserSpecified,
			Private,
			Internal,
			Expanded,
			Calculating,
			Calculated,
			ProtectFormula,
			ProtectValue,
			ForceEvaluation,
			ValueIncorrect,
			IsReadOnly,
			TrackChanges
		}

		public StatusBitArray()
		{
			_Status = new BitArray( 24 );
			_Status.SetAll(false);
			Enabled = true;
		}
		public void Purge()
		{
			_Status = null;
		}
		public bool Visible
		{
			get
			{
				return !_Status[(int)StatusBit.Invisible];
			}
			set
			{
				_Status[(int)StatusBit.Invisible] = !value;
			}
		}

		public bool Enabled
		{
			get
			{
				return _Status[(int)StatusBit.Enabled];
			}
			set
			{
				_Status[(int)StatusBit.Enabled] = value;
			}
		}
		public bool IsPurging
		{
			get
			{
				return _Status[(int)StatusBit.Purging];
			}
			set
			{
				_Status[(int)StatusBit.Purging] = value;
			}
		}
		public bool HasQueueTrigger
		{
			get
			{
				return _Status[(int)StatusBit.QueueTrigger];
			}
			set
			{
				_Status[(int)StatusBit.QueueTrigger] = value;
			}
		}
		public bool IsExpanded
		{
			get
			{
				return _Status[(int)StatusBit.Expanded];
			}
			set
			{
				_Status[(int)StatusBit.Expanded] = value;
			}
		}
		public bool IsMetaKnowledge
		{
			get
			{
				return _Status[(int)StatusBit.MetaKnowledge];
			}
			set
			{
				_Status[(int)StatusBit.MetaKnowledge] = value;
			}
		}
		public bool IsFormulaProtected
		{
			get
			{
				return _Status[(int)StatusBit.ProtectFormula];
			}
			set
			{
				_Status[(int)StatusBit.ProtectFormula] = value;
			}
		}
		public bool IsWhileFormula
		{
			get
			{
				return _Status[(int)StatusBit.Temporary];
			}
			set
			{
				_Status[(int)StatusBit.Temporary] = value;
			}
		}
		public bool UserSpecified
		{
			get
			{
				return _Status[(int)StatusBit.UserSpecified];
			}
			set
			{
				_Status[(int)StatusBit.UserSpecified] = value;
			}
		}
		public bool IsReadOnly
		{
			get
			{
				return _Status[(int)StatusBit.IsReadOnly];
			}
			set
			{
				_Status[(int)StatusBit.IsReadOnly] = value;
			}
		}
		public bool IsValueOverTyped
		{
			get
			{
				return _Status[(int)StatusBit.ValueOverTyped];
			}
			set
			{
				_Status[(int)StatusBit.ValueOverTyped] = value;
			}
		}
		public bool TrackChanges
		{
			get
			{
				return _Status[(int)StatusBit.TrackChanges];
			}
			set
			{
				_Status[(int)StatusBit.TrackChanges] = value;
			}
		}
		public bool IsValueProtected
		{
			get
			{
				return _Status[(int)StatusBit.ProtectValue];
			}
			set
			{
				_Status[(int)StatusBit.ProtectValue] = value;
			}
		}
		public bool IsValueDetermined
		{
			get
			{
				return !_Status[(int)StatusBit.ValueSuspect];
			}
			set
			{
				_Status[(int)StatusBit.ValueSuspect] = !value;
				_Status[(int)StatusBit.Calculating]= false;
			}
		}
		public bool Recalculate
		{
			get
			{
				return IsValueSuspect || IsReferenceBad; // || IsRecalculatedAlways;
			}
		}
		public bool AllowRecalculation
		{
			get
			{
				return !IsFormulaProtected && (IsCalculated || IsReferenceBad);
			}
		}
		public bool IsValueSuspect
		{
			get
			{
				return _Status[(int)StatusBit.ValueSuspect];
			}
			set
			{
				_Status[(int)StatusBit.ValueSuspect] = value;
				_Status[(int)StatusBit.Calculating] = false;

				if ( value == true && IsReferenceBad == true )
					IsReferenceBad = false;

				if ( value && IsValueIncorrect )
					IsValueIncorrect = false;
			}
		}
//		public bool IsExistenceUpToDate
//		{
//			get
//			{
//				return !m_Status[(int)StatusBit.ExistenceSuspect];
//			}
//			set
//			{
//				m_Status[(int)StatusBit.ExistenceSuspect] = !value;
//			}
//		}
		public bool IsExistenceSuspect
		{
			get
			{
				return _Status[(int)StatusBit.ExistenceSuspect];
			}
			set
			{
				_Status[(int)StatusBit.ExistenceSuspect] = value;
			}
		}

		public bool IsCalculating
		{
			get
			{
				return _Status[(int)StatusBit.Calculating];
			}
			set
			{
				_Status[(int)StatusBit.Calculating] = value;
			}
		}
		public bool IsCalculated
		{
			get
			{
				return _Status[(int)StatusBit.Calculated];
			}
			set
			{
				_Status[(int)StatusBit.Calculated] = value;
			}
		}
		public bool IsLocalMembers
		{
			get
			{
				return _Status[(int)StatusBit.LocalMembersOnly];
			}
			set
			{
				_Status[(int)StatusBit.LocalMembersOnly] = value;
			}
		}
		public bool Dynamic
		{
			get
			{
				return _Status[(int)StatusBit.Dynamic];
			}
			set
			{
				_Status[(int)StatusBit.Dynamic] = value;
			}
		}
		public bool IsReferenceBad
		{
			get
			{
				return _Status[(int)StatusBit.BadReference];
			}
			set
			{
				_Status[(int)StatusBit.BadReference] = value;
			}	
		}
		public bool IsValueIncorrect
		{
			get
			{
				return _Status[(int)StatusBit.ValueIncorrect];
			}
			set
			{
				_Status[(int)StatusBit.ValueIncorrect] = value;
			}
		}
		public bool IsEvaluationForce
		{
			get
			{
				return _Status[(int)StatusBit.ForceEvaluation];
			}
			set
			{
				_Status[(int)StatusBit.ForceEvaluation] = value;
			}
		}
		public bool IsInternal
		{
			get
			{
				return _Status[(int)StatusBit.Internal];
			}
			set
			{
				_Status[(int)StatusBit.Internal] = value;
			}
		}
		public bool IsTemporary
		{
			get
			{
				return _Status[(int)StatusBit.Temporary];
			}
			set
			{
				_Status[(int)StatusBit.Temporary] = value;
			}
		}
		public bool IsPrivate
		{
			get
			{
				return _Status[(int)StatusBit.Private];
			}
			set
			{
				_Status[(int)StatusBit.Private] = value;
			}
		}
		public bool IsPublic
		{
			get
			{
				return !_Status[(int)StatusBit.Private];
			}
			set
			{
				_Status[(int)StatusBit.Private] = !value;
			}
		}
	}
}
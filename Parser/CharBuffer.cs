
using System;
using System.Text;
using System.IO;

namespace FoundryCore 
{
	public class CharBuffer 
	{
		private string _Code;
		private int[] _Buffer;
		private int _Depth;
		private int _Pos;
		
		public CharBuffer(StreamReader sr, int depth) 
		{
			_Buffer = new int[depth];	
			// file to string
			_Code = sr.ReadToEnd();		
			_Depth = (depth > _Code.Length ? _Code.Length : depth);				
			SetPos(0);
		}
		
		public CharBuffer(string s, int depth) 
		{
			_Buffer = new int[depth];	
			// file to string
			_Code = s;		
			_Depth = (depth > _Code.Length ? _Code.Length : depth);				
			SetPos(0);
		}
		
		public string Buffer()
		{
			return _Code;
		}
		public string BufferConsumed()
		{
			return _Code.Substring(0,_Pos);
		}
		public string BufferToRead()
		{
			return _Code.Substring(_Pos);
		}
		public int GetPos() 
		{ 		
			// first symbol in buffer accordance ipos==2 
			return (_Pos-2); 
		}		
		public void SetPos(int index)	
		{
			try
			{
				_Pos = index;
				_Buffer[_Depth-1] = _Code[_Pos++];
			}
			catch {}
		}	
		
		public int LA(int i) 
		{
			if ( i >= 1 && i <= _Depth ) {
				return _Buffer[i-1];
			}
			return 0;
		}
		
		public void Consume() 
		{
			for(int i=0; i<_Depth-1; i++) {
			    // oldvalue = newvalue
				_Buffer[i] = _Buffer[i+1];
			}
			try {
				// set new value
				if (_Pos == _Code.Length) {
					// read next symbol frm buffer
					// end buffers
					_Buffer[_Depth-1] = 0;
				} else {
					_Buffer[_Depth-1] = _Code[_Pos++];
				}
				
			} 
			catch (Exception e) 
			{
				ApprenticeObject.ReportException(e);
			}
		}
	}
}
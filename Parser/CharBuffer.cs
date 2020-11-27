
using System;
using System.Text;
using System.IO;

namespace FoundryCore 
{
	public class CharBuffer 
	{
		private string m_sCode;
		private int[] m_iBuffer;
		private int m_iDepth;
		private int m_iPos;
		
		public CharBuffer(StreamReader sr, int depth) 
		{
			m_iBuffer = new int[depth];	
			// file to string
			m_sCode = sr.ReadToEnd();		
			m_iDepth = (depth > m_sCode.Length ? m_sCode.Length : depth);				
			SetPos(0);
		}
		
		public CharBuffer(string s, int depth) 
		{
			m_iBuffer = new int[depth];	
			// file to string
			m_sCode = s;		
			m_iDepth = (depth > m_sCode.Length ? m_sCode.Length : depth);				
			SetPos(0);
		}
		
		public string Buffer()
		{
			return m_sCode;
		}
		public string BufferConsumed()
		{
			return m_sCode.Substring(0,m_iPos);
		}
		public string BufferToRead()
		{
			return m_sCode.Substring(m_iPos);
		}
		public int GetPos() 
		{ 		
			// first symbol in buffer accordance ipos==2 
			return (m_iPos-2); 
		}		
		public void SetPos(int index)	
		{
			try
			{
				m_iPos = index;
				m_iBuffer[m_iDepth-1] = m_sCode[m_iPos++];
			}
			catch {}
		}	
		
		public int LA(int i) 
		{
			if ( i >= 1 && i <= m_iDepth ) {
				return m_iBuffer[i-1];
			}
			return 0;
		}
		
		public void Consume() 
		{
			for(int i=0; i<m_iDepth-1; i++) {
			    // oldvalue = newvalue
				m_iBuffer[i] = m_iBuffer[i+1];
			}
			try {
				// set new value
				if (m_iPos == m_sCode.Length) {
					// read next symbol frm buffer
					// end buffers
					m_iBuffer[m_iDepth-1] = 0;
				} else {
					m_iBuffer[m_iDepth-1] = m_sCode[m_iPos++];
				}
				
			} 
			catch (Exception e) 
			{
				ApprenticeObject.ReportException(e);
			}
		}
	}
}
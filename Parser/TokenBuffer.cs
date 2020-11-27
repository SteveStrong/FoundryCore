
using System;
using System.Text;
using System.IO;

namespace FoundryCore
{
	public class TokenBuffer {
		private Token[] m_TokenStack;
		private Lexer m_oLexer;
		private int k;

		public TokenBuffer(Lexer oLexer,int k) 
		{
			this.k = k;
			m_TokenStack = new Token[k];
			m_oLexer = oLexer;
			try {
				for(int i=0;i<k;i++) {
					m_TokenStack[i] = m_oLexer.GetToken();
				}
			} 
			catch (Exception e) 
			{
				ApprenticeObject.ReportException(e);
			}
		}
		public void setPos(int i) { 
			m_oLexer.SetPos(i); 
		}
		//public int getPos() { return m_oLexer.GetPos(); }
		
		public Token LA(int i) {
			if(i>=1 && i<=k) {
				return m_TokenStack[i-1];
			}
			return null;
		}
		
		public void consume() {
			for(int i=0;i<k-1;i++) {
				m_TokenStack[i]=m_TokenStack[i+1];
			}
			try {
				m_TokenStack[k-1] = m_oLexer.GetToken();
				//Console.WriteLine("Token    ={0}",buf[k-1].Text);
			} 
			catch (Exception e) 
			{
				ApprenticeObject.ReportException(e);
			}
		}
	}
}
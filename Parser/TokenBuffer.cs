
using System;
using System.Text;
using System.IO;

namespace FoundryCore
{
	public class TokenBuffer {
		private Token[] _TokenStack;
		private Lexer _Lexer;
		private int k;

		public TokenBuffer(Lexer oLexer,int k) 
		{
			this.k = k;
			_TokenStack = new Token[k];
			_Lexer = oLexer;
			try {
				for(int i=0;i<k;i++) {
					_TokenStack[i] = _Lexer.GetToken();
				}
			} 
			catch (Exception e) 
			{
				ApprenticeObject.ReportException(e);
			}
		}
		public void setPos(int i) { 
			_Lexer.SetPos(i); 
		}
		//public int getPos() { return m_oLexer.GetPos(); }
		
		public Token LA(int i) {
			if(i>=1 && i<=k) {
				return _TokenStack[i-1];
			}
			return null;
		}
		
		public void consume() {
			for(int i=0;i<k-1;i++) {
				_TokenStack[i]=_TokenStack[i+1];
			}
			try {
				_TokenStack[k-1] = _Lexer.GetToken();
				//Console.WriteLine("Token    ={0}",buf[k-1].Text);
			} 
			catch (Exception e) 
			{
				ApprenticeObject.ReportException(e);
			}
		}
	}
}
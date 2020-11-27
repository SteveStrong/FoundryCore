
using System;
using System.Text;
using System.IO;
using System.Collections;
using System.Reflection;

namespace FoundryCore 
{
	public enum TokenID 
	{
		IDENTIFIER,
		COMMENT,
		LBRACK,
		RBRACK,	
		LPAREN,
		RPAREN,
		LBRACE,
		RBRACE,
		QUOTE,
		COMMA,
		SEMICOLON,
		COLON,
		INTEGER,
		DOUBLE,
		STRING,
		BOOLEAN,
		AMPERSAND,

		PLUS,
		MINUS,
		MULT,
		DIV,
		EQUALS,
		NOTEQUALTO,
		GREATER,
		GREATEREQ,
		LESS,
		LESSEQ,
		NOT,
		AND,
		OR,
		XOR,
		MEMBEROF,
		NOTMEMBEROF,
		HASMEMBER,
		DOESNOTHAVEMEMBER,
		HASANYMEMBER,
		DOESNOTHAVEANYMEMBER,
		ISCONCEPT,
		ISNOTCONCEPT,
//		ISCLASS,
//		ISNOTCLASS,
//		ISTYPE,
//		ISNOTTYPE,
		LIKE,
		STARTSWITH,
		ENDSWITH,
		CONTAINS,
		DOESNOTCONTAIN,
		IN,

		FOREACH,
		IF,
		IIF,
		IIF3,
		THEN,
		ELSE,
		ENDIF,
		WHILE,
		ENDWHILE,
		FUNCTION,
		OPERATOR,
		LOOP,
		EXIT,
		FOR,
		NEXT,
		DO,
		OTHERWISE,
		SWITCH,
		CASE,

		END,
		TRUE,
		FALSE,
		YES,
		NO,

		SHARP,
		ATSIGN,
		PERIOD,
		QUESTIONMARK,

		BREAK,
		CONTINUE,
		RETURN

	}

	public enum TokenClass 
	{
		SUM,
		MULT,
		LOGICAL,
		REFERENCE,
		LITERIAL,
		QUOTED,
		FUNCTION,
		METHOD,
		FIELD,
		EVENT,
		INDEX,
		SET,
		ARGS,
		COMPARE,
		CONTROL,
		OTHER
	}
	public class Token 
	{
		private string m_sText;
		private TokenID m_ID;
		private TokenClass m_Class;

		// constructor
		public Token(TokenClass Class, TokenID ID, string sText) 
		{
			m_ID = ID;
			m_Class = Class;
			m_sText = sText;
		}
		public string Text {
			get { return ( m_sText ); }
			set { m_sText = value; }
		}
		public TokenID ID {
			get { return ( m_ID ); }
			set { m_ID = value; }
		}
		public TokenClass Class 
		{
			get { return ( m_Class ); }
			set { m_Class = value; }
		}
	} 
	
	public class Lexer : ApprenticeObject
	{
		static ApprenticeHash m_oTokens = null;

		private const int LOOKAHEAD_DEPTH = 1;
		private CharBuffer m_oInput;
		private Stack m_oStack = new Stack();
		private char m_currentchar;
		
		public Lexer( StreamReader f ) 
		{
			m_oInput = new CharBuffer(f,LOOKAHEAD_DEPTH);
			InitHash();
			//Console.WriteLine("pos ={0}",getPos());
		}
		
		public Lexer( string s ) 
		{
			m_oInput = new CharBuffer(s,LOOKAHEAD_DEPTH);
			InitHash();
		}
		public override void Purge()
		{
			m_oStack.Clear();
			m_oStack  = null;
			m_oInput = null;
			base.Purge ();
		}

		public string Buffer()
		{
			return m_oInput.Buffer();
		}
		public string BufferConsumed()
		{
			return m_oInput.BufferConsumed();
		}
		public string BufferToRead()
		{
			return m_oInput.BufferToRead();
		}
		private int LA ( int i ) { return m_oInput.LA(i); }
		public void Consume() { m_oInput.Consume(); }
		private void Match ( int c ) { Consume();}
		
		public int GetPos() { return m_oInput.GetPos(); }
		public void SetPos(int i) { m_oInput.SetPos(i); }
	
		public static void AddTok(TokenID i, string s, TokenClass c)
		{
			m_oTokens.Add(s, i);
			m_oTokens.Add(i, c);
		}

		private ApprenticeHash InitHash()
		{
			if ( m_oTokens != null) 
				return m_oTokens;

			m_oTokens = new ApprenticeHash(50);

			AddTok(TokenID.EQUALS,		"equals",			TokenClass.COMPARE);
			AddTok(TokenID.NOTEQUALTO,	"notequalto",		TokenClass.COMPARE);
			AddTok(TokenID.GREATER,		"greaterthan",		TokenClass.COMPARE);
			AddTok(TokenID.GREATEREQ,	"greaterthanequal",	TokenClass.COMPARE);
			AddTok(TokenID.LESS,		"lessthan",			TokenClass.COMPARE);
			AddTok(TokenID.LESSEQ,		"lessthanequal",	TokenClass.COMPARE);
			AddTok(TokenID.MEMBEROF,	"memberof",			TokenClass.COMPARE);
			AddTok(TokenID.NOTMEMBEROF,	"notmemberof",		TokenClass.COMPARE);
			AddTok(TokenID.HASMEMBER,	"hasmember",		TokenClass.COMPARE);
			AddTok(TokenID.DOESNOTHAVEMEMBER,	"doesnothavemember",		TokenClass.COMPARE);
			AddTok(TokenID.HASANYMEMBER,	"hasanymember",		TokenClass.COMPARE);
			AddTok(TokenID.DOESNOTHAVEANYMEMBER,	"doesnothaveanymember",		TokenClass.COMPARE);

			//AddTok(TokenID.SORT,		"sort",			TokenClass.COMPARE);
			AddTok(TokenID.ISCONCEPT,	"isconcept",	TokenClass.COMPARE);
			AddTok(TokenID.ISNOTCONCEPT,"isnotconcept",	TokenClass.COMPARE);
//			AddTok(TokenID.ISCLASS,		"isclass",		TokenClass.COMPARE);
//			AddTok(TokenID.ISNOTCLASS,	"isnotclass",	TokenClass.COMPARE);
//			AddTok(TokenID.ISTYPE,	    "istype",		TokenClass.COMPARE);
//			AddTok(TokenID.ISNOTTYPE,	"isnottype",	TokenClass.COMPARE);
			AddTok(TokenID.LIKE,	    "like",			TokenClass.COMPARE);
			//AddTok(TokenID.IN,		"in",			TokenClass.COMPARE);
			AddTok(TokenID.STARTSWITH,	"startswith",	TokenClass.COMPARE);
			AddTok(TokenID.ENDSWITH,	"endswith",		TokenClass.COMPARE);
			AddTok(TokenID.CONTAINS,	"contains",		TokenClass.COMPARE);
			AddTok(TokenID.DOESNOTCONTAIN,		"doesnotcontain",		TokenClass.COMPARE);

			AddTok(TokenID.OR,			"or",		TokenClass.LOGICAL);
			AddTok(TokenID.AND,			"and",		TokenClass.LOGICAL);

			AddTok(TokenID.TRUE,		"true",		TokenClass.LITERIAL);
			AddTok(TokenID.FALSE,		"false",	TokenClass.LITERIAL);
			AddTok(TokenID.YES,			"yes",		TokenClass.LITERIAL);
			AddTok(TokenID.NO,			"no",	    TokenClass.LITERIAL);

			AddTok(TokenID.IIF,			"iif",		TokenClass.CONTROL );
			AddTok(TokenID.IIF3,		"iif3",		TokenClass.CONTROL );
			AddTok(TokenID.IF,			"if",		TokenClass.CONTROL );
			AddTok(TokenID.THEN,		"then",		TokenClass.CONTROL);
			AddTok(TokenID.ELSE,		"else",		TokenClass.CONTROL);
			AddTok(TokenID.ENDIF,		"endif",	TokenClass.CONTROL);
			AddTok(TokenID.WHILE,		"while",	TokenClass.CONTROL);
			AddTok(TokenID.SWITCH,		"switch",	TokenClass.CONTROL );
			AddTok(TokenID.CASE,		"case",		TokenClass.CONTROL );
			AddTok(TokenID.FOREACH,		"foreach",	TokenClass.CONTROL );
			AddTok(TokenID.FOR,			"for",		TokenClass.CONTROL);
			AddTok(TokenID.DO,			"do",		TokenClass.CONTROL);
			AddTok(TokenID.BREAK,		"break",	TokenClass.CONTROL);
			AddTok(TokenID.CONTINUE,	"continue", TokenClass.CONTROL);
			AddTok(TokenID.RETURN,		"return",	TokenClass.CONTROL);

			return m_oTokens;
		}

		public void PutToken(Token oTok) 
		{
			if ( oTok != null )
				m_oStack.Push(oTok);
		}

		public Token PeekToken() 
		{
			Token oTok = GetToken();
			PutToken(oTok);
			return oTok;
		}

		public char RemoveWhiteSpace()
		{
			char c = (char)LA(1); //Eat up all the white spaces
			while ( char.IsWhiteSpace(c) )
			{
				Consume();
				c = (char)LA(1);
			}
			return c;
		}

		public Token GetToken() 
		{
			Token oReturnValue = null;

			if (m_oStack.Count != 0 ) 
				return (Token)m_oStack.Pop();

			char c = RemoveWhiteSpace();  //Eat up all the white spaces

			m_currentchar = c;

			Consume();				
			switch ( m_currentchar ) {
				case (char)0:
					break;
				case (char)8220:
				case '"':
					oReturnValue = QuotedText();	
					break;
				case (char)39:
					oReturnValue = LiteralText();
					string sText = oReturnValue.Text;
					if ( sText.Length > 4 )
					{
						char[] sString = oReturnValue.Text.ToCharArray();
						if ( sString[1] == '"' && sString[sString.Length-2] == '"' )
							oReturnValue.Text = sText.Substring(1,sText.Length-2);
					}
					break;
				case '!':
					switch ((char)LA(1))
					{
						case '=':
							Consume();
							oReturnValue = new Token(TokenClass.COMPARE, TokenID.NOTEQUALTO, "!=" );
							break;
						default:
							oReturnValue = new Token(TokenClass.COMPARE, TokenID.NOT, "!" );
							break;
					}
					break;
				case '>':
					switch ((char)LA(1))
					{
						case '=':
							Consume();
							oReturnValue = new Token(TokenClass.COMPARE, TokenID.GREATEREQ, ">=" );
							break;
						default:
							oReturnValue = new Token(TokenClass.COMPARE, TokenID.GREATER, ">" );
							break;
					}
					break;
				case '<':
					switch ((char)LA(1))
					{
						case '=':
							Consume();
							oReturnValue = new Token(TokenClass.COMPARE, TokenID.LESSEQ, "<=" );
							break;
						case '>':
							Consume();
							oReturnValue = new Token(TokenClass.COMPARE, TokenID.NOTEQUALTO, "<>" );
							break;
						default:
							oReturnValue = new Token(TokenClass.COMPARE, TokenID.LESS, "<" );
							break;
					}
					break;
				case '=':
					oReturnValue = new Token(TokenClass.COMPARE, TokenID.EQUALS, "=" );
					if ( (char)LA(1) != '=' )
						break;
						
					Consume();
					oReturnValue = new Token(TokenClass.COMPARE, TokenID.EQUALS, "==" );
					break;
				case ':':
					oReturnValue = new Token(TokenClass.COMPARE, TokenID.COLON , ":" );
					break;
				case '*':
					oReturnValue = new Token(TokenClass.MULT, TokenID.MULT, "*" );
					break;
				case '/':
					oReturnValue = new Token(TokenClass.MULT, TokenID.DIV, "/" );
					break;
				case '{':
					oReturnValue = new Token(TokenClass.SET , TokenID.LBRACE, "{" );
					break;
				case '}':
					oReturnValue = new Token(TokenClass.SET, TokenID.RBRACE, "}" );
					break;
				case '(':
					oReturnValue = new Token(TokenClass.ARGS , TokenID.LPAREN, "(" );
					break;
				case ')':
					oReturnValue = new Token(TokenClass.ARGS, TokenID.RPAREN, ")" );
					break;
				case ',':
					oReturnValue = new Token(TokenClass.OTHER, TokenID.COMMA, "," );
					break;
				case '&':
					oReturnValue = new Token(TokenClass.SUM, TokenID.AMPERSAND , "&" );
					break;
				case ';':
					oReturnValue = new Token(TokenClass.OTHER, TokenID.SEMICOLON , ";" );
					break;
				case '[':
					oReturnValue = new Token(TokenClass.REFERENCE , TokenID.LBRACK, "[" );
					break;
				case ']':
					oReturnValue = new Token(TokenClass.REFERENCE, TokenID.RBRACK, "]" );
					break;
				case '#':
					oReturnValue = new Token(TokenClass.REFERENCE, TokenID.SHARP, "#" );
					break;
				case '@':
					oReturnValue = new Token(TokenClass.REFERENCE, TokenID.ATSIGN, "@" );
					break;
				case '?':
					oReturnValue = new Token(TokenClass.REFERENCE, TokenID.QUESTIONMARK, "?" );
					break;
				case '+':
					oReturnValue = new Token(TokenClass.SUM, TokenID.PLUS, "+" );
					break;
				case '-':
					if ( char.IsNumber((char)LA(1)) || (char)LA(1) == '.' )
						oReturnValue = RealNumber();
					else
						oReturnValue = new Token(TokenClass.SUM, TokenID.MINUS, "-" );
					break;
				case '.':
					if ( char.IsNumber((char)LA(1)) )
						oReturnValue = RealNumber();
					else
						oReturnValue = new Token(TokenClass.REFERENCE, TokenID.PERIOD, "." );
					break;
				case '_':
					oReturnValue = Identifier();
					break;
				default:
					if(char.IsNumber(m_currentchar))
						oReturnValue = RealNumber();
					else if(char.IsLetter(m_currentchar))
						oReturnValue = Identifier();
					break;
			} 

			return oReturnValue;
		}
		
		private Token Identifier() 
		{
			StringBuilder sString = new StringBuilder();
			sString.Append( m_currentchar );

			char c = (char)LA(1);
			while( char.IsLetterOrDigit(c) || c == '_'	) 
			{
				sString.Append( c );
				Consume();
				c = (char)LA(1);
			}

			string sTemp = sString.ToString();
			//DebugTrace("Token =",sTemp);

			if ( m_oTokens.ContainsKey(sTemp.ToLower()) )
			{
				TokenID id = (TokenID)m_oTokens[sTemp.ToLower()];
				TokenClass cls = (TokenClass)m_oTokens[id];
				return new Token( cls, id, sTemp );
			}

			c = RemoveWhiteSpace();
			if ( c == '(' )
				return new Token( TokenClass.CONTROL, TokenID.FUNCTION, sTemp);

			return new Token( TokenClass.REFERENCE, TokenID.IDENTIFIER, sTemp);
		}

		private Token IncludeSpaces() 
		{
			StringBuilder sString = new StringBuilder();
			sString.Append( m_currentchar );
			char c = (char)LA(1);
			while( c != 39 )
			{
				sString.Append( c );
				Consume();
				c = (char)LA(1);
			}
			Consume();

			string sTemp = sString.ToString();
			
			if ( m_oTokens.ContainsKey(sTemp.ToLower()) )
			{
				TokenID id = (TokenID)m_oTokens[sTemp.ToLower()];
				TokenClass cls = (TokenClass)m_oTokens[id];
				return new Token( cls, id, sTemp );
			}

			c = RemoveWhiteSpace();
			if ( c == '(' )
				return new Token( TokenClass.CONTROL, TokenID.FUNCTION, sTemp);

			return new Token( TokenClass.REFERENCE, TokenID.IDENTIFIER, sTemp);
		}

		private Token OldQuotedText() {
			StringBuilder sString = new StringBuilder();
			char c = (char)LA(1);

			while( c != 0 && c != '"' && c <= 255 ) 
			{
				sString.Append( c );
				Consume();
				c = (char)LA(1);
			}
			// skip "
			Consume();
			return new Token( TokenClass.QUOTED, TokenID.STRING, sString.ToString() );
		}
		private Token QuotedText() 
		{
			StringBuilder sString = new StringBuilder();
			char c = (char)LA(1);

			while( c != 0 && c <= 255 ) 
				switch ( c )
				{
					case '"':
						Consume();
						char d = (char)LA(1);
						if ( d != '"' )
							c = (char)0;
						else
						{
							sString.Append( '"' );
							Consume();
							c = (char)LA(1);
						}
						break;
					default:
						sString.Append( c );
						Consume();
						c = (char)LA(1);
						break;
				}

			return new Token( TokenClass.QUOTED, TokenID.STRING, sString.ToString() );
		}
		private Token LiteralText() 
		{
			StringBuilder sString = new StringBuilder();
			sString.Append( m_currentchar );
			char c = (char)LA(1);
			while( c != 0 && c != 39 && c <= 255  ) 
			{
				sString.Append( c );
				Consume();
				c = (char)LA(1);
			}
			sString.Append( c );
			Consume();
			return new Token( TokenClass.LITERIAL, TokenID.STRING, sString.ToString() );
		}
		
		public string ReadUntil(char cStop) 
		{
			StringBuilder sString = new StringBuilder();
			char c = (char)LA(1);

			while( c != 0 && c != cStop && c <= 255 ) 
			{
				sString.Append( c );
				Consume();
				c = (char)LA(1);
			}
			// skip cStop
			Consume();
			return sString.ToString();
		}

//		private Token Number() 
//		{
//			StringBuilder sString = new StringBuilder();
//			sString.Append( m_currentchar );
//			bool bInteger = ( m_currentchar == '.' ) ? false : true;
//
//			char c = (char)LA(1);
//			while( char.IsNumber(c) || c == '.' ) {
//				bInteger = ( c == '.' ) ? false : bInteger;
//				sString.Append( c );
//				Consume();
//				c = (char)LA(1);
//			}
//	
//			return new Token( TokenClass.LITERIAL, ((bInteger) ? TokenID.INTEGER : TokenID.DOUBLE), sString.ToString() );
//		}
		private Token RealNumber() 
		{
			StringBuilder sString = new StringBuilder();
			sString.Append( m_currentchar );
			bool bInteger = ( m_currentchar == '.' ) ? false : true;

			char c = (char)LA(1);
			while( char.IsNumber(c) || c == '.' ) 
			{
				bInteger = ( c == '.' ) ? false : bInteger;
				sString.Append( c );
				Consume();

				c = (char)LA(1);
				if ( c == 'e' || c == 'E' ) 
				{
					bInteger = false;
					sString.Append( c );
					Consume();

					c = (char)LA(1);
					if ( c == '+' || c == '-' )
					{
						sString.Append( c );
						Consume();
						c = (char)LA(1);
					}
				}
			}
	
			return new Token( TokenClass.LITERIAL, ((bInteger) ? TokenID.INTEGER : TokenID.DOUBLE), sString.ToString() );
		}
	}
} 
using System;
using System.IO;

namespace FoundryCore
{

    /// <summary>
    /// Summary description for Parser.
    /// </summary>
    public class Parser
	{
		private Lexer _Lexer; 
		public bool OnlyLiterials = false;
		public bool AsConstraints = false;
		public bool AsFilters = false;
		public bool AsBooleans = false;

		public Parser(string sBuffer)
		{
			//DebugTrace("Parser",sBuffer);
			_Lexer = new Lexer(sBuffer);
		}
		public Parser(StreamReader fStream)
		{
			_Lexer = new Lexer(fStream);
		}

		public Operator LiteralOrReference(string sText)
		{
			Token oNextTok = _Lexer.PeekToken();

			if ( oNextTok == null )
				return new Literal(sText,sText);

			switch ( oNextTok.ID )
			{
				case TokenID.ATSIGN:
				case TokenID.SHARP:
				case TokenID.QUESTIONMARK:
					break;
				default:
					return new Literal(sText,sText);
			}

			sText = sText.Replace("'","");
			_Lexer.PutToken(new Token( TokenClass.REFERENCE, TokenID.IDENTIFIER, sText));
			return ReadFactor();
		}

		public Operator ReadFactor()
		{
			Token oTok;
			string sText;
			Operator obj;

			oTok = _Lexer.GetToken();
			if ( oTok == null )
				ArgumentExpectedError("Token not found");

			sText = oTok.Text; 
			string sUtext = sText.ToUpper();

			switch (oTok.ID) {
				case TokenID.DOUBLE:
					if ( sText.StartsWith(".") )
						sText = "0" + sText;

					double dValue = System.Convert.ToDouble(sText);
					obj = new Constant(sText,dValue);
					//ReadUnits(obj);
					break;
				case TokenID.INTEGER:
					try
					{
						int iValue = System.Convert.ToInt32(sText);
						obj = new Constant(sText,iValue);
					}
					catch(OverflowException)
					{
						obj = new Constant(sText,System.Convert.ToDouble(sText));
					}
					//ReadUnits(obj);
					break;
				case TokenID.STRING:
					if (oTok.Class == TokenClass.QUOTED)
						obj = new Constant(sText,sText);
					else
						obj = LiteralOrReference(sText);
					break;

				case TokenID.TRUE: 
				case TokenID.FALSE: 
				case TokenID.YES: 
				case TokenID.NO: 
					obj = new Constant(sText.ToLower());
					break;

				case TokenID.MINUS: //Unary operators
					obj = new FunctionOperator("Negate");
					obj.AddChildObject(ReadFactor());
					break;
				case TokenID.PLUS: 
					obj = new FunctionOperator("NoOp");
					obj.AddChildObject(ReadFactor());
					break;

//				case TokenID.QUOTE:  // "" found in a string
//					break;

				case TokenID.LPAREN:
					_Lexer.PutToken(oTok);
					obj = ReadGroup();
					break;

				case TokenID.LBRACE:
					_Lexer.PutToken(oTok);
					obj = ReadSet();
					break;

				case TokenID.FUNCTION:
					_Lexer.PutToken(oTok);
					obj = ReadFunction();
					break;

				case TokenID.LBRACK:
					_Lexer.PutToken(oTok);
					obj = ReadFieldReference();
					break;

				case TokenID.ATSIGN:
				case TokenID.QUESTIONMARK:
					_Lexer.PutToken(oTok);
					obj = ReadEvaluatedComponentReference();
					break;

				case TokenID.IDENTIFIER:
					if ( sUtext == "PI" || sUtext == "TRUE" || sUtext == "FALSE" )
					{
						obj = new Constant(sText);
					} 
					else if ( OnlyLiterials == false)
					{
						_Lexer.PutToken(oTok);
						obj = ReadEvaluatedValueReference();
					} 
					else if ( Common.IsNumber(sText) == true )
					{
						obj = new Constant(sText, System.Convert.ToDouble(sText) );
					}
					else
					{
						obj = new Constant(sText,sText);
					}
					break;
				default:
					switch (oTok.Class) {
						case TokenClass.CONTROL: 
							_Lexer.PutToken(oTok);
							obj = ReadControl();
							break;
						default:
							obj = null;
							break;
					}
					break;
			}
			return obj;
		}

		public Operator ReadFunction()
		{
			Token oTok;
			Operator obj, obj1;

			oTok = _Lexer.PeekToken();
			if ( oTok != null && oTok.ID != TokenID.FUNCTION) 
				return null;

			oTok = _Lexer.GetToken();
			obj = new FunctionOperator(oTok.Text);

			oTok = _Lexer.PeekToken();
			if ( oTok != null && oTok.ID != TokenID.LPAREN )
				MissingItemError("ReadFunction","(");
			else
				_Lexer.GetToken(); //Consume (

			oTok = _Lexer.GetToken();
			while ( oTok != null && oTok.ID != TokenID.RPAREN) 
			{

				_Lexer.PutToken(oTok);
				obj1 = ReadArgument();

				if ( obj1 == null) 
					ArgumentExpectedError("ReadFunction");

				obj.AddChildObject(obj1);
				oTok = _Lexer.GetToken();

				if ( oTok != null && oTok.ID == TokenID.COMMA )
					oTok = _Lexer.GetToken();
			}

			return obj;		
		}

		public Operator ReadEvaluation(Token oFunctionTok)
		{
			Token oTok;
			Operator obj, obj1;

			obj = new FunctionOperator(oFunctionTok.Text);

			oTok = _Lexer.PeekToken();
			if ( oTok != null && oTok.ID != TokenID.LPAREN )
				MissingItemError("ReadEvaluation","(");
			else
				_Lexer.GetToken(); //Consume (

			oTok = _Lexer.GetToken();
			while ( oTok != null && oTok.ID != TokenID.RPAREN) 
			{

				_Lexer.PutToken(oTok);
				obj1 = ReadArgument();

				if ( obj1 == null) 
					ArgumentExpectedError("ReadEvaluation");

				obj.AddChildObject(obj1);
				oTok = _Lexer.GetToken();

				if ( oTok != null && oTok.ID == TokenID.COMMA )
					oTok = _Lexer.GetToken();
			}

			return obj;		
		}
		public Operator ReadControl()
		{
			Token oTok;
			Operator obj;

			oTok = _Lexer.PeekToken();
			if ( oTok != null && oTok.Class != TokenClass.CONTROL) 
				return null;
			else
				oTok = _Lexer.GetToken();

			switch (oTok.ID)
			{
				case TokenID.IIF:
					obj = ReadInlineIf(new BranchOperator(oTok.Text));
					break;
				case TokenID.IIF3:
					obj = ReadInlineIf3(new BranchOperator(oTok.Text));
					break;
//				case TokenID.IF:
//					oTok.Text = "IIF";
//					obj = ReadIfThenElse(new BranchOperator(oTok.Text));
//					break;
				case TokenID.SWITCH:
					obj = ReadSwitch(new SwitchOperator(oTok.Text));
					break;
				case TokenID.CASE:
					obj = ReadCase(new SwitchOperator(oTok.Text));
					break;
				default:
					return null;
			}
			return obj;		
		}
		public Operator ReadInlineIf(Operator obj)
		{
			Token oTok;
			Operator obj1;

			oTok = _Lexer.PeekToken();
			if ( oTok != null && oTok.ID != TokenID.LPAREN )
				MissingItemError("ReadInlineIf","(");
			else
				_Lexer.GetToken(); // consume (

			obj1 = ReadArgument(); //This is the TEST part
			if ( obj1 == null) 
				ArgumentExpectedError("ReadInlineIf");

			obj.AddChildObject(obj1);

			oTok = _Lexer.PeekToken();
			if ( oTok != null && oTok.ID != TokenID.COMMA )
				MissingItemError("ReadInlineIf",",");
			else
				_Lexer.GetToken(); // consule ,

			obj1 = ReadArgument(); //This is the TRUE part
			if ( obj1 == null) 
				ArgumentExpectedError("ReadInlineIf");

			obj.AddChildObject(obj1);

			oTok = _Lexer.PeekToken();
			if ( oTok != null && oTok.ID != TokenID.COMMA )
				MissingItemError("ReadInlineIf",",");
			else
				_Lexer.GetToken(); //Consume ,

			obj1 = ReadArgument(); //This is the FALSE part
			if ( obj1 == null) 
				ArgumentExpectedError("ReadInlineIf");

			obj.AddChildObject(obj1);

			oTok = _Lexer.GetToken();
			if ( oTok != null && oTok.ID != TokenID.RPAREN )
			{
				_Lexer.PutToken(oTok);
				MissingItemError("ReadInlineIf",")");
			}

			return obj;		
		}
		public Operator ReadInlineIf3(Operator obj)
		{
			Token oTok;
			Operator obj1;

			oTok = _Lexer.PeekToken();
			if ( oTok != null && oTok.ID != TokenID.LPAREN )
				MissingItemError("ReadInlineIf3","(");
			else
				_Lexer.GetToken(); // consume (

			obj1 = ReadArgument(); //This is the TEST part
			if ( obj1 == null) 
				ArgumentExpectedError("ReadInlineIf3");

			obj.AddChildObject(obj1);

			oTok = _Lexer.PeekToken();
			if ( oTok != null && oTok.ID != TokenID.COMMA )
				MissingItemError("ReadInlineIf3",",");
			else
				_Lexer.GetToken(); // consule ,

			obj1 = ReadArgument(); //This is the TRUE part
			if ( obj1 == null) 
				ArgumentExpectedError("ReadInlineIf3");

			obj.AddChildObject(obj1);

			oTok = _Lexer.PeekToken();
			if ( oTok != null && oTok.ID != TokenID.COMMA )
				MissingItemError("ReadInlineIf3",",");
			else
				_Lexer.GetToken(); //Consume ,

			obj1 = ReadArgument(); //This is the Null part
			if ( obj1 == null) 
				ArgumentExpectedError("ReadInlineIf3");

			obj.AddChildObject(obj1);

			oTok = _Lexer.PeekToken();
			if ( oTok != null && oTok.ID != TokenID.COMMA )
				MissingItemError("ReadInlineIf3",",");
			else
				_Lexer.GetToken(); //Consume ,

			obj1 = ReadArgument(); //This is the FALSE part
			if ( obj1 == null) 
				ArgumentExpectedError("ReadInlineIf3");

			obj.AddChildObject(obj1);

			oTok = _Lexer.GetToken();
			if ( oTok != null && oTok.ID != TokenID.RPAREN )
			{
				_Lexer.PutToken(oTok);
				MissingItemError("ReadInlineIf3",")");
			}

			return obj;		
		}

		public Operator ReadSwitch(Operator obj)
		{
			Token oTok;
			Operator obj1;

			oTok = _Lexer.PeekToken();
			if ( oTok != null && oTok.ID != TokenID.LPAREN )
				MissingItemError("ReadSwitch","(");
			else
				_Lexer.GetToken(); // consume (

			obj1 = ReadArgument(); //This is the switch
			if ( obj1 == null) 
				ArgumentExpectedError("ReadSwitch");

			obj.AddChildObject(obj1);

			oTok = _Lexer.PeekToken();
			if ( oTok != null && oTok.ID != TokenID.COMMA )
				MissingItemError("ReadSwitch",",");
			else
				_Lexer.GetToken(); // consule ,

			obj1 = ReadArgument(); //This is the case
			if ( obj1 == null) 
				ArgumentExpectedError("ReadSwitch");

			obj.AddChildObject(obj1);


			oTok = _Lexer.GetToken();
			if ( oTok != null && oTok.ID != TokenID.RPAREN )
			{
				_Lexer.PutToken(oTok);
				MissingItemError("ReadSwitch",")");
			}

			return obj;		
		}

		public Operator ReadCase(Operator obj)
		{
			Token oTok;
			Operator obj1;

			oTok = _Lexer.PeekToken();
			if ( oTok != null && oTok.ID != TokenID.LPAREN )
				MissingItemError("ReadCase","(");
			else
				_Lexer.GetToken(); // consume (

			oTok = _Lexer.GetToken();
			while ( oTok != null && oTok.ID != TokenID.RPAREN) 
			{
				_Lexer.PutToken(oTok);
				obj1 = ReadArgument();

				if ( obj1 == null) 
					ArgumentExpectedError("ReadAList");

				obj.AddChildObject(obj1);
				oTok = _Lexer.GetToken();

				if ( oTok != null && oTok.ID == TokenID.COMMA )
					oTok = _Lexer.GetToken();
			}
			return obj;		
		}
		public Operator ReadTerm()
		{
			Token oTok;
			Operator obj, obj1, obj2;

			obj1 = obj = ReadFactor();

			oTok = _Lexer.GetToken();

			while ( oTok != null && oTok.Class == TokenClass.MULT)
			{
				if ( obj1 == null) 
					ArgumentExpectedError("ReadTerm LHS");

				obj = new BinaryOperator(oTok.Text);
				obj.AddChildObject(obj1);
				obj2 = ReadFactor();

				if ( obj2 == null) 
					ArgumentExpectedError("ReadTerm RHS");
					
				obj.AddChildObject(obj2);
				obj1 = obj;
				oTok = _Lexer.GetToken();
			}
			_Lexer.PutToken(oTok);

			return obj;		
		}
		public Operator ReadFieldReference()
		{
			Token oTok = _Lexer.GetToken();
			if ( oTok == null )
				return null;

			if ( oTok.ID != TokenID.LBRACK )
			{
				_Lexer.PutToken(oTok);
				return null;
			}

			string sName = _Lexer.ReadUntil(']');
			return new FieldReference(sName);
		}


		public Operator ReadRemainingReference (Operator oRef)
		{
			Operator oOp = null;
			Token oTok = _Lexer.GetToken();

			while ( oTok != null && (oTok.ID == TokenID.PERIOD || oTok.ID == TokenID.LBRACK))
			{
				bool bIndex = (oTok.ID == TokenID.LBRACK);
				oTok = _Lexer.GetToken();
				if ( oTok == null )
					return oRef;

				switch ( oTok.ID ) 
				{
					case TokenID.FUNCTION:
						oOp = ReadEvaluation(oTok);
						break;
					case TokenID.IDENTIFIER:
					case TokenID.INTEGER:
						if ( bIndex )
						{
							oOp = new IndexReference(oTok.Text);
							oTok = _Lexer.GetToken();
							if ( oTok.ID != TokenID.RBRACK )
								ArgumentExpectedError("ReadReference");
						}
						else
						{
							oOp = new ComponentReference(Common.UnwrapSQ(oTok.Text));
						}
						break;
					default:
						ArgumentExpectedError("ReadReference");
						break;
				}	
				oRef.AddChildObject(oOp);
				oTok = _Lexer.GetToken();
			}
			_Lexer.PutToken(oTok);

			return oRef;
		}
		public Operator ReadEvaluatedValueReference()
		{
			TokenID IDRefType = TokenID.ATSIGN;

			Operator oRef;
			Token oTok, oNextTok;

			oTok = _Lexer.PeekToken();
			if ( oTok == null || oTok.Class != TokenClass.REFERENCE)
				return null;
			else
				oTok = _Lexer.GetToken();

			//loop to construct an evaluating reference
			//that includes properties, and function references
			//Add these elements to the value reference as a tree...

			oNextTok = _Lexer.PeekToken();
			if ( oNextTok == null )
				oRef = new ValueReference(oTok.Text);
			else
				switch(oNextTok.ID)
				{
					case TokenID.ATSIGN:
					case TokenID.SHARP:
					case TokenID.QUESTIONMARK:
						IDRefType = oNextTok.ID;
						oNextTok = _Lexer.GetToken();
						oRef = new ValueAtReference(oTok.Text,(char)oNextTok.Text[0]);

						oTok = _Lexer.PeekToken();
						if ( oTok != null && oTok.ID != TokenID.PERIOD)
							if ( oTok.ID == TokenID.LPAREN || 
								oTok.ID == TokenID.FUNCTION || 
								oTok.Class == TokenClass.REFERENCE )
								_Lexer.PutToken(new Token(TokenClass.REFERENCE,TokenID.PERIOD, "."));
						break;
					default:
						oRef = new ValueReference(oTok.Text);
						break;
				}

			return ReadRemainingReference(oRef);
		}

		public Operator ReadEvaluatedComponentReference()
		{
			Operator oRef;
			Token oTok, oControlTok;

			oControlTok = _Lexer.GetToken();
			if ( oControlTok == null)
				return null;

			switch ( oControlTok.ID )
			{
				case TokenID.ATSIGN:
				case TokenID.SHARP:
				case TokenID.QUESTIONMARK:
					oTok = _Lexer.GetToken();
					string sTok = oTok != null ? Common.UnwrapSQ(oTok.Text) : "";
					oRef = new ComponentAtReference(sTok,(char)oControlTok.Text[0]);
					break;
				case TokenID.LBRACK:
					oTok = _Lexer.GetToken();  //this is the [
					oRef = new FieldReference(Common.UnwrapSQ(oTok.Text));
					oTok = _Lexer.GetToken();
					if ( oTok.ID != TokenID.RBRACK )
						ArgumentExpectedError("ReadEvaluatedComponentReference");
					break;
				default:
					_Lexer.PutToken(oControlTok);
					return null;
			}

			return ReadRemainingReference(oRef);
		}

		public Operator ReadExpression()
		{
			Token oTok;
			Operator obj, obj1, obj2;

			obj1 = obj = ReadTerm();

			//looking for the 7+-236.5 case
			oTok = _Lexer.GetToken();
			if ( oTok != null && oTok.Class == TokenClass.LITERIAL && oTok.Text.StartsWith("-") )
			{
				_Lexer.PutToken(oTok);
				oTok = new Token(TokenClass.SUM, TokenID.PLUS, "+" );
			}

			while ( oTok != null && oTok.Class == TokenClass.SUM)
			{
				if ( obj1 == null) 
					ArgumentExpectedError("ReadExpression LHS");

				obj = new BinaryOperator(oTok.Text);
				obj.AddChildObject(obj1);
				obj2 = ReadTerm();

				if ( obj2 == null) 
					ArgumentExpectedError("ReadExpression RHS");

				obj.AddChildObject(obj2);
				obj1 = obj;
				oTok = _Lexer.GetToken();
			}
			_Lexer.PutToken(oTok);
			return obj;	
		}


		public Operator ReadOperation()
		{
			Token oTok;
			Operator obj, obj1, obj2;

			obj1 = obj = ReadExpression();

			oTok = _Lexer.GetToken();

			while ( oTok != null && oTok.Class == TokenClass.COMPARE)
			{
				if ( obj1 == null) 
					ArgumentExpectedError("ReadOperation LHS");

				obj = new BinaryOperator(oTok.Text);
				obj.AddChildObject(obj1);
				obj2 = ReadExpression();

				if ( obj2 == null) 
					ArgumentExpectedError("ReadOperation RHS");

				obj.AddChildObject(obj2);
				obj1 = obj;
				oTok = _Lexer.GetToken();
			}
			_Lexer.PutToken(oTok);

			return obj;	
		}
		public Operator ReadSet()
		{
			Token oTok;
			Operator obj, obj1;

			oTok = _Lexer.GetToken();

			obj = null;
			if ( oTok != null && oTok.ID != TokenID.LBRACE)
				MissingItemError("ReadSet","{");

			if (OnlyLiterials == false)
				obj = new Set("");
			else
				obj = new ValidValues("ValidValues");
			
			oTok = _Lexer.GetToken();
			while ( oTok != null && oTok.ID != TokenID.RBRACE)
			{
				_Lexer.PutToken(oTok);
				obj1 = null;

				if (OnlyLiterials == false)
					obj1 = ReadArgument();
				else
					obj1 = ReadFactor();

				obj.AddChildObject(obj1);

				oTok = _Lexer.GetToken();
				if ( oTok != null && oTok.ID != TokenID.COMMA)
				{
					_Lexer.PutToken(oTok);
					if ( oTok.ID != TokenID.RBRACE ) 
						MissingItemError("ReadSet",",");
				}

				oTok = _Lexer.GetToken();
			}
			return obj;		
		}
		public Operator ReadGroup()
		{
			Token oTok;
			Operator obj, obj1;

			oTok = _Lexer.GetToken();

			obj = null;
			if ( oTok != null && oTok.ID  == TokenID.LPAREN)
			{
				obj = new GroupOperator(oTok.Text);
				obj1 = ReadArgument();
				if ( obj1 == null)
					ArgumentExpectedError("ReadGroup LHS");

				obj.AddChildObject(obj1);
				oTok = _Lexer.GetToken();
				if ( oTok == null || oTok.ID != TokenID.RPAREN)
					MissingItemError("ReadGroup RHS",")");

			}
			return obj;		
		}
		public Operator ReadLogicalAnd()
		{
			Token oTok;
			Operator obj, obj1, obj2;
	
			obj1 = obj = ReadOperation();

			oTok = _Lexer.GetToken();

			while ( oTok != null && oTok.ID == TokenID.AND ) 
			{
				if ( obj1 == null) 
					ArgumentExpectedError("ReadLogicalAnd LHS");

				obj = new LogicalBinaryOperator(oTok.Text);
				obj.AddChildObject(obj1);
				obj2 = ReadOperation();

				if ( obj2 == null) 
					ArgumentExpectedError("ReadLogicalAnd RHS");

				obj.AddChildObject(obj2);
				obj1 = obj;
				oTok = _Lexer.GetToken();
			}
			_Lexer.PutToken(oTok);

			return obj;
		}

		public Operator ReadLogicalOr()
		{
			Token oTok;
			Operator obj, obj1, obj2;
	
			obj1 = obj = ReadLogicalAnd();

			oTok = _Lexer.GetToken();

			while ( oTok != null && oTok.ID == TokenID.OR  ) 
			{
				if ( obj1 == null) 
					ArgumentExpectedError("ReadLogicalOr LHS");

				obj = new LogicalBinaryOperator(oTok.Text);
				obj.AddChildObject(obj1);
				obj2 = ReadLogicalOr();

				if ( obj2 == null) 
					ArgumentExpectedError("ReadLogicalOr RHS");

				obj.AddChildObject(obj2);
				obj1 = obj;
				oTok = _Lexer.GetToken();
			}
			_Lexer.PutToken(oTok);

			return obj;
		}


		public void MissingItemError(string sTitle, string sMissing)
		{
			throw new ApprenticeSyntaxException(sTitle,BufferConsumed(),sMissing);
		}
		public void ArgumentExpectedError(string sTitle)
		{
			throw new ApprenticeSyntaxException(sTitle,BufferConsumed());
		}
		public string SyntaxCheck()
		{
			try 
			{
				ClearLeadingAndOR();
				Operator op = ReadLogicalOr();
				CheckThatBufferIsEmpty();
				return "";
			}
			catch(ApprenticeSyntaxException eSyntax)
			{
				return eSyntax.ErrorMessage;
			}
			catch(Exception e)
			{
				return e.Message;
			}
		}
		private void ClearLeadingAndOR()
		{
			Token oTok = _Lexer.GetToken();
			if ( oTok != null )
				switch ( oTok.ID )
				{
					case TokenID.AND:
					case TokenID.OR:
						break;
					default:
						_Lexer.PutToken(oTok);
						break;
				}
		}
		public Operator ReadFormula()
		{
			try 
			{
				ClearLeadingAndOR();
				Operator op = ReadLogicalOr();
				CheckThatBufferIsEmpty();
				return op;
			}
			catch(ApprenticeSyntaxException eSyntax)
			{
				throw(eSyntax);
			}
			catch(Exception e)
			{
				ApprenticeObject.ReportException(e);
				return null;
			}		
		}

		public Operator ReadArgument()
		{
			try 
			{
				//string sBuffer = BufferConsumed();
				Operator op = ReadLogicalOr();
				return op;
			}
			catch(ApprenticeSyntaxException eSyntax)
			{
				throw eSyntax;
			}
		}
		public string BufferConsumed()
		{
			return _Lexer.BufferConsumed();
		}
		public bool CheckThatBufferIsEmpty()
		{
			Token oTok = _Lexer.GetToken();

			if ( oTok == null )
				return false;

			throw new ApprenticeSyntaxException("Not all tokens read",BufferConsumed(),oTok);
		}
	}
}

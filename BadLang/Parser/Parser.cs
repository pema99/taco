using System;
using System.Collections.Generic;

namespace BadLang
{
    public class Parser
    {
        private int start;
        private int current;
        private List<Token> tokens;
        private Dictionary<string, FuncDecl> functions;

        public Dictionary<string, FuncDecl> Parse(List<Token> tokens)
        {
            this.tokens = tokens;
            this.start = 0;
            this.current = 0;
            this.functions = new Dictionary<string, FuncDecl>();

            while (!IsAtEnd())
            {
                if (Match(TokenType.Func))
                {
                    FuncDecl func = ParseFunc();

                    if (functions.ContainsKey(func.Name))
                    {
                        throw new Exception("Variable \'" + func.Name + "\' already declared");
                    }

                    functions.Add(func.Name, func);
                }
                else
                {
                    throw new Exception("Invalid token \'" + Peek().Lexeme + "\' at line " + Peek().Line + ", only functions are allowed in global scope.");
                }
            }

            return functions;
        }

        private Expression ParseStatement()
        {
            switch (Peek().Type)
            {
                case TokenType.Var:
                    return ParseVariableDeclaration();

                case TokenType.Print:
                case TokenType.PrintLine:
                    return ParsePrint();

                case TokenType.Identifier:
                    return ParseIdentifier();

                case TokenType.If:
                    return ParseConditional();

                /*case TokenType.While:
                    return While();*/

                case TokenType.Left_Brace:
                    return new BlockExpr(ParseBlock());

                /*case TokenType.Clear:
                    return ClearConsole();*/

                case TokenType.Return:
                    return ParseReturn();

                /*case TokenType.For:
                    return For();*/

                default:
                    throw new Exception("Unexpected token: \'" + Peek().Type + "\' at line " + Peek().Line + ".");
                    break;
            }
        }

        private Block ParseBlock()
        {
            Block result = new Block();

            Advance(TokenType.Left_Brace);
            while (!IsAtEnd() && !Match(TokenType.Right_Brace))
            {
                result.Statements.Add(ParseStatement());
            }
            Advance(TokenType.Right_Brace);

            return result;
        }

        #region Statement parsing
        private FuncDecl ParseFunc()
        {
            Advance(TokenType.Func);
            Token identifier = identifier = Advance(TokenType.Identifier);
            
            Advance(TokenType.Left_Paren);
            List<string> parameters = new List<string>();
            if (Match(TokenType.Identifier))
            {
                while (!Match(TokenType.Right_Paren))
                {
                    Token param = Advance();
                    if (param.Type == TokenType.Identifier)
                    {
                        if (parameters.Contains(param.Lexeme))
                        {
                            throw new Exception("Error at line " + param.Line + ". Two parameters with the same name are not allowed in a function declaration.");
                        }
                        parameters.Add(param.Lexeme);
                    }
                    else if (param.Type == TokenType.Comma);
                    else
                    {
                        throw new Exception("Error at line " + param.Line + ". Only identifiers are allowed in function declarations.");
                    }
                }
            }
            Advance(TokenType.Right_Paren);

            return new FuncDecl(identifier.Lexeme, ParseBlock(), parameters.ToArray());
        }

        private VariableDeclarationExpr ParseVariableDeclaration()
        {
            Advance(TokenType.Var);
            Token identifier = Advance(TokenType.Identifier);
            Advance(TokenType.Equal);
            Expression Intializer = ParseExpression();

            return new VariableDeclarationExpr(identifier.Lexeme, Intializer);
        }

        private FunctionCallExpr ParseFuncCall()
        {
            Token identifier = Advance(TokenType.Identifier);

            Advance(TokenType.Left_Paren);
            List<Expression> Params = new List<Expression>();
            while (!Match(TokenType.Right_Paren))
            {
                Params.Add(ParseExpression());
                if (Match(TokenType.Comma))
                {
                    Advance(TokenType.Comma);
                }
            }
            Advance(TokenType.Right_Paren);

            return new FunctionCallExpr(identifier.Lexeme, Params);
        }

        private Expression ParseIdentifier()
        {
            Token identifier = null;

            switch (PeekNext().Type)
            {
                case TokenType.Plus_Equal:
                case TokenType.Plus_Plus:
                case TokenType.Minus_Equal:
                case TokenType.Minus_Minus:
                case TokenType.Star_Equal:
                case TokenType.Slash_Equal:
                    identifier = Advance(TokenType.Identifier);
                    Token op = Advance();

                    Expression delta = null;
                    switch (op.Type)
                    {
                        case TokenType.Plus_Plus:
                            delta = new ConstantExpr(1f);
                            break;

                        case TokenType.Minus_Minus:
                            delta = new ConstantExpr(1f);
                            break;

                        default:
                            delta = ParseExpression();
                            break;
                    }
                    return new VariableAssigmentExpr
                    (
                        identifier.Lexeme,
                        new BinaryExpr
                        (
                            new VariableLookupExpr(identifier.Lexeme),
                            delta,
                            op.Type
                        )
                    );

                case TokenType.Equal:
                    identifier = Advance(TokenType.Identifier);
                    Advance(TokenType.Equal);
                    return new VariableAssigmentExpr(identifier.Lexeme, ParseExpression());

                //case TokenType.Left_Square:

                case TokenType.Left_Paren:
                    return ParseFuncCall();

                default:
                    throw new Exception("Invalid token \'" + tokens[current + 1].Type + "\' after Identifier on line " + identifier.Line);
            }
        }

        private PrintExpr ParsePrint()
        {
            Token token = Advance();
            return new PrintExpr(ParseExpression(), token.Type == TokenType.PrintLine);
        }

        private ReturnExpr ParseReturn()
        {
            Advance(TokenType.Return);
            return new ReturnExpr(ParseExpression());
        }

        /*private ClearConsoleExpr ClearConsole()
        {
            Advance(TokenType.Clear);
            return new ClearConsoleExpr();
        }*/

        /*private WhileExpr While()
        {
            Advance(TokenType.While);
            Advance(TokenType.Left_Paren);

            Expression Condition = Expression();

            Advance(TokenType.Right_Paren);

            ScopeInfo Body = ParseBlock();

            return new WhileExpr(Condition, Body);
        }

        private ForExpr For()
        {
            Advance(TokenType.For);
            Advance(TokenType.Left_Paren);

            Expression Initial = ParseStatement();

            Advance(TokenType.Comma);

            Expression Condition = Expression();

            Advance(TokenType.Comma);

            Expression Iteration = ParseStatement();

            Advance(TokenType.Right_Paren);

            ScopeInfo Body = ParseBlock();

            return new ForExpr(Initial, Condition, Iteration, Body);
        }*/

        private ConditionalExpr ParseConditional()
        {
            Advance(TokenType.If);
            Advance(TokenType.Left_Paren);

            Expression condition = ParseExpression();

            Advance(TokenType.Right_Paren);

            Block yes = ParseBlock();

            Block no = null;
            if (Match(TokenType.Else))
            {
                Advance(TokenType.Else);

                //Else if
                if (Match(TokenType.If))
                {
                    no = new Block(new List<Expression>() { ParseConditional() });
                }
                //Else
                else
                {
                    no = ParseBlock();
                }
            }

            return new ConditionalExpr(condition, yes, no);
        }
        #endregion

        #region Expression parsing
        private Expression ParseExpression()
        {
            Expression Higher = ParseTerm();

            //+ | - | == | != | <= | >= | < | > | && | || | ^^
            while (Match(TokenType.Plus) || Match(TokenType.Minus) || Match(TokenType.Equal_Equal) || Match(TokenType.Bang_Equal)
               ||  Match(TokenType.Less) || Match(TokenType.Less_Equal) || Match(TokenType.Greater) || Match(TokenType.Greater_Equal)
               ||  Match(TokenType.And) || Match(TokenType.Or) || Match(TokenType.Xor))
            {
                Token Token = Advance();
                Higher = new BinaryExpr(Higher, ParseTerm(), Token.Type);
                if (IsAtEnd())
                {
                    return Higher;
                }
            }

            return Higher;
        }

        private Expression ParseTerm()
        {
            Expression Higher = ParseFactor();

            //* | /
            while (Match(TokenType.Star) || Match(TokenType.Slash))
            {
                Token token = Advance();
                Higher = new BinaryExpr(Higher, ParseFactor(), token.Type);
                if (IsAtEnd())
                {
                    return Higher;
                }
            }

            return Higher;
        }

        private Expression ParseFactor()
        {
            switch (Peek().Type)
            {
                case TokenType.Number:
                    return new ConstantExpr((float)Advance().Literal);

                case TokenType.String:
                    return new ConstantExpr((string)Advance().Literal);

                case TokenType.True:
                case TokenType.False:
                    return new ConstantExpr(Advance().Type == TokenType.True ? true : false);

                case TokenType.Left_Paren:
                    Advance(TokenType.Left_Paren);
                    Expression Higher = ParseExpression();
                    Advance(TokenType.Right_Paren);
                    return Higher;

                case TokenType.Plus:
                case TokenType.Minus:
                case TokenType.Bang:
                    return new UnaryExpr(ParseFactor(), Advance().Type);

                case TokenType.Identifier:
                    if (PeekNext().Type == TokenType.Left_Paren)
                    {
                        return ParseFuncCall();
                    }                 
                    else
                    {
                        return new VariableLookupExpr(Advance().Lexeme);
                    }

                case TokenType.Line:
                case TokenType.Key:
                    return new InputExpr(Advance().Type);

                default:
                    throw new Exception("Unexpected token: \'" + Peek().Type + "\' at line " + Peek().Line + ".");
            }
        }
        #endregion

        #region Parser operations
        private Token Peek()
        {
            return tokens[current];
        }

        private Token PeekNext()
        {
            return tokens[current + 1];
        }

        private bool Match(TokenType type)
        {
            return type == Peek().Type;
        }

        private Token Advance(TokenType type)
        {
            current++;
            if (tokens[current - 1].Type == type)
            {
                return tokens[current - 1];
            }
            else
            {
                throw new Exception("Unexpected token: \'" + tokens[current - 1].Type + "\' at line " + tokens[current - 1].Line + ".");
            }
        }

        private Token Advance()
        {
            current++;
            return tokens[current - 1];
        }

        private bool IsAtEnd()
        {
            return current >= tokens.Count || Peek().Type == TokenType.EOF;
        }
        #endregion
    }
}

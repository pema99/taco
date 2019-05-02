using System;
using System.Collections.Generic;
using System.Globalization;

namespace BadLang
{
    public class Lexer
    {
        private string source;
        private List<Token> tokens;
        private int start;
        private int current;
        private int line;
        private Dictionary<string, TokenType> keywords;

        public Lexer()
        {
            keywords = new Dictionary<string, TokenType>(); 
            keywords.Add("else", TokenType.Else);
            keywords.Add("false", TokenType.False);
            keywords.Add("for", TokenType.For);
            keywords.Add("func", TokenType.Func);
            keywords.Add("if", TokenType.If);
            keywords.Add("null", TokenType.Null);
            keywords.Add("print", TokenType.Print);
            keywords.Add("println", TokenType.PrintLine);
            keywords.Add("return", TokenType.Return);
            keywords.Add("true", TokenType.True);
            keywords.Add("var", TokenType.Var);
            keywords.Add("while", TokenType.While);
            keywords.Add("line", TokenType.Line);
            keywords.Add("key", TokenType.Key);
            keywords.Add("cls", TokenType.Clear);
        }

        public List<Token> ScanTokens(string source)
        {
            this.source = source;
            this.tokens = new List<Token>();
            this.start = 0;
            this.current = 0;
            this.line = 1;

            while (current < source.Length)
            {
                start = current;
                ScanToken();
            }
            tokens.Add(new Token(TokenType.EOF, null, null, line));

            return tokens;
        }

        private void ScanToken()
        {
            char c = Advance();
            switch (c)
            {
                //Tokens
                case '(': AddToken(TokenType.Left_Paren); break;
                case ')': AddToken(TokenType.Right_Paren); break;
                case '{': AddToken(TokenType.Left_Brace); break;
                case '}': AddToken(TokenType.Right_Brace); break;
                case '[': AddToken(TokenType.Left_Square); break;
                case ']': AddToken(TokenType.Right_Square); break;
                case ',': AddToken(TokenType.Comma); break;
                case '.': AddToken(TokenType.Dot); break;
                case '-': AddToken(Match('=') ? TokenType.Minus_Equal : Match('-') ? TokenType.Minus_Minus : TokenType.Minus); break;
                case '+': AddToken(Match('=') ? TokenType.Plus_Equal : Match('+') ? TokenType.Plus_Plus : TokenType.Plus); break;
                case '*': AddToken(Match('=') ? TokenType.Star_Equal : TokenType.Star); break;
                case '/': AddToken(Match('=') ? TokenType.Slash_Equal : TokenType.Slash); break;

                //Operators
                case '!': AddToken(Match('=') ? TokenType.Bang_Equal : TokenType.Bang); break;
                case '=': AddToken(Match('=') ? TokenType.Equal_Equal : TokenType.Equal); break;
                case '<': AddToken(Match('=') ? TokenType.Less_Equal : TokenType.Less); break;
                case '>': AddToken(Match('=') ? TokenType.Greater_Equal : TokenType.Greater); break;
                case '&': if (Match('&')) AddToken(TokenType.And); break;
                case '|': if (Match('|')) AddToken(TokenType.Or); break;
                case '^': if (Match('^')) AddToken(TokenType.Xor); break;

                //Literals
                case '"': String(); break;

                //Whitespace
                case ' ': break;
                case '\r': break;
                case '\t': break;
                case '\n': line++; break;

                //Comments
                case '\'': Comment(); break;

                default:
                    if (IsDigit(c))
                    {
                        Number();
                    }
                    else if (IsAlpha(c))
                    {
                        Identifier();
                    }
                    else
                    {
                        throw new Exception("Unexpected symbol \'" + c + "\' at line " + line);
                    }
                    break;
            }
        }

        private void Identifier()
        {
            while (IsAlphaNumeric(Peek()))
            {
                Advance();
            }

            string Text = source.Substring(start, current - start);
            if (keywords.ContainsKey(Text))
            {
                AddToken(keywords[Text]);
            }

            else
            {
                AddToken(TokenType.Identifier);
            }
        }

        private bool IsAlpha(char c)
        {
            return (c >= 'a' && c <= 'z') ||
                   (c >= 'A' && c <= 'Z') ||
                    c == '_';
        }

        private bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        private bool IsAlphaNumeric(char c)
        {
            return IsAlpha(c) || IsDigit(c);
        }

        private void Number()
        {
            while (IsDigit(Peek()))
            {
                Advance();
            }
            if (Peek() == '.')
            {
                Advance();
                while (IsDigit(Peek()))
                {
                    Advance();
                }
            }
            
            AddToken(TokenType.Number, float.Parse(source.Substring(start, current - start), CultureInfo.InvariantCulture));
        }

        private void String()
        {
            while (Peek() != '"' && current < source.Length)
            {
                if (Peek() == '\n')
                {
                    line++;
                }
                Advance();
            }
            if (current >= source.Length)
            {
                throw new Exception("Unterminated string at line " + line);
            }
            Advance();

            AddToken(TokenType.String, source.Substring(start+1, (current-1)-(start+1)));
        }

        private void Comment()
        {
            while (Peek() != '\n' && current < source.Length - 1)
            {
                Advance();
            }
        }

        private char Peek()
        {
            return source[current];
        }

        private bool Match(char expected)
        {
            if (Peek() != expected)
            {
                return false;
            }
            current++;
            return true;
        }

        private char Advance()
        {
            current++;
            return source[current - 1];
        }

        private void AddToken(TokenType type)
        {
            AddToken(type, null);
        }

        private void AddToken(TokenType type, object literal)
        {
            String text = source.Substring(start, current-start);
            tokens.Add(new Token(type, text, literal, line));
        }
    }
}

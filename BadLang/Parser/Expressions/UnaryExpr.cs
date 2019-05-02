using System;

namespace BadLang
{
    public class UnaryExpr : Expression
    {
        public Expression Expr { get; private set; }
        public TokenType Operation { get; private set; }

        public UnaryExpr(Expression expr, TokenType operation)
        {
            this.Expr = expr;
            this.Operation = operation;
        }
    }
}

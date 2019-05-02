using System;

namespace BadLang
{
    public class ReturnExpr : Expression
    {
        public Expression Expr { get; private set; }

        public ReturnExpr(Expression expr)
        {
            this.Expr = expr;
        }
    }
}

using System;

namespace TacoCompiler
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

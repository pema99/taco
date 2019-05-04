using System;

namespace TacoCompiler
{
    public class PrintExpr : Expression
    {
        public Expression Expr { get; private set; }
        public bool NewLine { get; private set; }

        public PrintExpr(Expression expr, bool newLine)
        {
            this.Expr = expr;
            this.NewLine = newLine;
        }
    }
}

using System;

namespace BadLang
{
    public class VariableAssigmentExpr : Expression
    {
        public string Name { get; private set; }
        public Expression Expr { get; private set; }

        public VariableAssigmentExpr(string name, Expression expr)
        {
            this.Name = name;
            this.Expr = expr;
        }
    }
}

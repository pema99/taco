using System;

namespace BadLang
{
    public class VariableDeclarationExpr : Expression
    {
        public string Name { get; private set; }
        public Expression Expr { get; private set; }

        public VariableDeclarationExpr(string name, Expression expr)
        {
            this.Name = name;
            this.Expr = expr;
        }
    }
}

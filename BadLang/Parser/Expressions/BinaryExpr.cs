using System;

namespace BadLang
{
    public class BinaryExpr : Expression
    {
        public Expression Left { get; private set; }
        public Expression Right { get; private set; }
        public TokenType Operation { get; private set; }

        public BinaryExpr(Expression left, Expression right, TokenType operation)
        {
            this.Left = left;
            this.Right = right;
            this.Operation = operation;
        }
    }
}

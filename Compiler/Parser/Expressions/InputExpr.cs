using System;

namespace TacoCompiler
{
    public class InputExpr : Expression
    {
        public TokenType Operation { get; private set; }

        public InputExpr(TokenType operation)
        {
            this.Operation = operation;
        }
    }
}

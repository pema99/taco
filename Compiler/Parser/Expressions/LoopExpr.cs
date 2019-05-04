using System;

namespace TacoCompiler
{
    public class LoopExpr : Expression
    {
        public Block Body { get; private set; }

        public LoopExpr(Block body)
        {
            this.Body = body;
        }
    }
}

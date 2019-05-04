using System;

namespace TacoCompiler
{
    public class BlockExpr : Expression
    {
        public Block Body { get; private set; }

        public BlockExpr(Block body)
        {
            this.Body = body;
        }
    }
}

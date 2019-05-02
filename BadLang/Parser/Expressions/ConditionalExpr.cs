using System;

namespace BadLang
{
    public class ConditionalExpr : Expression
    {
        public Expression Condition { get; private set; }
        public Block Yes { get; private set; }
        public Block No { get; private set; }

        public ConditionalExpr(Expression condition, Block yes, Block no)
        {
            this.Condition = condition;
            this.Yes = yes;
            this.No = no;
        }
    }
}

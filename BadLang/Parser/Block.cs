using System;
using System.Collections.Generic;

namespace BadLang
{
    public class Block
    {
        public List<Expression> Statements { get; set; }
        public Expression ReturnExpression { get; set; }

        public Block(List<Expression> statements, Expression returnExpression = null)
        {
            this.Statements = statements;
            this.ReturnExpression = returnExpression;
        }

        public Block()
            : this(new List<Expression>(), null)
        {
        }
    }
}

using System;
using System.Collections.Generic;

namespace TacoCompiler
{
    public class FunctionCallExpr : Expression
    {
        public string Name { get; private set; }
        public List<Expression> Parameters { get; private set; }

        public FunctionCallExpr(string name, List<Expression> parameters)
        {
            this.Name = name;
            this.Parameters = parameters;
        }
    }
}

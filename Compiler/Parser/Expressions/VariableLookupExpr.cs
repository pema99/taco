using System;

namespace TacoCompiler
{
    public class VariableLookupExpr : Expression
    {
        public string Name { get; private set; }

        public VariableLookupExpr(string name)
        {
            this.Name = name;
        }
    }
}

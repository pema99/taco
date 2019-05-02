using System;

namespace BadLang
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

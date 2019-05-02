using System;

namespace BadLang
{
    public class ConstantExpr : Expression
    {
        public object Data { get; private set; }

        public ConstantExpr(object data)
        {
            this.Data = data;
        }
    }
}

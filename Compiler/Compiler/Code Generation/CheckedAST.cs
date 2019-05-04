using System;
using System.Collections.Generic;
using System.Text;

namespace TacoCompiler
{
    public class CheckedAST
    {
        public Dictionary<string, FuncDeclaration> Functions { get; private set; }
        public Dictionary<string, Scope> Scopes { get; private set; }

        public CheckedAST(Dictionary<string, FuncDeclaration> functions, Dictionary<string, Scope> scopes)
        {
            this.Functions = functions;
            this.Scopes = scopes;
        }
    }
}

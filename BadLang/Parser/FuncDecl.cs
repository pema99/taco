using System;
using System.Collections;
using System.Collections.Generic;

namespace BadLang
{
    public class FuncDecl
    {
        public string Name { get; private set; }
        public Block Body { get; private set; }
        public string[] Parameters { get; private set; }
        
        public FuncDecl(string name, Block body, string[] parameters)
        {
            this.Name = name;
            this.Body = body;
            this.Parameters = parameters;
        }
    }
}

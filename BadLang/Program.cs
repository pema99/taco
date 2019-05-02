using System;
using System.IO;
using System.Runtime.InteropServices;

namespace BadLang
{
    public class Program
    {
        public static void Main()
        {
            Lexer l = new Lexer();
            Parser p = new Parser();
            var o = p.Parse(l.ScanTokens(File.ReadAllText("example.bl")));
            Binary b = new Compiler().Compile(o);
            new VM().Execute(b);
            Console.ReadKey();
        }
    }
}

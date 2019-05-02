using System;
using System.Diagnostics;
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
            Stopwatch s = Stopwatch.StartNew();
            new VM().Execute(b);
            s.Stop();
            Console.WriteLine();
            Console.WriteLine("Execution took " + s.ElapsedMilliseconds + "ms");
            Console.ReadKey();
        }
    }
}

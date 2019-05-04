using System;
using System.IO;

namespace TacoCompiler
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: TacoCompiler <source file path> <output file path>");
            }
            else
            {
                Lexer lexer = new Lexer();
                Parser parser = new Parser();
                Compiler compiler = new Compiler();
                Binary b = compiler.Compile(parser.Parse(lexer.ScanTokens(File.ReadAllText(args[0]))));

                using (FileStream fs = new FileStream(args[1], FileMode.OpenOrCreate))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        bw.Write(b.EntryPoint);
                        bw.Write(b.Heap);
                        foreach (var instr in b.Instructions)
                        {
                            bw.Write(instr.Bits);
                        }
                    }
                }
            }
        }
    }
}

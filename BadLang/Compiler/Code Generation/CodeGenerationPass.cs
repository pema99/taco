using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BadLang
{
    public class CodeGenerationPass
    {
        private List<Variant> instructions;
        private Dictionary<string, int> symbols;
        private Dictionary<string, int> identifiers;
        private byte[] heap;
        private int heapPtr;

        public UnlinkedAST Process(CheckedAST ast)
        {
            instructions = new List<Variant>();
            symbols = new Dictionary<string, int>();
            identifiers = ast.Identifiers;
            heap = new byte[1000];
            heapPtr = 0;

            foreach (FuncDecl func in ast.Functions.Values)
            {
                symbols.Add(func.Name, instructions.Count);;

                for (int i = 0; i < func.Parameters.Length; i++)
                {
                    AddInstruction(Instruction.VAR_ASSIGN, new Variant(i));
                }

                foreach (Expression statement in func.Body.Statements)
                {
                    CompileExpression(statement);
                }
            }

            return new UnlinkedAST(instructions.ToArray(), symbols, identifiers, heap);
        }

        private void CompileExpression(Expression expression)
        {
            switch (expression)
            {
                //Statements
                case ReturnExpr e:
                    CompileExpression(e.Expr);
                    AddInstruction(Instruction.RETURN);
                    break;

                case PrintExpr e:
                    CompileExpression(e.Expr);
                    AddInstruction(e.NewLine ? Instruction.PRINTLN : Instruction.PRINT);
                    break;

                case VariableDeclarationExpr e:
                    CompileExpression(e.Expr);
                    AddInstruction(Instruction.VAR_ASSIGN, new Variant(identifiers[e.Name]));
                    break;

                case VariableAssigmentExpr e:
                    CompileExpression(e.Expr);
                    AddInstruction(Instruction.VAR_ASSIGN, new Variant(identifiers[e.Name]));
                    break;

                //Expressions
                case VariableLookupExpr e:
                    AddInstruction(Instruction.VAR_LOOKUP, new Variant(identifiers[e.Name]));
                    break;

                case BinaryExpr e:
                    CompileExpression(e.Left);
                    CompileExpression(e.Right);

                    switch (e.Operation)
                    {
                        case TokenType.Plus_Equal:
                        case TokenType.Plus_Plus:
                        case TokenType.Plus: AddInstruction(Instruction.ADD); break;

                        case TokenType.Minus_Equal:
                        case TokenType.Minus_Minus:
                        case TokenType.Minus: AddInstruction(Instruction.SUB); break;

                        case TokenType.Star_Equal:
                        case TokenType.Star: AddInstruction(Instruction.MUL); break;

                        case TokenType.Slash_Equal:
                        case TokenType.Slash: AddInstruction(Instruction.DIV); break;

                        default:
                            throw new Exception("Unimplemented");
                            break;
                    }
                    break;

                case ConstantExpr e:
                    switch (e.Data)
                    {
                        case float f: AddInstruction(Instruction.CONST, new Variant(f)); break;
                        case bool b: AddInstruction(Instruction.CONST, new Variant(b)); break;
                        case string s:
                            AddInstruction(Instruction.CONST, new Variant(heapPtr));

                            //Insert into hype
                            byte[] bytes = Encoding.ASCII.GetBytes(s);
                            foreach (byte b in BitConverter.GetBytes(bytes.Length))
                            {
                                heap[heapPtr++] = b;
                            }
                            foreach (byte b in bytes)
                            {
                                heap[heapPtr++] = b;
                            }               
                            break;
                    }
                    break;
            }
        }

        private void CompileBlock(Block block)
        {

        }

        private void AddInstruction(Instruction instr)
        {
            instructions.Add((long)instr);
        }

        private void AddInstruction(Instruction instr, Variant operand)
        {
            instructions.Add((long)instr);
            instructions.Add(operand);
        }
    }
}

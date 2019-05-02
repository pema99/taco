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
        private Dictionary<string, Scope> scopes;
        private List<(string, int)> unlinked;
        private byte[] heap;
        private int heapPtr;
        private int tempLabelNum;

        public UnlinkedAST Process(CheckedAST ast)
        {
            instructions = new List<Variant>();
            symbols = new Dictionary<string, int>();
            scopes = ast.Scopes;
            unlinked = new List<(string, int)>();
            heap = new byte[1000];
            heapPtr = 0;
            tempLabelNum = 0;

            foreach (FuncDeclaration func in ast.Functions.Values)
            {
                symbols.Add(func.Name, instructions.Count);;

                AddInstruction(Instruction.PUSH_PARAMS, new Variant(func.Parameters.Length));

                foreach (Expression statement in func.Body.Statements)
                {
                    CompileExpression(statement, scopes[func.Name]);
                }
            }

            return new UnlinkedAST(instructions.ToArray(), symbols, unlinked, heap);
        }

        private void CompileExpression(Expression expression, Scope scope)
        {
            switch (expression)
            {
                case ReturnExpr e:
                    CompileExpression(e.Expr, scope);
                    AddInstruction(Instruction.RETURN);
                    break;

                case PrintExpr e:
                    CompileExpression(e.Expr, scope);
                    AddInstruction(e.NewLine ? Instruction.PRINTLN : Instruction.PRINT);
                    break;

                case VariableDeclarationExpr e:
                    CompileExpression(e.Expr, scope);
                    AddInstruction(Instruction.VAR_ASSIGN, new Variant(scope.Symbols[e.Name]));
                    break;

                case VariableAssigmentExpr e:
                    CompileExpression(e.Expr, scope);
                    AddInstruction(Instruction.VAR_ASSIGN, new Variant(scope.Symbols[e.Name]));
                    break;

                case VariableLookupExpr e:
                    AddInstruction(Instruction.VAR_LOOKUP, new Variant(scope.Symbols[e.Name]));
                    break;

                case ClearConsoleExpr e:
                    AddInstruction(Instruction.CLS);
                    break;

                case BinaryExpr e:
                    CompileExpression(e.Left, scope);
                    CompileExpression(e.Right, scope);

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

                        case TokenType.Less_Equal:
                        case TokenType.Less: AddInstruction(Instruction.LESS); break;

                        case TokenType.Greater_Equal:
                        case TokenType.Greater: AddInstruction(Instruction.GREATER); break;
                            break;

                        case TokenType.Equal_Equal:
                        case TokenType.Equal: AddInstruction(Instruction.EQUAL); break;

                        default:
                            throw new Exception("Unimplemented binary operation");
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

                case FunctionCallExpr e:
                    foreach (Expression param in e.Parameters)
                    {
                        CompileExpression(param, scope);
                    }
                    AddInstruction(Instruction.CALL, 0);
                    unlinked.Add((e.Name, instructions.Count - 2));
                    break;

                case ConditionalExpr e:
                    CompileExpression(e.Condition, scope);

                    AddInstruction(Instruction.JMP_COND, 0);

                    string label = "_" + tempLabelNum++;

                    unlinked.Add((label, instructions.Count - 2));

                    CompileBlock(e.Yes, scope);

                    symbols.Add(label, instructions.Count);

                    CompileBlock(e.No, scope);
                    break;
            }
        }

        private void CompileBlock(Block block, Scope scope)
        {
            foreach (Expression expr in block.Statements)
            {
                CompileExpression(expr, scope);
            }
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BadLang
{
    public class SemanticPass
    {
        private Scope currentScope;
        private Dictionary<string, int> identifiers;

        public CheckedAST Process(RawAST ast)
        {
            identifiers = new Dictionary<string, int>();
            foreach (FuncDecl func in ast.Functions.Values)
            {
                CheckDeclaration(func);
            }
            return new CheckedAST(ast.Functions, identifiers);
        }

        private void CheckDeclaration(FuncDecl func)
        {
            currentScope = new Scope(null, new Dictionary<string, int>(), 0);
            foreach (string param in func.Parameters)
            {
                currentScope.AddSymbol(param);
            }

            foreach (Expression expr in func.Body.Statements)
            {
                CheckExpression(expr);
            }

            currentScope.Symbols.ToList().ForEach(x => identifiers.Add(x.Key, x.Value));
        }

        private void CheckExpression(Expression expr)
        {
            switch (expr)
            {
                //Statements
                case VariableDeclarationExpr e:
                    currentScope.AddSymbol(e.Name);
                    break;

                case VariableAssigmentExpr e: //TODO CHECK IF VARIABLE EXISTS
                    CheckExpression(e.Expr);
                    break;

                case ReturnExpr e:
                    CheckExpression(e.Expr);
                    break;

                case PrintExpr e:
                    CheckExpression(e.Expr);
                    break;

                //Expression
                case BinaryExpr e:
                    CheckExpression(e.Left);
                    CheckExpression(e.Right); //TODO: Typecheck?
                    break;

                case ConstantExpr e:
                    break;

                case FunctionCallExpr e:
                    if (currentScope.GetSymbol(e.Name) != -1)
                    {
                        foreach (Expression param in e.Parameters)
                        {
                            CheckExpression(param);
                        }
                    }
                    else
                    {
                        throw new Exception("Couldn't find function");
                    }
                    break;

                case VariableLookupExpr e: //TODO Check if variable exists
                    break;

                case BlockExpr e:
                    CheckBlock(e.Body);
                    //TODO: Return?
                    break;

                case ConditionalExpr e:
                    CheckExpression(e.Condition);
                    CheckBlock(e.Yes);
                    CheckBlock(e.No);
                    break;

                default:
                    throw new Exception("Unimplemented: " + expr);
            }
        }

        private void CheckBlock(Block block)
        {
            foreach (Expression stmt in block.Statements)
            {
                CheckExpression(stmt);
            }
        }
    }
}

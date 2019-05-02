using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BadLang
{
    public class SemanticPass
    {
        private Scope currentScope;

        public CheckedAST Process(RawAST ast)
        {
            Dictionary<string, Scope> scopes = new Dictionary<string, Scope>();   
            foreach (FuncDeclaration func in ast.Functions.Values)
            {
                CheckDeclaration(func);
                scopes.Add(func.Name, currentScope);
            }
            return new CheckedAST(ast.Functions, scopes);
        }

        private void CheckDeclaration(FuncDeclaration func)
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
        }

        private void CheckExpression(Expression expr)
        {
            switch (expr)
            {
                case VariableDeclarationExpr e:
                    if (currentScope.GetSymbol(e.Name) != -1)
                    {
                        throw new Exception(string.Format("Compiler error: Variable '{0}' already exists." + e.Name));
                    }
                    currentScope.AddSymbol(e.Name);
                    break;

                case VariableAssigmentExpr e: 
                    if (currentScope.GetSymbol(e.Name) == -1)
                    {
                        throw new Exception(string.Format("Compiler error: Variable '{0}' does not exist." + e.Name));
                    }
                    CheckExpression(e.Expr);
                    break;

                case VariableLookupExpr e:
                    if (currentScope.GetSymbol(e.Name) == -1)
                    {
                        throw new Exception(string.Format("Compiler error: Variable '{0}' does not exist.", e.Name));
                    }
                    break;

                case ReturnExpr e:
                    CheckExpression(e.Expr);
                    break;

                case PrintExpr e:
                    CheckExpression(e.Expr);
                    break;

                case BinaryExpr e:
                    CheckExpression(e.Left);
                    CheckExpression(e.Right);
                    break;

                case FunctionCallExpr e:
                    foreach (Expression param in e.Parameters)
                    {
                        CheckExpression(param);
                    }
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

                case UnaryExpr e:
                    CheckExpression(e.Expr);
                    break;

                case LoopExpr e:
                    CheckBlock(e.Body);
                    break;

                default:
                    break;
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

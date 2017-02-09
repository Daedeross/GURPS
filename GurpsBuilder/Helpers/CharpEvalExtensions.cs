using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GurpsBuilder.DataModels;

namespace ExpressionEvaluator.Extensions
{
    public static class CompiledExpresionExtensions
    {
        public static ParameterExpression GetScope(this Expression e)
        {
            if (e is ParameterExpression)
            {
                var pe = e as ParameterExpression;
                if (pe.Name == "scope")
                {
                    return e as ParameterExpression;
                }
            }
            else if (e is BinaryExpression)
            {
                var be = e as BinaryExpression;
                var p = GetScope(be.Left);
                if (p != null)
                {
                    return p;
                }
                else return GetScope(be.Right);

            }
            else if (e is UnaryExpression)
            {
                var ue = e as UnaryExpression;
                return GetScope(ue.Operand);
            }
            else if (e is ConditionalExpression)
            {
                var ce = e as ConditionalExpression;
                var p = GetScope(ce.Test);
                if (p != null)
                {
                    return p;
                }
                p = GetScope(ce.IfTrue);
                if (p != null)
                {
                    return p;
                }
                return GetScope(ce.IfFalse);
            }
            else if (e is MemberExpression)
            {
                var me = e as MemberExpression;
                return GetScope(me.Expression);
            }
            else if (e is DynamicExpression)
            {
                var de = e as DynamicExpression;
                ParameterExpression p;
                foreach (Expression expr in de.Arguments)
                {
                    p = GetScope(expr);
                    if (p != null)
                    {
                        return p;
                    }
                }
            }

            return null;
        }

        public static ParameterExpression GetScope(this CompiledExpression ce)
        {
            return ce.Expression.GetScope();
        }

        public static HashSet<INotifyValueChanged> GetDependencies<TScope>(this Expression e, TScope scope, HashSet<INotifyValueChanged> dependencies)
        {
            if (dependencies == null)
            {
                dependencies = new HashSet<INotifyValueChanged>();
            }
            
            if (e is BinaryExpression)
            {
                var be = e as BinaryExpression;
                dependencies = GetDependencies(be.Left, scope, dependencies);
                dependencies = GetDependencies(be.Right, scope, dependencies);

            }
            else if (e is UnaryExpression)
            {
                var ue = e as UnaryExpression;
                dependencies = GetDependencies<TScope>(ue.Operand, scope, dependencies);
            }
            else if (e is ConditionalExpression)
            {
                var ce = e as ConditionalExpression;
                dependencies = GetDependencies(ce.Test, scope, dependencies);
                dependencies = GetDependencies(ce.IfTrue, scope, dependencies);
            }
            else if (e is MemberExpression)
            {
                var scopeParam = e.GetScope();
                var context = Expression.Lambda<Func<TScope, object>>(e, scopeParam).Compile()(scope);
                if (context is INotifyValueChanged)
                {
                    dependencies.Add(context as INotifyValueChanged);
                }
            }
            else if (e is DynamicExpression)
            {
                var de = e as DynamicExpression;

                System.Dynamic.GetMemberBinder binder = (de.Binder as System.Dynamic.GetMemberBinder);

                if (binder != null)
                {
                    var scopeParam = e.GetScope();
                    var context = Expression.Lambda<Func<TScope, object>>(de.Arguments[0], scopeParam).Compile()(scope);
                    if (context is INotifyValueChanged)
                    {
                        dependencies.Add(context as INotifyValueChanged);
                    }
                }
                else
                {
                    foreach (Expression expr in de.Arguments)
                    {
                        dependencies = GetDependencies(expr, scope, dependencies);
                    }
                }
            }

            return dependencies;
        }

        public static HashSet<INotifyValueChanged> GetDependencies<TScope, TResult>(this CompiledExpression<TResult> ce, TScope scope)
        {
            return ce.Expression.GetDependencies(scope, null);
        }
    }
}

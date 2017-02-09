using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ExpressionEvaluator;
using ExpressionEvaluator.Extensions;
using GurpsBuilder.DataModels;
using System.Linq.Expressions;
using System.Collections.ObjectModel;

using Expression = System.Linq.Expressions.Expression;
using Microsoft.CSharp.RuntimeBinder;

namespace GurpsBuilder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        dynamic c;

        public ObservableCollection<string> propNames { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            c = new Character();
            c.Age = new BaseTrait();
            c.Age.score = 10;
            c.Height = new BaseTrait();
            c.Height.score = 3;
            dynamic st = new BaseTrait();
            st.Attatch(c);
            ValueTag<double> score = new ValueTag<double>(st);

            score.Name = "score";
            
            //st.Tags.Add("score", score);
            st.score = score;
            //st.Test1 = 11;
            score.Text = "13";
            dynamic attr = c.Attributes;
            attr.ST = st;
            propNames = new ObservableCollection<string>(); 
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string exprString = exprText.Text;
            CompiledExpression<double> ce = new CompiledExpression<double>(exprString);
            Func<Character, double> del;

            try
            {
                del = ce.ScopeCompile<Character>();
            }
            catch (ExpressionEvaluator.Parser.ExpressionParseException)
            {
                throw;
            }

            double result;

            try
            {
                result = del(c);
                statusText.Text = "OK";
            }
            catch (Exception ex)
            {
                statusText.Text = ex.Message;
                result = -1;
            }
            propNames = new ObservableCollection<string>(getPropNames(ce.Expression, null));
            propList.ItemsSource = propNames;
            exprResult.Text = result.ToString();

            //var props = getProps(ce.Expression, null);
            var parameter = getParameter(ce.Expression);
            var members = getMembers(ce.Expression, null);
            var dict = ce.GetDependencies(c as Character);
            foreach (var m in members)
            {
                var lamb = Expression.Lambda<Func<Character, object>>(m.ObjectExpression, parameter);
                var d = lamb.Compile();
                var x = d(c as Character);
            }
        }

        private List<string> getPropNames(Expression e, List<string> props)
        {
            if (props == null)
            {
                props = new List<string>();
            }
            if (e is BinaryExpression)
            {
                var be = e as BinaryExpression;
                props = getPropNames(be.Left, props);
                props = getPropNames(be.Right, props);
                
            }
            else if (e is UnaryExpression)
            {
                var ue = e as UnaryExpression;
                props = getPropNames(ue.Operand, props);
            }
            else if (e is ConditionalExpression)
            {
                
            }
            else if (e is MemberExpression)
            {
                var me = e as MemberExpression;
                props.Add(me.Member.Name);
            }
            else if (e is DynamicExpression)
            {
                var de = e as DynamicExpression;

                System.Dynamic.GetMemberBinder binder = (de.Binder as System.Dynamic.GetMemberBinder);

                if (binder != null)
                {
                    //de.Arguments;
                    props.Add(binder.Name);
                    //var x = binder.call;
                }
                //else
                //{
                    foreach (Expression expr in de.Arguments)
                    {
                        props = getPropNames(expr, props);
                    }
                //}
            }
            return props;
        }

        private List<Expression> getProps(Expression e, List<Expression> props)
        {
            if (props == null)
            {
                props = new List<Expression>();
            }


            if (e is BinaryExpression)
            {
                var be = e as BinaryExpression;
                props = getProps(be.Left, props);
                props = getProps(be.Right, props);

            }
            else if (e is UnaryExpression)
            {
                var ue = e as UnaryExpression;
                props = getProps(ue.Operand, props);
            }
            else if (e is ConditionalExpression)
            {

            }
            else if (e is MemberExpression)
            {
                //var me = e as MemberExpression;
                props.Add(e);
            }
            else if (e is DynamicExpression)
            {
                var de = e as DynamicExpression;

                System.Dynamic.GetMemberBinder binder = (de.Binder as System.Dynamic.GetMemberBinder);

                if (binder != null)
                {
                    //de.Arguments;
                    props.Add(de);
                    //var x = binder.call;
                }
                else
                {
                    foreach (Expression expr in de.Arguments)
                    {
                        props = getProps(expr, props);
                    }
                }
            }

            return props;
        }

        private ParameterExpression getParameter(Expression e)
        {
            if (e is ParameterExpression)
            {
                return e as ParameterExpression;
            }
            else if (e is BinaryExpression)
            {
                var be = e as BinaryExpression;
                var p = getParameter(be.Left);
                if (p != null)
                {
                    return p;
                }
                else return getParameter(be.Right);

            }
            else if (e is UnaryExpression)
            {
                var ue = e as UnaryExpression;
                return getParameter(ue.Operand);
            }
            else if (e is ConditionalExpression)
            {
                var ce = e as ConditionalExpression;
                var p = getParameter(ce.Test);
                if (p != null)
                {
                    return p;
                }
                p = getParameter(ce.IfTrue);
                if (p != null)
                {
                    return p;
                }
                return getParameter(ce.IfFalse);
            }
            else if (e is MemberExpression)
            {
                var me = e as MemberExpression;
                return getParameter(me.Expression);
            }
            else if (e is DynamicExpression)
            {
                var de = e as DynamicExpression;
                ParameterExpression p;
                foreach (Expression expr in de.Arguments)
                {
                    p = getParameter(expr);
                    if (p != null)
                    {
                        return p;
                    }
                }
            }

            return null;
        }

        private class MemberName
        {
            public Expression ObjectExpression { get; set; }
            public string PropertyName { get; set; }
        }
        
        private List<MemberName> getMembers(Expression e, List<MemberName> mes)
        {
            if (mes == null)
            {
                mes = new List<MemberName>();
            }


            if (e is BinaryExpression)
            {
                var be = e as BinaryExpression;
                mes = getMembers(be.Left, mes);
                mes = getMembers(be.Right, mes);

            }
            else if (e is UnaryExpression)
            {
                var ue = e as UnaryExpression;
                mes = getMembers(ue.Operand, mes);
            }
            else if (e is ConditionalExpression)
            {

            }
            else if (e is MemberExpression)
            {
                var me = e as MemberExpression;
                mes.Add(new MemberName { ObjectExpression = me.Expression, PropertyName = me.Member.Name });
            }
            else if (e is DynamicExpression)
            {
                var de = e as DynamicExpression;

                System.Dynamic.GetMemberBinder binder = (de.Binder as System.Dynamic.GetMemberBinder);

                if (binder != null)
                {
                    //de.Arguments;
                    mes.Add(new MemberName { ObjectExpression = de.Arguments[0], PropertyName = binder.Name });
                    //var x = binder.call;
                }
                else
                {
                    foreach (Expression expr in de.Arguments)
                    {
                        mes = getMembers(expr, mes);
                    }
                }
            }

            return mes;
        }
    }
}

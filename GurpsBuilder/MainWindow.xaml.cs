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
        Character c;

        public ObservableCollection<string> propNames { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            c = new Character();
            c.Age = 10;
            c.Height = 3;
            dynamic st = new BaseTrait();
            st.Attatch(c);
            ValueTag<int> score = new ValueTag<int>(st);

            score.Name = "score";
            
            //st.Tags.Add("score", score);
            st.score = score;
            st.Test1 = 11;
            score.Text = "13";
            c.Attributes["ST"] = st;
            propNames = new ObservableCollection<string>(); 
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string exprString = exprText.Text;
            CompiledExpression<int> ce = new CompiledExpression<int>(exprString);

            Func<Character, int> del = ce.ScopeCompile<Character>();

            int result = 0;

            try
            {
                result = del(c);
            }
            catch
            {
                result = -1;
            }
            propNames = new ObservableCollection<string>(getPropNames(ce.Expression, null));
            propList.ItemsSource = propNames;
            exprResult.Text = result.ToString();
        }

        private List<string> getPropNames(System.Linq.Expressions.Expression e, List<string> props)
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
                    props.Add(binder.Name);
                }
                else
                {
                    foreach (Expression expr in de.Arguments)
                    {
                        props = getPropNames(expr, props);
                    }
                }
                //de.Arguments
            }
            return props;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpressionEvaluator;
using System.Dynamic;
using System.Linq.Expressions;

namespace GurpsBuilder.DataModels
{
    public class ValueTag<T>: DynamicDataModel, IValueTag<T>
    {
        #region Fields

        protected T mDefaultValue;
        protected T mBonusValue;
        protected string _exprText;
        protected CompiledExpression<T> _exprCompiled;
        protected Func<Context, T> _exprDelegate;
        protected Func<ValueTag<T>, T> _finalValueDelegate;
        protected Context context;

        #endregion // Fields

        #region Properties

        public T Value
        {
            get { return _exprDelegate(context); }
        }

        public T DefaultValue
        {
            get { return mDefaultValue; }
            set { mDefaultValue = value; }
        }

        public T BonusValue
        {
            get { return mBonusValue;  }
            set { mBonusValue = value; }
        }

        public T FinalValue
        {
            get { return Value; }
            //get { return _finalValueDelegate(this); }
        }

        protected bool mIsOverride = false;
        public bool IsOverride
        {
            get { return mIsOverride; }
            set
            {
                if (mIsOverride != value)
                {
                    mIsOverride = value;
                    OnPropertyChanged("IsOverride");
                }
            }
        }

        public T OverrideValue
        {
            get
            {
                return default(T);
            }
            set
            {
                
            }
        }

        public event EventHandler ValueChanged;

        public string Text
        {
            get
            {
                return _exprText;
            }
            set
            {
                if (value != _exprText)
                {
                    CompileExpression(value);
                    //System.Diagnostics.Debug.WriteLine("Compile!");
                }
            }
        }

        public bool ReadOnly
        {
            get;
            set;
        }

        public Type GetValueType()
        {
            return typeof(T);
        }

        protected ITaggable mOwner;
        public ITaggable Owner
        {
            get { return mOwner; }
        }

        #endregion //Properties

        #region Constructors

        public ValueTag()
        {
            Initialize();
        }

        public ValueTag(ITaggable owner)
        {
            Initialize();
            Attatch(owner);
        }

        private void Initialize()
        {
            //if (typeof(T) == typeof(string))
            //{
            //    mDefaultValue = (T)((object)"");
            //    mBonusValue = (T)((object)"");
            //}
            mDefaultValue = default(T);
            mBonusValue = default(T);
            //if (_finalValueDelegate == null)
            //{
                string expr = "IsOverride ? OverrideValue : (BonusValue == null ? Value : Value + BonusValue)";
                CompiledExpression<T> ce = new CompiledExpression<T>(expr);
                _finalValueDelegate = ce.ScopeCompile<ValueTag<T>>();
            //}
        }

        #endregion // Constructors

        #region Commands

        #endregion // Commands

        #region Private Methods

        private void CompileExpression(string expr)
        {
            //try
            //{
                _exprText = expr;
                _exprCompiled = new CompiledExpression<T>(_exprText);
                _exprDelegate = _exprCompiled.ScopeCompile<Context>();

            //}
            //catch
            //{
            //    _exprCompiled = null;
            //    _exprDelegate = (c => this.mDefaultValue);
            //}
        }

        private void ResetContext(ITaggable owner)
        {
            context = Context.Generate(owner);
        }

        #endregion // Private Methods

        #region Public Methods

        public void Attatch(ITaggable owner)
        {
            mOwner = owner;
            ResetContext(mOwner);
        }

            #region DynamicObject Overloads

        public override bool TryBinaryOperation(BinaryOperationBinder binder, object arg, out object result)
        {
            switch (binder.Operation)
            {
                case ExpressionType.Add:
                    
                    break;
                case ExpressionType.AddAssign:
                    break;
                case ExpressionType.AddAssignChecked:
                    break;
                case ExpressionType.AddChecked:
                    break;
                case ExpressionType.And:
                    break;
                case ExpressionType.AndAlso:
                    break;
                case ExpressionType.AndAssign:
                    break;
                case ExpressionType.ArrayIndex:
                    break;
                case ExpressionType.ArrayLength:
                    break;
                case ExpressionType.Assign:
                    break;
                case ExpressionType.Block:
                    break;
                case ExpressionType.Call:
                    break;
                case ExpressionType.Coalesce:
                    break;
                case ExpressionType.Conditional:
                    break;
                case ExpressionType.Constant:
                    break;
                case ExpressionType.Convert:
                    break;
                case ExpressionType.ConvertChecked:
                    break;
                case ExpressionType.DebugInfo:
                    break;
                case ExpressionType.Decrement:
                    break;
                case ExpressionType.Default:
                    break;
                case ExpressionType.Divide:
                    break;
                case ExpressionType.DivideAssign:
                    break;
                case ExpressionType.Dynamic:
                    break;
                case ExpressionType.Equal:
                    break;
                case ExpressionType.ExclusiveOr:
                    break;
                case ExpressionType.ExclusiveOrAssign:
                    break;
                case ExpressionType.Extension:
                    break;
                case ExpressionType.Goto:
                    break;
                case ExpressionType.GreaterThan:
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    break;
                case ExpressionType.Increment:
                    break;
                case ExpressionType.Index:
                    break;
                case ExpressionType.Invoke:
                    break;
                case ExpressionType.IsFalse:
                    break;
                case ExpressionType.IsTrue:
                    break;
                case ExpressionType.Label:
                    break;
                case ExpressionType.Lambda:
                    break;
                case ExpressionType.LeftShift:
                    break;
                case ExpressionType.LeftShiftAssign:
                    break;
                case ExpressionType.LessThan:
                    break;
                case ExpressionType.LessThanOrEqual:
                    break;
                case ExpressionType.ListInit:
                    break;
                case ExpressionType.Loop:
                    break;
                case ExpressionType.MemberAccess:
                    break;
                case ExpressionType.MemberInit:
                    break;
                case ExpressionType.Modulo:
                    break;
                case ExpressionType.ModuloAssign:
                    break;
                case ExpressionType.Multiply:
                    break;
                case ExpressionType.MultiplyAssign:
                    break;
                case ExpressionType.MultiplyAssignChecked:
                    break;
                case ExpressionType.MultiplyChecked:
                    break;
                case ExpressionType.Negate:
                    break;
                case ExpressionType.NegateChecked:
                    break;
                case ExpressionType.New:
                    break;
                case ExpressionType.NewArrayBounds:
                    break;
                case ExpressionType.NewArrayInit:
                    break;
                case ExpressionType.Not:
                    break;
                case ExpressionType.NotEqual:
                    break;
                case ExpressionType.OnesComplement:
                    break;
                case ExpressionType.Or:
                    break;
                case ExpressionType.OrAssign:
                    break;
                case ExpressionType.OrElse:
                    break;
                case ExpressionType.Parameter:
                    break;
                case ExpressionType.PostDecrementAssign:
                    break;
                case ExpressionType.PostIncrementAssign:
                    break;
                case ExpressionType.Power:
                    break;
                case ExpressionType.PowerAssign:
                    break;
                case ExpressionType.PreDecrementAssign:
                    break;
                case ExpressionType.PreIncrementAssign:
                    break;
                case ExpressionType.Quote:
                    break;
                case ExpressionType.RightShift:
                    break;
                case ExpressionType.RightShiftAssign:
                    break;
                case ExpressionType.RuntimeVariables:
                    break;
                case ExpressionType.Subtract:
                    break;
                case ExpressionType.SubtractAssign:
                    break;
                case ExpressionType.SubtractAssignChecked:
                    break;
                case ExpressionType.SubtractChecked:
                    break;
                case ExpressionType.Switch:
                    break;
                case ExpressionType.Throw:
                    break;
                case ExpressionType.Try:
                    break;
                case ExpressionType.TypeAs:
                    break;
                case ExpressionType.TypeEqual:
                    break;
                case ExpressionType.TypeIs:
                    break;
                case ExpressionType.UnaryPlus:
                    break;
                case ExpressionType.Unbox:
                    break;
                default:
                    break;
            }
            return base.TryBinaryOperation(binder, arg, out result);
        }

            #endregion // DynamicObject Overloads

        #endregion // Public Methods

    }
}

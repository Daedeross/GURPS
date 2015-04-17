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
        protected dynamic context;
        protected Type myValueType = typeof(T);

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

        private bool IsNumeric(Type t)
        {
            switch (Type.GetTypeCode(t))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }

        #endregion // Private Methods

        #region Public Methods

        public void Attatch(ITaggable owner)
        {
            mOwner = owner;
            ResetContext(mOwner);
        }

            #region DynamicObject Overloads

        public override bool TryUnaryOperation(UnaryOperationBinder binder, out object result)
        {
            if (IsNumeric(myValueType))
            {
                switch (binder.Operation)
                {
                    case ExpressionType.Decrement:
                        result = (dynamic)this.FinalValue - 1;
                        return true;
                    case ExpressionType.Increment:
                        result = (dynamic)this.FinalValue + 1;
                        return true;
                    case ExpressionType.Negate:
                        result = -(dynamic)(this.FinalValue);
                        return true;
                    case ExpressionType.OnesComplement:
                        if (myValueType == typeof(int) || myValueType == typeof(uint))
                        {
                            result = ~((int)(object)this.FinalValue);
                            return true;
                        }
                        result = this;
                        return false;
                    default:
                        result = this;
                        return false;
                }
            }
            else if (myValueType == typeof(string))
            {
                switch (binder.Operation)
                {
                    case ExpressionType.IsFalse:
                        result = String.Compare(this.FinalValue.ToString(), "false", true) == 0;
                        return true;
                    case ExpressionType.IsTrue:
                        result = String.Compare(this.FinalValue.ToString(), "true", true) == 0;
                        return true;
                    default:
                        result = this;
                        return false;
                }

            }
            result = this;
            return false;
        }

        public override bool TryBinaryOperation(BinaryOperationBinder binder, object arg, out object result)
        {
            // if we hold a string, then convert arg to string and do opp
            if(myValueType == typeof(string))
            {
                string argStr = arg.ToString();

                switch (binder.Operation)
                {
                    case ExpressionType.Add:
                        result = this.FinalValue + argStr;
                        return true;
                    case ExpressionType.AddAssign:
                        result = this.FinalValue + argStr;
                        return true;
                    case ExpressionType.Equal:
                        result = this.FinalValue.ToString() == argStr;
                        return true;
                    case ExpressionType.NotEqual:
                        result = this.FinalValue.ToString() != argStr;
                        return true;
                    default:
                        result = this.FinalValue;
                        return false;
                }
            }
            try
            {
                if (IsNumeric(binder.ReturnType))
                {
                    switch (binder.Operation)
                    {
                        case ExpressionType.Add:
                            result = (dynamic)this.FinalValue + arg;
                            return true;
                        case ExpressionType.AddAssign:
                            result = (dynamic)this.FinalValue + arg;
                            return true;
                        case ExpressionType.Divide:
                            result = (dynamic)this.FinalValue / arg;
                            return true;
                        case ExpressionType.DivideAssign:
                            result = (dynamic)this.FinalValue / arg;
                            return true;
                        case ExpressionType.GreaterThan:
                            result = (dynamic)this.FinalValue > arg;
                            return true;
                        case ExpressionType.GreaterThanOrEqual:
                            result = (dynamic)this.FinalValue >= arg;
                            return true;
                        case ExpressionType.LeftShift:
                            if (myValueType == typeof(int))
                            {
                                result = (dynamic)FinalValue << arg;
                                return true;
                            }
                            break;
                        case ExpressionType.LeftShiftAssign:
                            if (myValueType == typeof(int))
                            {
                                result = (dynamic)FinalValue << arg;
                                return true;
                            }
                            break;
                        case ExpressionType.LessThan:
                            result = (dynamic)this.FinalValue < arg;
                            return true;
                        case ExpressionType.LessThanOrEqual:
                            result = (dynamic)this.FinalValue <= arg;
                            break;
                        case ExpressionType.Modulo:
                            if (myValueType == typeof(int))
                            {
                                result = (dynamic)FinalValue % arg;
                                return true;
                            }
                            break;
                        case ExpressionType.ModuloAssign:
                            if (myValueType == typeof(int))
                            {
                                result = (dynamic)FinalValue % arg;
                                return true;
                            }
                            break;
                        case ExpressionType.Multiply:
                            result = (dynamic)this.FinalValue * arg;
                            return true;
                        case ExpressionType.MultiplyAssign:
                            result = (dynamic)this.FinalValue * arg;
                            return true;
                        case ExpressionType.NotEqual:
                            result = (dynamic)this.FinalValue != arg;
                            return true;
                        case ExpressionType.RightShift:
                            if (myValueType == typeof(int))
                            {
                                result = (dynamic)FinalValue >> arg;
                                return true;
                            }
                            break;
                        case ExpressionType.RightShiftAssign:
                            if (myValueType == typeof(int))
                            {
                                result = (dynamic)FinalValue >> arg;
                                return true;
                            }
                            break;
                        case ExpressionType.Subtract:
                            result = (dynamic)FinalValue - arg;
                            return true;
                        case ExpressionType.SubtractAssign:
                            result = (dynamic)FinalValue - arg;
                            return true;
                        default:
                            break;
                    }
                }  

            }
            catch (InvalidCastException)
            {
                result = null;
                return false;
            }

            // see what the arg type is;
            //Type argType = arg.GetType();
            //bool isString = ( myValueType == typeof(string) );
            //if (argType.IsGenericType)
            //{
            //    Type genType = argType.GetGenericTypeDefinition();
            //    if (genType == typeof(IValueTag<>))
            //    {
            //        Type valType = genType.GetGenericArguments()[0];
            //    }
            //}
            return base.TryBinaryOperation(binder, arg, out result);
        }

        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            // the simplest case
            if (binder.Type == typeof(T))
            {
                result = this.FinalValue;
                return true;
            }

            // try converting to output type
            try
            {
                if (binder.Type == typeof(Int16))
                {
                    result = Convert.ToInt16(this.FinalValue);
                }
                else if (binder.Type == typeof(Int32))
                {
                    result = Convert.ToInt32(this.FinalValue);
                }
                else if (binder.Type == typeof(Int64))
                {
                    result = Convert.ToInt64(this.FinalValue);
                }
                else if (binder.Type == typeof(Boolean))
                {
                    result = Convert.ToBoolean(this.FinalValue);
                }
                else if (binder.Type == typeof(Single))
                {
                    result = Convert.ToSingle(this.FinalValue);
                }
                else if (binder.Type == typeof(Double))
                {
                    result = Convert.ToDouble(this.FinalValue);
                }
                else if (binder.Type == typeof(Decimal))
                {
                    result = Convert.ToDecimal(this.FinalValue);
                }
                else
                {
                    result = null;
                    return false;
                }

                return true;
            }
            catch (InvalidCastException)
            {
                result = null;
                return true;
            }
        }

            #endregion // DynamicObject Overloads

        #endregion // Public Methods

    }
}

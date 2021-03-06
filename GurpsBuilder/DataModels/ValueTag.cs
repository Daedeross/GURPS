﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpressionEvaluator;
using System.Dynamic;
using System.Linq.Expressions;
using GurpsBuilder.DataModels.Events;
using GurpsBuilder.Helpers;

namespace GurpsBuilder.DataModels
{
    public class ValueTag<T>: DynamicDataModel, IValueTag<T>
    {
        #region Fields

        protected T mDefaultValue;
        protected T mBonusValue;
        protected T mLastValidValue;
        protected string _exprText;
        protected CompiledExpression<T> _exprCompiled;
        protected Func<Context, T> _exprDelegate;
        protected Func<ValueTag<T>, T> _finalValueDelegate;
        protected dynamic context;
        protected Type mValueType = typeof(T);

        #endregion // Fields
        #region Properties

        public T Value
        {
            get
            {
                return mLastValidValue;
                //T val;
                //try
                //{
                //    val = _exprDelegate(context);
                //    mLastValidValue = val;
                //}
                //catch
                //{
                //    val = mLastValidValue;
                //}
                //return val;
            }
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

        public event ValueChangedEventHandler ValueChanged;

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
                    if (CompileExpression(value))
                    {
                        T val;
                        try
                        {
                            val = _exprDelegate(context);
                            mLastValidValue = val;
                        }
                        catch (Exception e)
                        {
                            Console.Write(e.Message);
                            val = mLastValidValue;
                        }

                        if (!val.Equals(mLastValidValue))
                        {
                            OnValueChanged(new ValueChangedEventArgs(mLastValidValue, null));
                        }
                    }
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
        #region Private Methods

        private bool CompileExpression(string expr)
        {
            var lastText = _exprText;
            //var lastExpr = _exprCompiled;
            //var lastDelegate = _exprDelegate;
            try
            {
                _exprText = expr;
                _exprCompiled = new CompiledExpression<T>(_exprText);
                _exprDelegate = _exprCompiled.ScopeCompile<Context>();
                return true;
            }
            catch
            {
                _exprText = lastText;
                _exprCompiled = null;
                _exprDelegate = (c => this.mDefaultValue);
                return false;
            }
        }

        private void ResetContext(ITaggable owner)
        {
            context = Context.Generate(owner);
        }
        
        private void OnValueChanged(object sender, ValueChangedEventArgs e)
        {
            ValueChanged?.Invoke(sender, e);
        }

        private void OnValueChanged(ValueChangedEventArgs e)
        {
            OnValueChanged(this, e);
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
            if (ObjectExtensions.IsNumeric(mValueType))
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
                        if (mValueType == typeof(int) || mValueType == typeof(uint))
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
            else if (mValueType == typeof(string))
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
            if(mValueType == typeof(string))
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
                if (ObjectExtensions.IsNumeric(binder.ReturnType))
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
                            if (mValueType == typeof(int))
                            {
                                result = (dynamic)FinalValue << arg;
                                return true;
                            }
                            break;
                        case ExpressionType.LeftShiftAssign:
                            if (mValueType == typeof(int))
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
                            if (mValueType == typeof(int))
                            {
                                result = (dynamic)FinalValue % arg;
                                return true;
                            }
                            break;
                        case ExpressionType.ModuloAssign:
                            if (mValueType == typeof(int))
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
                            if (mValueType == typeof(int))
                            {
                                result = (dynamic)FinalValue >> arg;
                                return true;
                            }
                            break;
                        case ExpressionType.RightShiftAssign:
                            if (mValueType == typeof(int))
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
            //bool isString = ( mValueType == typeof(string) );
            //if (argType.IsGenericType)
            //{
            //    Type genType = argType.GetGenericTypeDefinition();
            //    if (genType == typeof(IValueTag<>))
            //    {
            //        Type valType = genType.GetGenericArguments()[0];
            //    }
            //}
            dynamic me = this;
            if (ObjectExtensions.IsNumeric(binder.ReturnType))
            {
                float t = me;
                switch (binder.Operation)
                {
                    case ExpressionType.Add:
                        result = t + (float) arg;
                        return true;
                    case ExpressionType.AddAssign:
                        result = t + (float)arg;
                        return true;
                    case ExpressionType.Equal:
                        result = t == (float)arg;
                        return true;
                    case ExpressionType.NotEqual:
                        result = t != (float)arg;
                        return true;
                    default:
                        result = this.FinalValue;
                        return false;
                }
            }

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
        #region Implicit Conversion Methods

        public static implicit operator bool(ValueTag<T> t)
        {
            bool result;
            try
            {
                result = Convert.ToBoolean(t.FinalValue);
            }
            catch (InvalidCastException)
            {
                result = false;
            }
            return result;
        }

        public static implicit operator short(ValueTag<T> t)
        {
            short result;
            try
            {
                result = Convert.ToInt16(t.FinalValue);
            }
            catch (InvalidCastException)
            {
                result = 0;
            }
            return result;
        }

        public static implicit operator int(ValueTag<T> t)
        {
            int result;
            try
            {
                result = Convert.ToInt32(t.FinalValue);
            }
            catch (InvalidCastException)
            {
                result = 0;
            }
            return result;
        }

        public static implicit operator long(ValueTag<T> t)
        {
            long result;
            try
            {
                result = Convert.ToInt64(t.FinalValue);
            }
            catch (InvalidCastException)
            {
                result = 0;
            }
            return result;
        }

        public static implicit operator float(ValueTag<T> t)
        {
            float result;
            try
            {
                result = Convert.ToSingle(t.FinalValue);
            }
            catch (InvalidCastException)
            {
                result = 0;
            }
            return result;
        }

        public static implicit operator double(ValueTag<T> t)
        {
            double result;
            try
            {
                result = Convert.ToDouble(t.FinalValue);
            }
            catch (InvalidCastException)
            {
                result = 0;
            }
            return result;
        }

        public static implicit operator decimal(ValueTag<T> t)
        {
            decimal result;
            try
            {
                result = Convert.ToDecimal(t.FinalValue);
            }
            catch (InvalidCastException)
            {
                result = 0;
            }
            return result;
        }

        #endregion
    }
}

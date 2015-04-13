using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpressionEvaluator;

namespace GurpsBuilder.DataModels
{
    public class ValueTag<T>: DataModelBase, IValueTag<T>
    {
        #region Fields

        protected T mDefaultValue;
        protected T mBonusValue;
        protected string _exprText;
        protected CompiledExpression<T> _exprCompiled;
        protected Func<Context, T> _exprDelegate;
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
            get { return Value }
        }

        public T OverrideValue
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
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
                throw new NotImplementedException();
            }
        }

        public bool ReadOnly
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
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

        #region Private Methods

        private void CompileExpression(string expr)
        {
            try
            {
                _exprText = expr;
                _exprCompiled = new CompiledExpression<T>(_exprText);
                _exprDelegate = _exprCompiled.ScopeCompile<Context>();

            }
            catch
            {
                
                throw;
            }
        }

        #endregion // Private Methods

        #region Constructors

        #endregion // Constructors

        #region Commands

        #endregion // Commands

        #region Private Methods

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

        #endregion // Public Methods
    }
}

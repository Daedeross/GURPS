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
        protected T _baseValue;
        protected T _bonusValue;
        protected string _exprText;
        protected CompiledExpression<T> _exprCompiled;
        protected Func<Context, T> _exprDelegate;

        public T Value
        {
            get
            {
                Context c = new Context() { character = Owner.Character, owner = this.Owner };
                return _exprDelegate(c);
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public T DefaultValue
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

        public T BonusValue
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

        public T FinalValue
        {
            get { throw new NotImplementedException(); }
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


        public ITrait Owner
        {
            get { throw new NotImplementedException(); }
        }

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
    }
}

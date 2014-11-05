using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GurpsBuilder.DataModels
{
    interface IValueTag<T> : ITag
    {
        public T Value { get; set; }

        public T BaseValue { get; set; }

        public T BonusValue { get; set; }

        public T FinalValue { get; }

        public Nullable<T> OverrideValue { get; set; }

        event EventHandler ValueChanged;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GurpsBuilder.DataModels
{
    public interface IValueTag<T> : ITag
    {
        T Value { get; set; }

        T DefaultValue { get; set; }

        T BonusValue { get; set; }

        T FinalValue { get; }

        T OverrideValue { get; set; }

        Type GetValueType();

        event EventHandler ValueChanged;
    }
}

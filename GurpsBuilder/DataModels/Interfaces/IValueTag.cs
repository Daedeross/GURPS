using System;

namespace GurpsBuilder.DataModels
{
    public interface IValueTag<T> : ITag, INotifyValueChanged
    {
        T Value { get; }

        T DefaultValue { get; set; }

        T BonusValue { get; set; }

        T FinalValue { get; }

        T OverrideValue { get; set; }

        Type GetValueType();
    }
}

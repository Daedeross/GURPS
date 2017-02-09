using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GurpsBuilder.DataModels.Events
{
    public delegate void ValueChangedEventHandler(object sender, ValueChangedEventArgs e);

    public class ValueChangedEventArgs: EventArgs
    {
        public ValueChangedEventArgs(dynamic oldValue, dynamic newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        public dynamic OldValue { get; set; }
        public dynamic NewValue { get; set; }
    }
}

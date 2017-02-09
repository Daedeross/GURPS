using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GurpsBuilder.DataModels.Events;

namespace GurpsBuilder.DataModels
{
    public interface INotifyValueChanged
    {
        event ValueChangedEventHandler ValueChanged;
    }
}

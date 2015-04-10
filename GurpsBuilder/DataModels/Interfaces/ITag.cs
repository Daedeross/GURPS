using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GurpsBuilder.DataModels
{
    public interface ITag: INotifyPropertyChanged
    {
        string Text { get; set; }
        bool ReadOnly { get; set; }

        ITrait Owner { get; }
    }
}

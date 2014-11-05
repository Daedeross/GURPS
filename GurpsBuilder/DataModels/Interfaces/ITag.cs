using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GurpsBuilder.DataModels
{
    interface ITag: INotifyPropertyChanged
    {
        public string Text { get; set; }
        public bool ReadOnly { get; set; }

        public ITrait Owner { get; }
    }
}

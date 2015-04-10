using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GurpsBuilder.DataModels
{
    public interface IGivesTag: ITag
    {
        event EventHandler EventChanged;
    }
}

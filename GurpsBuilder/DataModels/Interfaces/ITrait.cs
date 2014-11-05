using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GurpsBuilder.DataModels
{
    interface ITrait
    {
        public ITag this[string name] { get; set; }

        public List<ITrait> Mods { get; set; }
    }
}

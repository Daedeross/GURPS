using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GurpsBuilder.DataModels
{
    public class Character : DataModelBase
    {
        public Dictionary<string, ITrait> Attributes { get; set; }

        public Dictionary<string, ITrait> Advantages { get; set; }

        public Dictionary<string, ITrait> Disadvantages { get; set; }

        public Dictionary<string, ITrait> Skills { get; set; }

        public Dictionary<string, ITrait> Items { get; set; }

    }
}

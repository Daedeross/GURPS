using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GurpsBuilder.DataModels
{
    public class Character : DataModelBase
    {
        #region Private Fields

        #endregion // Private fields

        #region Properties

        public int Age { get; set; }

        public int Height { get; set; }

        public Dictionary<string, BaseTrait> Attributes { get; set; }

        public Dictionary<string, BaseTrait> Advantages { get; set; }

        public Dictionary<string, BaseTrait> Disadvantages { get; set; }

        public Dictionary<string, BaseTrait> Skills { get; set; }

        public Dictionary<string, BaseTrait> Items { get; set; }
        
        #endregion // Properties

        #region Constructors

        public Character()
        {
            Attributes = new Dictionary<string, BaseTrait>();
            Advantages = new Dictionary<string, BaseTrait>();
            Disadvantages = new Dictionary<string, BaseTrait>();
            Skills = new Dictionary<string, BaseTrait>();
            Items = new Dictionary<string, BaseTrait>();
        }

        #endregion // Constructors

        #region Private Methods

        #endregion // Private Methods

        #region Public Methods

        #endregion // Public Methods

    }
}

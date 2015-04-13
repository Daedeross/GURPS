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

        public dynamic Attributes { get; set; }

        public dynamic Advantages { get; set; }

        public dynamic Disadvantages { get; set; }

        public dynamic Skills { get; set; }

        public dynamic Items { get; set; }
        
        #endregion // Properties

        #region Constructors

        public Character()
        {
            Attributes = new ExpandoObject();
            Advantages = new ExpandoObject();
            Disadvantages = new ExpandoObject();
            Skills = new ExpandoObject();
            Items = new ExpandoObject();
        }

        #endregion // Constructors

        #region Private Methods

        #endregion // Private Methods

        #region Public Methods

        #endregion // Public Methods

    }
}

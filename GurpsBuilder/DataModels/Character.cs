using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GurpsBuilder.DataModels
{
    public class TraitList : DynamicObject
    {
        private Dictionary<string, BaseTrait> _dict;

        public TraitList()
        {
            _dict = new Dictionary<string, BaseTrait>();
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            BaseTrait o;
            bool ret;
            ret = _dict.TryGetValue(binder.Name, out o);
            result = o;
            return ret;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            BaseTrait bt = value as BaseTrait;
            if (bt != null)
            {
                _dict[binder.Name] = bt;
                return true;
            }
            return false;
        }
    }

    public class Character : DynamicDataModel
    {
        #region Private Fields

        #endregion // Private fields

        #region Properties

        public double Age { get; set; }

        public double Height { get; set; }

        //public Dictionary<string, BaseTrait> Attributes { get; set; }
        //
        //public Dictionary<string, BaseTrait> Advantages { get; set; }
        //
        //public Dictionary<string, BaseTrait> Disadvantages { get; set; }
        //
        //public Dictionary<string, BaseTrait> Skills { get; set; }
        //
        //public Dictionary<string, BaseTrait> Items { get; set; }

        public TraitList Attributes { get; set; }
        public TraitList Advantages { get; set; }
        public TraitList Disadvantages { get; set; }
        public TraitList Skills { get; set; }
        public TraitList Items { get; set; }
        
        #endregion // Properties

        #region Constructors

        public Character()
        {
            //Attributes = new Dictionary<string, BaseTrait>();
            //Advantages = new Dictionary<string, BaseTrait>();
            //Disadvantages = new Dictionary<string, BaseTrait>();
            //Skills = new Dictionary<string, BaseTrait>();
            //Items = new Dictionary<string, BaseTrait>();

            Attributes = new TraitList();
            Advantages = new TraitList();
            Disadvantages = new TraitList();
            Skills = new TraitList();
            Items = new TraitList();
        }

        #endregion // Constructors

        #region Private Methods

        #endregion // Private Methods

        #region Public Methods

        #endregion // Public Methods

    }
}

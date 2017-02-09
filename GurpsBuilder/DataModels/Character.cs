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
        
        public TraitList General { get; set; }
        public TraitList Attributes { get; set; }
        public TraitList Advantages { get; set; }
        public TraitList Disadvantages { get; set; }
        public TraitList Skills { get; set; }
        public TraitList Items { get; set; }
        
        #endregion // Properties
        #region Constructors

        public Character()
        {
            General = new TraitList();
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

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            switch (binder.Name)
            {
                case "Attributes":
                    result = Attributes;
                    return true;
                case "Advantages":
                    result = Advantages;
                    return true;
                case "Disadvantages":
                    result = Disadvantages;
                    return true;
                case "Skills":
                    result = Skills;
                    return true;
                case "Items":
                    result = Items;
                    return true;
                default:
                    return General.TryGetMember(binder, out result);
            }
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            switch (binder.Name)
            {
                case "Attributes":
                case "Advantages":
                case "Disadvantages":
                case "Skills":
                case "Items":
                    return false;
                default:
                    return General.TrySetMember(binder, value);
            }
            
        }

        #endregion // Public Methods
    }
}

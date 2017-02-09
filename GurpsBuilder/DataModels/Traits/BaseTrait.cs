using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;
using GurpsBuilder.DataModels.Events;

namespace GurpsBuilder.DataModels
{
    public class BaseTrait: DynamicDataModel, IModdable, ITaggable, INotifyValueChanged
    {
        #region Private Fields

        #endregion // Private fields
        #region Properties

        //public readonly string DefaultTagName;

        //public readonly ITag DefaultTag;

        public Character Character { get; private set; }

        //public ITag this[string index]
        //{
        //    get
        //    {
        //        return Tags[index];
        //    }
        //    set { Tags[index] = value; }
        //}

            #region ITaggable Implementation

        protected Dictionary<string, ITag> mTags;
        public Dictionary<string, ITag> Tags
        {
            get { return mTags; }
            set
            {
                if (value != mTags)
                {
                    mTags = value;
                    OnPropertyChanged("Tags");
                }
            }
        }

        public bool ContainsTag(string name)
        {
            return mTags.ContainsKey(name);
        }

        public bool TryGetTag(string name, out ITag tag)
        {
            return(mTags.TryGetValue(name, out tag));
        }

        public T GetTagValue<T>(string name)
        {
            T obj = default(T);
            ITag tag;

            if (mTags.TryGetValue(name, out tag))
            {
                IValueTag<T> vtag = tag as IValueTag<T>;
                if (vtag != null)
                {
                    obj = vtag.FinalValue;
                }
            }

            return obj;
        }

        public bool TryGetTagValue<T>(string name, out T value)
        {
            value = default(T);
            ITag tag;

            if (mTags.TryGetValue(name, out tag))
            {
                IValueTag<T> vtag = tag as IValueTag<T>;
                if (vtag != null)
                {
                    value = vtag.FinalValue;
                    return true;
                }
            }
            return false;
        }

            #endregion ITaggable Implementation
            #region IModdable Implementation

        protected Dictionary<string, Modifier> mMods;

        public Dictionary<string, Modifier> Mods
        {
            get { return mMods; }
            set
            {
                if (value != mMods)
                {
                    mMods = value;
                    OnPropertyChanged("Mods");
                }
            }
        }

        public bool TryGetMod(string name, out Modifier mod)
        {
            return mMods.TryGetValue(name, out mod);
        }

        public bool ContainsMod(string name)
        {
            return mMods.ContainsKey(name);
        }

            #endregion // IModdable Implementation

        #endregion // Properties
        #region Constructors

        public BaseTrait()
        {
            Initialize();
        }

        public BaseTrait(Character character)
        {
            Character = character;
            Initialize();
        }

        private void Initialize()
        {
            mMods = new Dictionary<string, Modifier>();
            mTags = new Dictionary<string, ITag>();
        }

        #endregion // Constructors
        #region Private Methods

        #endregion // Private Methods
        #region Public Methods

        public void Attatch(Character character)
        {
            this.Character = character;
        }

            #region DynamicObject Overloads

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            string name = binder.Name.ToLower();
            ITag obj;

            bool ret = mTags.TryGetValue(name, out obj);
            result = obj;
            return ret;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            string name = binder.Name.ToLower();
            ITag tag = value as ITag;
            if (tag != null)
            {
                mTags[name] = tag;
                if (name == "score" && tag is INotifyValueChanged)
                {
                    var vt = tag as INotifyValueChanged;
                    vt.ValueChanged += this.OnValueChanged;
                }

                return true;
            }
            return false;
        }

        #endregion // DynamicObject Overloads
        #endregion // Public Methods
        #region IValueChangedImplimentation
        
        public event ValueChangedEventHandler ValueChanged;

        private void OnValueChanged(object sender, ValueChangedEventArgs e)
        {
            ValueChanged?.Invoke(sender, e);
        }

        #endregion // IValueChangedImplimentation
        #region Arithmetric Operators

        public override bool TryBinaryOperation(BinaryOperationBinder binder, object arg, out object result)
        {
            return base.TryBinaryOperation(binder, arg, out result);
        }

        public static implicit operator bool(BaseTrait t)
        {
            bool result = false;
            ITag tag;
            if (t.TryGetTag("score", out tag))
            {
                dynamic vt = tag;
                try
                {
                    result = Convert.ToBoolean(vt.FinalValue);
                }
                catch (InvalidCastException)
                {
                    result = false;
                }
            }
            return result;
        }

        public static implicit operator short(BaseTrait t)
        {
            short result = 0;
            ITag tag;
            if (t.TryGetTag("score", out tag))
            {
                dynamic vt = tag;
                try
                {
                    result = Convert.ToInt16(vt.FinalValue);
                }
                catch (InvalidCastException)
                {
                    result = 0;
                }
            }
            return result;
        }

        public static implicit operator int(BaseTrait t)
        {
            int result = 0;
            ITag tag;
            if (t.TryGetTag("score", out tag))
            {
                dynamic vt = tag;
                try
                {
                    result = Convert.ToInt32(vt.FinalValue);
                }
                catch (InvalidCastException)
                {
                    result = 0;
                }
            }
            return result;
        }

        public static implicit operator long(BaseTrait t)
        {
            long result = 0L;
            ITag tag;
            if (t.TryGetTag("score", out tag))
            {
                dynamic vt = tag;
                try
                {
                    result = Convert.ToInt64(vt.FinalValue);
                }
                catch (InvalidCastException)
                {
                    result = 0;
                }
            }
            return result;
        }

        public static implicit operator float(BaseTrait t)
        {
            float result = 0f;
            ITag tag;
            if (t.TryGetTag("score", out tag))
            {
                dynamic vt = tag;
                try
                {
                    result = Convert.ToSingle(vt.FinalValue);
                }
                catch (InvalidCastException)
                {
                    result = 0;
                }
            }
            return result;
        }

        public static implicit operator double(BaseTrait t)
        {
            double result = 0;
            ITag tag;
            if (t.TryGetTag("score", out tag))
            {
                dynamic vt = tag;
                try
                {
                    result = Convert.ToDouble(vt.FinalValue);
                }
                catch (InvalidCastException)
                {
                    result = 0;
                }
            }
            return result;
        }

        public static implicit operator decimal(BaseTrait t)
        {
            decimal result = 0m;
            ITag tag;
            if (t.TryGetTag("score", out tag))
            {
                dynamic vt = tag;
                try
                {
                    result = Convert.ToDecimal(vt.FinalValue);
                }
                catch (InvalidCastException)
                {
                    result = 0;
                }
            }
            return result;
        }

        #endregion // Arithmetric Operators
    }
}

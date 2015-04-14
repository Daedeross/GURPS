using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;

namespace GurpsBuilder.DataModels
{
    public class BaseTrait: DynamicDataModel, IModdable, ITaggable
    {
        #region Private Fields

        #endregion // Private fields

        #region Properties

        public int Test1 { get; set; }

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

        #region Commands

        #endregion // Commands

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
                return true;
            }
            return false;
        }

            #endregion // DynamicObject Overloads

        #endregion // Public Methods

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GurpsBuilder.DataModels
{
    public class Modifier : DataModelBase, IModdable, ITaggable
    {
        #region Private Fields

        #endregion // Private fields

        #region Properties

        public Character Character
        {
            get { return mOwner.Character; }
        }

        protected IModdable mOwner;
        public IModdable Owner
        {
            get { return mOwner; }
            set
            {
                if (value != mOwner)
                {
                    mOwner = value;
                    OnPropertyChanged("Owner");
                }
            }
        }

            #region ITaggable Implementation

        protected Dictionary<string, ITag> _tags;
        public Dictionary<string, ITag> Tags
        {
            get { return _tags; }
            set
            {
                if (value != _tags)
                {
                    _tags = value;
                    OnPropertyChanged("Tags");
                }
            }
        }

        public bool ContainsTag(string name)
        {
            return _tags.ContainsKey(name);
        }

        public bool TryGetTag(string name, out ITag tag)
        {
            return (_tags.TryGetValue(name, out tag));
        }

        public T GetTagValue<T>(string name)
        {
            T obj = default(T);
            ITag tag;

            if (_tags.TryGetValue(name, out tag))
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

            if (_tags.TryGetValue(name, out tag))
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

        #endregion // Constructors

        #region Commands

        #endregion // Commands

        #region Private Methods

        #endregion // Private Methods

        #region Public Methods

        public void Attatch(IModdable owner)
        {
            mOwner = owner;
        }

        #endregion // Public Methods

    }
}

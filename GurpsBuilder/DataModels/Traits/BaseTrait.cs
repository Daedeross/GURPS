using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GurpsBuilder.DataModels
{
    public class BaseTrait: DataModelBase, ITrait
    {
        protected Dictionary<string, ITag> _tags;
        protected Dictionary<string, ITrait> _mods;

        public Character Character { get; set; }

        public ITag this[string name]
        {
            get
            {
                return _tags[name];
            }
            set
            {
                _tags[name] = value;
            }
        }

        public List<ITrait> Mods
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        bool ContainsTag(string name)
        {
            return _tags.ContainsKey(name);
        }

        bool TryGetTag(string name, out ITag tag)
        {
            return(_tags.TryGetValue(name, out tag));
        }

        public T GetValueOfTag<T>(string name)
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
    }
}

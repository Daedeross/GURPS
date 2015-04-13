using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GurpsBuilder.DataModels
{
    public interface ITaggable
    {
        Character Character { get; }

        Dictionary<string, ITag> Tags { get; set; }

        bool TryGetTag(string name, out ITag tag);

        bool ContainsTag(string name);

        T GetTagValue<T>(string name);

        bool TryGetTagValue<T>(string name, out T value);
    }
}

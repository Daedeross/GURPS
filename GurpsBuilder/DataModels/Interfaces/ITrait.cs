using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GurpsBuilder.DataModels
{
    public interface ITrait
    {
        Character Character { get; set; }

        ITag this[string name] { get; set; }

        List<ITrait> Mods { get; set; }

        bool TryGetTag(string name, out ITag tag);

        bool ContainsTag(string name);

        T GetValueOfTag<T>(string name);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GurpsBuilder.DataModels
{
    public interface IModdable
    {
        Character Character { get; }

        Dictionary<string, Modifier> Mods { get; set; }

        bool TryGetMod(string name, out Modifier mod);

        bool ContainsMod(string name);
    }
}

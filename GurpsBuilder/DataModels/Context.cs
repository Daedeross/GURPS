using System.Collections.Generic;

namespace GurpsBuilder.DataModels
{
    public struct Context
    {
        public Character character;

        public Dictionary<string, ITrait> Attributes { get { return character.Attributes; } }

        public Dictionary<string, ITrait> Advantages { get { return character.Advantages; } }

        public Dictionary<string, ITrait> Disadvantages { get { return character.Disadvantages; } }

        public Dictionary<string, ITrait> Skills { get { return character.Skills; } }

        public Dictionary<string, ITrait> Items { get { return character.Attributes; } }

        public ITrait owner;
    }
}

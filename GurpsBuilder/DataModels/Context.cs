using System.Collections.Generic;

namespace GurpsBuilder.DataModels
{
    public struct Context
    {
        public Character character;

        public Dictionary<string, BaseTrait> Attributes { get { return character.Attributes; } }

        public Dictionary<string, BaseTrait> Advantages { get { return character.Advantages; } }

        public Dictionary<string, BaseTrait> Disadvantages { get { return character.Disadvantages; } }

        public Dictionary<string, BaseTrait> Skills { get { return character.Skills; } }

        public Dictionary<string, BaseTrait> Items { get { return character.Attributes; } }

        public ITaggable owner;

        public static Context Generate(ITaggable owner)
        {
            Context context = new Context();
            context.owner = owner;
            context.character = owner.Character;
            return context;
        }
    }
}

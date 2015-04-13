using System.Collections.Generic;

namespace GurpsBuilder.DataModels
{
    public struct Context
    {
        public Character character;

        public dynamic Attributes { get { return character.Attributes; } }

        public dynamic Advantages { get { return character.Advantages; } }

        public dynamic Disadvantages { get { return character.Disadvantages; } }

        public dynamic Skills { get { return character.Skills; } }

        public dynamic Items { get { return character.Attributes; } }

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

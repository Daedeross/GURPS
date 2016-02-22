using System.Collections.Generic;

namespace GurpsBuilder.DataModels
{
    public struct Context
    {
        public Character character;

        public TraitList Attributes { get { return character.Attributes; } }
        public TraitList Advantages { get { return character.Advantages; } }
        public TraitList Disadvantages { get { return character.Disadvantages; } }
        public TraitList Skills { get { return character.Skills; } }
        public TraitList Items { get { return character.Attributes; } }
        public dynamic owner;

        public static Context Generate(ITaggable owner)
        {
            Context context = new Context();
            context.owner = owner;
            context.character = owner.Character;
            return context;
        }
    }
}

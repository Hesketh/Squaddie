using System.Collections.Generic;

namespace Squaddie
{
    public sealed class CharacterPool : List<Character>
    {
        public string Name { get; set; }

        public CharacterPool(string name = "Squaddie")
        {
            Name = name;
        }
    }
}
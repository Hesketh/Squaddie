using System.Collections.Generic;

namespace Squaddie
{
    public sealed class CharacterPool
    {
        public string Name { get; set; }
        public List<Character> Characters { get; set; }

        public CharacterPool(string name = "Squaddie")
        {
            Characters = new List<Character>();
            Name = name;
        }
    }
}
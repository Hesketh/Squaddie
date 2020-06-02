using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Squaddie
{
    [DataContract]
    public sealed class CharacterPool
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public List<Character> Characters { get; set; }

        public CharacterPool(string name = "Squaddie")
        {
            Characters = new List<Character>();
            Name = name;
        }
    }
}
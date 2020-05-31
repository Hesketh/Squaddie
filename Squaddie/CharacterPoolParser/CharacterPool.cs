using System.Collections.Generic;

namespace Squaddie
{
    public class CharacterPool : List<Character>
    {
        private string m_name;

        public CharacterPool()
        {
            m_name = "Squaddie";
        }

        public CharacterPool(string filepath)
        {
            Load(filepath);
        }

        public void Load(string filepath)
        {
            CharacterPoolBuilder builder = new CharacterPoolBuilder();
            CharacterPool pool = builder.Load(filepath);

            foreach (Character character in pool)
            {
                Add(new Character(character));
            }
        }

        public void Save()
        {
            CharacterPoolBuilder builder = new CharacterPoolBuilder();
            builder.Save(this);
        }

        public string Name
        {
            get
            {
                return m_name;
            }
            set
            {
                m_name = value;
            }
        }
    }
}
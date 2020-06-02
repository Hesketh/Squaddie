using Newtonsoft.Json;
using Squaddie;
using System.IO;

namespace Squaddie.Serialization
{
    public class CharacterPoolJsonBuilder : ICharacterPoolFileBuilder
    {
        Formatting formattingStyle = Formatting.None;

        public CharacterPoolJsonBuilder(bool prettyPrint = false)
        {
            formattingStyle = prettyPrint ? Formatting.Indented : Formatting.None;
        }

        public CharacterPool LoadFromFile(string filepath)
        {
            string json = File.ReadAllText(filepath);
            return JsonConvert.DeserializeObject<CharacterPool>(json);
        }

        public void SaveToFile(string filepath, CharacterPool pool)
        {
            string json = JsonConvert.SerializeObject(pool, formattingStyle);
            File.WriteAllText(filepath, json);
        }
    }
}

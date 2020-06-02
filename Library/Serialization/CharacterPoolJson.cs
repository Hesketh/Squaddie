using Newtonsoft.Json;
using Squaddie;
using System.IO;

namespace Library.Serialization
{
    public class CharacterPoolJson
    {
        public CharacterPool LoadFromFile(string filepath)
        {
            string json = File.ReadAllText(filepath);
            return JsonConvert.DeserializeObject<CharacterPool>(json);
        }

        public void SaveToFile(string filepath, CharacterPool pool, bool prettyPrint = false)
        {
            string json = JsonConvert.SerializeObject(pool, prettyPrint ? Formatting.Indented : Formatting.None);
            File.WriteAllText(filepath, json);
        }
    }
}

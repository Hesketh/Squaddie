using Squaddie;

namespace Squaddie.Serialization
{
    public interface ICharacterPoolFileBuilder
    {
        CharacterPool LoadFromFile(string filepath);
        void SaveToFile(string filepath, CharacterPool pool);
    }
}

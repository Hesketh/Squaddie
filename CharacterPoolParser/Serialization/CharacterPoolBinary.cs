using System;
using System.Collections.Generic;
using System.IO;
using Squaddie.Properties;

namespace Squaddie.Serialization
{
    public sealed class CharacterPoolBinary
    {
        private BinaryPoolReader binaryPoolReader;

        public CharacterPool LoadFromFile(string filepath)
        {
            CharacterPool pool = new CharacterPool();
            byte[] file = File.ReadAllBytes(filepath);

            if (file == null)
            {
                throw new Exception("File not found!");
            }
            else
            {
                binaryPoolReader = new BinaryPoolReader(file);
                VerifyHeader(pool);
                ReadCharacters(pool);
            }

            return pool;
        }

        public void SaveToFile(string filepath, CharacterPool pool)
        {
            PropertyFactory factory = new PropertyFactory();
            List<byte> data = new List<byte>();

            string filename = Path.GetFileName(filepath);

            // Create the Header
            // This is magic number, it isn't used as far as we know
            data.AddRange(ByteConversionUtility.ByteInt(-1));
            data.AddRange(factory.ByteProperty(factory.CreateProperty("CharacterPool", "ArrayProperty", pool.Count)));
            // XCOM 2 expects that the filename in the data is the same as the actual filename
            data.AddRange(factory.ByteProperty(factory.CreateProperty("PoolFileName", "StrProperty", string.Format("CharacterPool\\Importable\\{0}", filename))));
            data.AddRange(factory.ByteProperty(factory.CreateProperty("None", "None", null)));
            // The character count is placed here again
            data.AddRange(ByteConversionUtility.ByteInt(pool.Count));

            foreach (Character character in pool)
            {
                foreach (IProperty characterProperty in character)
                {
                    data.AddRange(factory.ByteProperty(characterProperty));
                }
                data.AddRange(factory.ByteProperty(factory.CreateProperty("None", "None", null)));
            }

            File.WriteAllBytes(filepath, data.ToArray());
        }

        public void SaveInDirectory(string directoryPath, CharacterPool pool)
        {
            PropertyFactory factory = new PropertyFactory();
            List<byte> data = new List<byte>();

            string filename = pool.Name + ".bin";

            // Create the Header
            // This is magic number, it isn't used as far as we know
            data.AddRange(ByteConversionUtility.ByteInt(-1));
            data.AddRange(factory.ByteProperty(factory.CreateProperty("CharacterPool", "ArrayProperty", pool.Count)));
            // XCOM 2 expects that the filename in the data is the same as the actual filename
            data.AddRange(factory.ByteProperty(factory.CreateProperty("PoolFileName", "StrProperty", string.Format("CharacterPool\\Importable\\{0}", filename))));
            data.AddRange(factory.ByteProperty(factory.CreateProperty("None", "None", null)));
            // The character count is placed here again
            data.AddRange(ByteConversionUtility.ByteInt(pool.Count));

            foreach (Character guy in pool)
            {
                foreach (IProperty guyProp in guy)
                {
                    data.AddRange(factory.ByteProperty(guyProp));
                }
                data.AddRange(factory.ByteProperty(factory.CreateProperty("None", "None", null)));
            }

            File.WriteAllBytes(Path.Combine(directoryPath, filename), data.ToArray());
        }

        private void ReadCharacters(CharacterPool pool)
        {
            int amountOfCharacters = binaryPoolReader.ReadInt();

            for (int index = 0; index < amountOfCharacters; index++)
            {
                pool.Add(ReadCharacter());
            }
        }

        private Character ReadCharacter()
        {
            PropertyFactory factory = new PropertyFactory();
            Character character = new Character();

            IProperty property = factory.ReadProperty(ref binaryPoolReader);

            if (property == null)
            {
                throw new Exception("Character property was null");
            }

            while (property.Name != "None")
            {
                character.Add(property);
                property = factory.ReadProperty(ref binaryPoolReader);

                if (property == null)
                {
                    throw new Exception("Character property was null");
                }
            }

            return character;
        }

        private void VerifyHeader(CharacterPool pool)
        {
            //Verify Magic Number
            const int MagicNumber = -1;
            if (binaryPoolReader.ReadInt() != MagicNumber)
            {
                throw new Exception("Incorrect Header: Unexpected Magic Number!");
            }

            //Verify file properties
            PropertyFactory factory = new PropertyFactory();

            if (factory.ReadProperty(ref binaryPoolReader).Name != "CharacterPool")
            {
                throw new Exception("Incorrect Header: Did Not Read Expected Property CharacterPool!");
            }

            IProperty poolFileName = factory.ReadProperty(ref binaryPoolReader);
            if (poolFileName.Name == "PoolFileName")
            {
                //xcom has some issues if you change this from the format that it actually expects
                string readName = (poolFileName).Value;
                //we extract the actual filename from the filepath because we want to enable the possibility of renaming it
                pool.Name = (readName.Substring(25, readName.Length - 29)); 
            }
            else
            {
                throw new Exception("Incorrect Header: Did Not Read Expected Property PoolFileName!");
            }

            if (factory.ReadProperty(ref binaryPoolReader).Name != "None")
            {
                throw new Exception("Incorrect Header: Did Not Read Expected Property None!");
            }
        }
    }
}

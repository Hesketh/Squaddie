using System;
using System.Collections.Generic;
using System.IO;

namespace Squaddie
{
    internal class CharacterPoolBuilder
    {
        private CharacterPool pool;
        private BinaryPoolReader binaryPoolReader;

        public CharacterPoolBuilder()
        {
            pool = new CharacterPool();
        }

        public CharacterPool Load(string filepath)
        {
            byte[] file = File.ReadAllBytes(filepath);

            if (file == null)
            {
                throw new Exception("File not found!");
            }
            else
            {
                binaryPoolReader = new BinaryPoolReader(file);
                VerifyHeader();
                ReadCharacters();
            }

            return pool;
        }

        public void Save(CharacterPool pool)
        {
            PropertyFactory factory = new PropertyFactory();
            List<byte> data = new List<byte>();

            //Create the Header
            data.AddRange(StructureReading.ByteInt(-1)); //Magic Number, -1
            data.AddRange(factory.ByteProperty(factory.CreateProperty("CharacterPool", "ArrayProperty", pool.Count)));
            data.AddRange(factory.ByteProperty(factory.CreateProperty("PoolFileName", "StrProperty", string.Format("CharacterPool\\Importable\\{0}.bin", pool.Name))));
            data.AddRange(factory.ByteProperty(factory.CreateProperty("None", "None", null)));
            data.AddRange(StructureReading.ByteInt(pool.Count)); //character count... again

            foreach (Character guy in pool)
            {
                foreach (IProperty guyProp in guy)
                {
                    data.AddRange(factory.ByteProperty(guyProp));
                }
                data.AddRange(factory.ByteProperty(factory.CreateProperty("None", "None", null)));
            }

            File.WriteAllBytes(pool.Name + ".bin", data.ToArray());
        }

        private void ReadCharacters()
        {
            //int amountOfCharacters = m_binFile.ReadInt();
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
                throw new Exception("PROPERTY IS NULL;");
            }

            while (property.Name != "None")
            {
                character.Add(property);
                property = factory.ReadProperty(ref binaryPoolReader);

                if (property == null)
                {
                    throw new Exception("ads");
                }
            }

            return character;
        }

        public void VerifyHeader()
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
                pool.Name = (readName.Substring(25, readName.Length - 29)); //we extract the actual filename from the filepath because we want to enable the possibility of renaming it
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Squaddie
{
    internal class PropertyFactory
    {
        public PropertyFactory() {}

        public IProperty CreateProperty(string name, string type, dynamic data)
        {
            IProperty property = null;

            if (name != "None")
            {
                switch (type)
                {
                    case "ArrayProperty":
                    case "IntProperty":
                    case "BoolProperty":
                    case "NameProperty":
                    case "StrProperty":
                    case "StructProperty":
                        property = new Property(name, type, data);
                        break;
                    default:
                        throw new Exception("Property Creation Error: Cannot make property named " + name + " and of type " + type + ".");
                }
            }
            else
            {
                property = new NullProperty();
            }
            return property;
        }

        public byte[] ByteProperty(IProperty property)
        {
            List<byte> data = new List<byte>();

            data.AddRange(StructureReading.ByteString(property.Name));
            data.AddRange(StructureReading.BytePadding());

            //None property special case
            if (property.Name == "None")
            {
                //data.AddRange(StructureReading.BytePadding());
                return data.ToArray();
            }

            data.AddRange(StructureReading.ByteString(property.Type));
            data.AddRange(StructureReading.BytePadding());

            int size = -1;
            List<byte> propertyData = new List<byte>();
            switch (property.Type)
            {
                case "ArrayProperty":
                case "IntProperty":
                    propertyData.AddRange(StructureReading.ByteInt(((property)).Value));
                    size = propertyData.Count;
                    break;
                case "BoolProperty":
                    propertyData.AddRange(BitConverter.GetBytes(((property)).Value));
                    size = 0;   //The property on disk for a bool property is said to have a size of 0, it actually has a size of 1
                    break;
                case "NameProperty":
                    propertyData.AddRange(StructureReading.ByteString(((property)).Value));
                    propertyData.AddRange(StructureReading.BytePadding());
                    size = propertyData.Count;
                    break;
                case "StrProperty":
                    propertyData.AddRange(StructureReading.ByteString(((property)).Value));
                    size = propertyData.Count;
                    break;
                case "StructProperty":
                    foreach (IProperty internalStructProperty in ((property)).Value)
                    {
                        propertyData.AddRange(ByteProperty(internalStructProperty));
                    }
                    size = propertyData.Count;
                    propertyData.AddRange(ByteProperty(new NullProperty()));
                    break;
            }

            if (size < 0)
            {
                throw new Exception("Size is 0");
            }

            data.AddRange(StructureReading.ByteInt(size));
            data.AddRange(StructureReading.BytePadding());

            if (property.Type == "StructProperty")
            {
                //A struct property has an extra string and integer between the size and the data, irrelevant.
                data.AddRange(StructureReading.ByteString("TAppearance"));
                data.AddRange(StructureReading.BytePadding());
            }

            data.AddRange(propertyData);

            return data.ToArray();
        }

        public IProperty ReadProperty(ref BinaryPoolReader binFile)
        {
            IProperty property = null;

            string name = binFile.ReadString();
            binFile.ReadPadding();

            //None property, to be skipped usually
            if (name != "None")
            {
                string type = binFile.ReadString();
                binFile.ReadPadding();

                int size = binFile.ReadInt();
                binFile.ReadPadding();

                switch (type)
                {
                    case "ArrayProperty":
                    case "IntProperty":
                        property = new Property(name, type, binFile.ReadInt());
                        break;
                    case "BoolProperty":
                        size = 1;   //The property on disk for a bool property is said to have a size of 0, it actually has a size of 1
                        property = new Property(name, type, binFile.ReadBool());
                        break;
                    case "NameProperty":
                        property = new Property(name, type, binFile.ReadString());
                        binFile.ReadInt(); //name properties have an extra int after for some reason
                        break;
                    case "StrProperty":
                        property = new Property(name, type, binFile.ReadString());
                        break;
                    case "StructProperty":
                        binFile.ReadString(); //A struct property has an extra string and integer between the size and the data, safely skipped over
                        binFile.ReadInt();
                        property = new Property(name, type, ReadPropertiesWithinStruct(binFile.ReadBytes(size)));
                        break;
                    default:
                        throw new Exception("Property Creation Error: Cannot make property named " + name + " and of type " + type + ".");
                }
            }
            else
            {
                property = new NullProperty();
            }
            return property;
        }

        private List<IProperty> ReadPropertiesWithinStruct(byte[] raw)
        {
            List<IProperty> properties = new List<IProperty>();

            BinaryPoolReader binFile = new BinaryPoolReader(raw);
            //PropertyFactory factory = new PropertyFactory();

            IProperty property = ReadProperty(ref binFile);
            while (property.Name != "None")
            {
                properties.Add(property);
                property = ReadProperty(ref binFile);
            }
            return properties;
        }
    }
}
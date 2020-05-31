using System;
using System.Collections.Generic;
using Squaddie.Serialization;

namespace Squaddie.Properties
{
    internal class PropertyFactory
    {
        public PropertyFactory() { }

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
            Console.WriteLine($"\t\tByting Property {property.Name}");
            List<byte> data = new List<byte>();

            data.AddRange(ByteConversionUtility.ByteString(property.Name));
            data.AddRange(ByteConversionUtility.BytePadding());

            //None property special case
            if (property.Name == "None")
            {
                return data.ToArray();
            }

            data.AddRange(ByteConversionUtility.ByteString(property.Type));
            data.AddRange(ByteConversionUtility.BytePadding());

            int size = -1;
            List<byte> propertyData = new List<byte>();
            switch (property.Type)
            {
                case "ArrayProperty":
                case "IntProperty":
                    propertyData.AddRange(ByteConversionUtility.ByteInt(((property)).Value));
                    size = propertyData.Count;
                    break;
                case "BoolProperty":
                    propertyData.AddRange(BitConverter.GetBytes(((property)).Value));
                    //The property on disk for a bool property has a size of 1 but the property falsely claims 0
                    size = 0;   
                    break;
                case "NameProperty":
                    propertyData.AddRange(ByteConversionUtility.ByteString(((property)).Value));
                    propertyData.AddRange(ByteConversionUtility.BytePadding());
                    size = propertyData.Count;
                    break;
                case "StrProperty":
                    propertyData.AddRange(ByteConversionUtility.ByteString(((property)).Value));
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
                throw new Exception("Size is less than 0");
            }

            data.AddRange(ByteConversionUtility.ByteInt(size));
            data.AddRange(ByteConversionUtility.BytePadding());

            if (property.Type == "StructProperty")
            {
                //A struct property has an extra string and integer between the size and the data, irrelevant.
                data.AddRange(ByteConversionUtility.ByteString("TAppearance"));
                data.AddRange(ByteConversionUtility.BytePadding());
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
                        // Assume that invalid properties are supposed to be spaces
                        property = new NullProperty();
                        break;
                }
            }
            else
            {
                property = new NullProperty();
            }

            Console.WriteLine($"\t\tUnbit Property {property.Name}");
            return property;
        }

        private List<IProperty> ReadPropertiesWithinStruct(byte[] raw)
        {
            List<IProperty> properties = new List<IProperty>();
            BinaryPoolReader binFile = new BinaryPoolReader(raw);

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
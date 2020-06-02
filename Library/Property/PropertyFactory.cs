using Library.Property;
using Library.Serialization;
using Squaddie.Serialization;
using System;
using System.Collections.Generic;

namespace Squaddie.Properties
{
    internal class PropertyFactory
    {
        public PropertyFactory() { }

        public IProperty CreateProperty(string name, string type, dynamic data)
        {
            IProperty property = null;
            Console.WriteLine("Creating property " + name + " with type " + type);


            if (name != NoneProperty.TypeName)
            {
                switch (type)
                {
                    case ArrayProperty.TypeName:
                        property = new ArrayProperty(name, (int)data);
                        break;
                    case IntProperty.TypeName:
                        property = new IntProperty(name, (int)data);
                        break;
                    case BoolProperty.TypeName:
                        property = new BoolProperty(name, (bool)data);
                        break;
                    case NameProperty.TypeName:
                        property = new NameProperty(name, (string)data);
                        break;
                    case StringProperty.TypeName:
                        property = new StringProperty(name, (string)data);
                        break;
                    case StructProperty.TypeName:
                        property = new StructProperty(name, (List<IProperty>)data);
                        break;
                    default:
                        throw new Exception("Property Creation Error: Cannot make property named " + name + " and of type " + type + ".");
                }
            }
            else
            {
                property = new NoneProperty();
            }
            return property;
        }

        public byte[] ByteProperty(IProperty property)
        {
            Console.WriteLine("Byting... " + property.Name);
            return property.ToBinary();
        }

        public IProperty ReadProperty(ref BinaryPoolReader binFile)
        {
            IProperty property = null;

            string name = binFile.ReadString();
            binFile.ReadPadding();

            //None property, to be skipped usually
            if (name != NoneProperty.TypeName)
            {
                string type = binFile.ReadString();
                binFile.ReadPadding();

                int size = binFile.ReadInt();
                binFile.ReadPadding();

                switch (type)
                {
                    case ArrayProperty.TypeName:
                    case IntProperty.TypeName:
                        property = CreateProperty(name, type, binFile.ReadInt());
                        break;
                    case BoolProperty.TypeName:
                        //The property on disk for a bool property is said to have a size of 0, it actually has a size of 1
                        size = 1;
                        property = CreateProperty(name, type, binFile.ReadBool());
                        break;
                    case NameProperty.TypeName:
                        property = CreateProperty(name, type, binFile.ReadString());
                        //name properties have an extra int after for some reason
                        binFile.ReadInt();
                        break;
                    case StringProperty.TypeName:
                        property = CreateProperty(name, type, binFile.ReadString());
                        break;
                    case StructProperty.TypeName:
                        //A struct property has an extra string and integer between the size and the data, safely skipped over
                        binFile.ReadString();
                        binFile.ReadInt();
                        property = CreateProperty(name, type, ReadPropertiesWithinStruct(binFile.ReadBytes(size)));
                        break;
                    default:
                        throw new Exception("Property Creation Error: Cannot make property named " + name + " and of type " + type + ".");
                }
            }
            else
            {
                property = new NoneProperty();
            }

            return property;
        }

        private List<IProperty> ReadPropertiesWithinStruct(byte[] raw)
        {
            List<IProperty> properties = new List<IProperty>();
            BinaryPoolReader binFile = new BinaryPoolReader(raw);

            IProperty property = ReadProperty(ref binFile);
            while (property.Name != NoneProperty.TypeName)
            {
                properties.Add(property);
                property = ReadProperty(ref binFile);
            }
            return properties;
        }
    }
}
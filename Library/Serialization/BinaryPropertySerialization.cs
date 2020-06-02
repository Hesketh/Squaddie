using Library.Property;
using Squaddie.Serialization;
using System;
using System.Collections.Generic;

namespace Library.Serialization
{
    public static class BinaryPropertySerialization
    {
        public static byte[] ToBinary(this ArrayProperty property)
        {
            List<byte> data = new List<byte>();

            data.AddRange(ByteConversionUtility.ByteString(property.Name));
            data.AddRange(ByteConversionUtility.BytePadding());

            data.AddRange(ByteConversionUtility.ByteString(property.Type));
            data.AddRange(ByteConversionUtility.BytePadding());

            List<byte> propertyData = new List<byte>();
            propertyData.AddRange(ByteConversionUtility.ByteInt(((property)).Value));

            data.AddRange(ByteConversionUtility.ByteInt(propertyData.Count));
            data.AddRange(ByteConversionUtility.BytePadding());
            data.AddRange(propertyData);

            return data.ToArray();
        }

        public static byte[] ToBinary(this IntProperty property)
        {
            List<byte> data = new List<byte>();

            data.AddRange(ByteConversionUtility.ByteString(property.Name));
            data.AddRange(ByteConversionUtility.BytePadding());

            data.AddRange(ByteConversionUtility.ByteString(property.Type));
            data.AddRange(ByteConversionUtility.BytePadding());

            List<byte> propertyData = new List<byte>();
            propertyData.AddRange(ByteConversionUtility.ByteInt(((property)).Value));

            data.AddRange(ByteConversionUtility.ByteInt(propertyData.Count));
            data.AddRange(ByteConversionUtility.BytePadding());
            data.AddRange(propertyData);

            return data.ToArray();
        }

        public static byte[] ToBinary(this NoneProperty property)
        {
            List<byte> data = new List<byte>();

            data.AddRange(ByteConversionUtility.ByteString(property.Name));
            data.AddRange(ByteConversionUtility.BytePadding());

            return data.ToArray();
        }

        public static byte[] ToBinary(this NameProperty property)
        {
            List<byte> data = new List<byte>();

            data.AddRange(ByteConversionUtility.ByteString(property.Name));
            data.AddRange(ByteConversionUtility.BytePadding());
            data.AddRange(ByteConversionUtility.ByteString(property.Type));
            data.AddRange(ByteConversionUtility.BytePadding());

            List<byte> propertyData = new List<byte>();
            propertyData.AddRange(ByteConversionUtility.ByteString(((property)).Value));
            propertyData.AddRange(ByteConversionUtility.BytePadding());

            data.AddRange(ByteConversionUtility.ByteInt(propertyData.Count));
            data.AddRange(ByteConversionUtility.BytePadding());
            data.AddRange(propertyData);

            return data.ToArray();
        }

        public static byte[] ToBinary(this StringProperty property)
        {
            List<byte> data = new List<byte>();

            data.AddRange(ByteConversionUtility.ByteString(property.Name));
            data.AddRange(ByteConversionUtility.BytePadding());
            data.AddRange(ByteConversionUtility.ByteString(property.Type));
            data.AddRange(ByteConversionUtility.BytePadding());

            List<byte> propertyData = new List<byte>();
            propertyData.AddRange(ByteConversionUtility.ByteString(((property)).Value));

            data.AddRange(ByteConversionUtility.ByteInt(propertyData.Count));
            data.AddRange(ByteConversionUtility.BytePadding());
            data.AddRange(propertyData);

            return data.ToArray();
        }

        public static byte[] ToBinary(this BoolProperty property)
        {
            List<byte> data = new List<byte>();

            data.AddRange(ByteConversionUtility.ByteString(property.Name));
            data.AddRange(ByteConversionUtility.BytePadding());
            data.AddRange(ByteConversionUtility.ByteString(property.Type));
            data.AddRange(ByteConversionUtility.BytePadding());

            List<byte> propertyData = new List<byte>();
            propertyData.AddRange(BitConverter.GetBytes(((property)).Value));

            //The property on disk for a bool property has a size of 1 but the property falsely claims 0
            data.AddRange(ByteConversionUtility.ByteInt(0));
            data.AddRange(ByteConversionUtility.BytePadding());
            data.AddRange(propertyData);

            return data.ToArray();
        }

        public static byte[] ToBinary(this StructProperty property)
        {
            List<byte> data = new List<byte>();

            data.AddRange(ByteConversionUtility.ByteString(property.Name));
            data.AddRange(ByteConversionUtility.BytePadding());
            data.AddRange(ByteConversionUtility.ByteString(property.Type));
            data.AddRange(ByteConversionUtility.BytePadding());

            List<byte> propertyData = new List<byte>();
            foreach (IProperty internalStructProperty in property.Value)
            {
                propertyData.AddRange(internalStructProperty.ToBinary());
            }
            propertyData.AddRange((new NoneProperty()).ToBinary());

            data.AddRange(ByteConversionUtility.ByteInt(propertyData.Count));
            data.AddRange(ByteConversionUtility.BytePadding());

            //A struct property has an extra string and integer between the size and the data which is irrelevant.
            data.AddRange(ByteConversionUtility.ByteString("TAppearance"));
            data.AddRange(ByteConversionUtility.BytePadding());

            data.AddRange(propertyData);

            return data.ToArray();
        }

        public static byte[] ToBinary(this IProperty property)
        {
            switch (property.Type)
            {
                case ArrayProperty.TypeName:
                    return ((ArrayProperty)property).ToBinary();
                case IntProperty.TypeName:
                    return ((IntProperty)property).ToBinary();
                case BoolProperty.TypeName:
                    return ((BoolProperty)property).ToBinary();
                case NameProperty.TypeName:
                    return ((NameProperty)property).ToBinary();
                case StringProperty.TypeName:
                    return ((StringProperty)property).ToBinary();
                case StructProperty.TypeName:
                    return ((StructProperty)property).ToBinary();
                case NoneProperty.TypeName:
                    return ((NoneProperty)property).ToBinary();
                default:
                    throw new Exception("Attempt to convert interface property to binary");
            }
        }
    }
}

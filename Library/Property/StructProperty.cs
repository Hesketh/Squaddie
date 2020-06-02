using System.Collections.Generic;

namespace Library.Property
{
    public class StructProperty : IProperty
    {
        public const string TypeName = "StructProperty";

        public string Name { get; set; }
        public string Type => TypeName;
        public List<IProperty> Value { get; set; }

        public StructProperty(string name, List<IProperty> value)
        {
            Name = name;
            Value = value;
        }
    }
}

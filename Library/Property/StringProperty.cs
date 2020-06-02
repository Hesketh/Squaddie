namespace Squaddie.Property
{
    public class StringProperty : IProperty
    {
        public const string TypeName = "StrProperty";

        public string Name { get; set; }
        public string Type => TypeName;
        public string Value { get; set; }

        public StringProperty(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}

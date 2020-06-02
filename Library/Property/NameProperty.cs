namespace Library.Property
{
    public class NameProperty : IProperty
    {
        public const string TypeName = "NameProperty";

        public string Name { get; set; }
        public string Type => TypeName;
        public string Value { get; set; }

        public NameProperty(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}

namespace Library.Property
{
    public class BoolProperty : IProperty
    {
        public const string TypeName = "BoolProperty";

        public string Name { get; set; }
        public string Type => TypeName;
        public bool Value { get; set; }

        public BoolProperty(string name, bool value)
        {
            Name = name;
            Value = value;
        }
    }
}

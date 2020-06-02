namespace Library.Property
{
    public class ArrayProperty : IProperty
    {
        public const string TypeName = "ArrayProperty";

        public string Name { get; set; }
        public string Type => TypeName;
        public int Value { get; set; }

        public ArrayProperty(string name, int value)
        {
            Name = name;
            Value = value;
        }
    }
}

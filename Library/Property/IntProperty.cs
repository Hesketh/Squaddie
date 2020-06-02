namespace Squaddie.Property
{
    public class IntProperty : IProperty
    {
        public const string TypeName = "IntProperty";

        public string Name { get; set; }
        public string Type => TypeName;
        public int Value { get; set; }

        public IntProperty(string name, int value)
        {
            Name = name;
            Value = value;
        }
    }
}

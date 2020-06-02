namespace Library.Property
{
    public class NoneProperty : IProperty
    {
        public const string TypeName = "None";

        public string Name => TypeName;
        public string Type => TypeName;
    }
}

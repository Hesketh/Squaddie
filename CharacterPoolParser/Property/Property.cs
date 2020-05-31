namespace Squaddie.Properties
{
    internal class Property : IProperty
    {
        private string name;
        private string type;
        private dynamic data;

        public Property(string name, string type, dynamic value)
        {
            this.name = name;
            this.type = type;
            data = value;
        }

        public Property(Property prop)
        {
            name = prop.Name;
            type = prop.Type;
            data = prop.Value;
        }

        public string Type
        {
            get
            {
                return type;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public dynamic Value
        {
            get
            {
                return data;
            }
            set
            {
                data = value;
            }
        }
    }
}

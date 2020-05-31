using System;

namespace Squaddie
{
    internal class NullProperty : IProperty
    {
        public string Name
        {
            get
            {
                return "None";
            }
        }

        public string Type
        {
            get
            {
                throw new Exception("Null Property: A Null property does not have a type!");
            }
        }

        public dynamic Value
        {
            get
            {
                throw new Exception("Null Property: A Null property does not have a value!");
            }
            set
            {
                throw new Exception("Null Property: A Null property cannot be given a value!");
            }
        }
    }
}

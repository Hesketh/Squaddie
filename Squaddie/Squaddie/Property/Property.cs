using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Squaddie
{
    internal class Property : IProperty
    {
        private string m_name;
        private string m_type;
        private dynamic m_data;

        public Property(string name, string type, dynamic value)
        {
            m_name = name;
            m_type = type;
            m_data = value;
        }

        public Property(Property prop)
        {
            m_name = prop.Name;
            m_type = prop.Type;
            m_data = prop.Value;
        }

        public string Type
        {
            get
            {
                return m_type;
            }
        }

        public string Name
        {
            get
            {
                return m_name;
            }
        }

        public dynamic Value
        {
            get
            {
                return m_data;
            }
            set
            {
                m_data = value;
            }
        }
    }
}

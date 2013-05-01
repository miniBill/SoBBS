using System.Collections.Generic;
using Sobbs.Data.List;

namespace Sobbs.Config.Ini
{
    public class IniSection
    {
        public string this[string key]
        {
            get
            {
                return Values.Lookup(key);
            }
        }

        public string Name
        {
            get;
            private set;
        }

        private IImmutableList<KeyValuePair<string, string>> Values
        {
            get;
            set;
        }

        public IniSection(string name, IImmutableList<KeyValuePair<string, string>> values)
        {
            Name = name;
            Values = values;
        }
    }
}

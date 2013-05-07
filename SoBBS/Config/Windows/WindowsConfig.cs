using Sobbs.Functional.Data.List;
using System.Collections.Generic;
using Sobbs.Functional.Data.Maybe;

namespace Sobbs.Config.Windows
{
    public class WindowsConfig:IEnumerable<WindowConfig>
    {
        private IImmutableList<WindowConfig> Configurations
        {
            get; set;
        }

        public IMaybe<WindowConfig> this[string key]
        {
            get
            {
                return Configurations.Lookup(key, conf => conf.Name);
            }
        }

        public WindowsConfig(IImmutableList<WindowConfig> configurations)
        {
            Configurations = configurations;
        }

        public System.Collections.IEnumerator GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator<WindowConfig> IEnumerable<WindowConfig>.GetEnumerator()
        {
            return Configurations.GetEnumerator();
        }
    }
}


using System.Collections;
using Sobbs.Functional.Data.List;
using System.Collections.Generic;

namespace Sobbs.Config.Windows
{
    public class WindowsConfig:IEnumerable<WindowConfig>
    {
        private IImmutableList<WindowConfig> Configurations
        {
            get; set;
        }

        public IEnumerable<WindowConfig> this[string key]
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

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<WindowConfig> GetEnumerator()
        {
            return Configurations.GetEnumerator();
        }
    }
}


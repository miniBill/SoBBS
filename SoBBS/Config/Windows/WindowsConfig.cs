using Sobbs.Data.List;
using Sobbs.Functional.Data.List;

namespace Sobbs.Config.Windows
{
    public class WindowsConfig
    {
        private IImmutableList<WindowConfig> Configurations
        {
            get; set;
        }

        public WindowConfig this[string key]
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
    }
}


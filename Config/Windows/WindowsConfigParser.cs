using Sobbs.Config.Ini;
using Sobbs.Config.Sizes;
using Sobbs.Data.List;

namespace Sobbs.Config.Windows
{
    public static class WindowsConfigParser
    {
        public static WindowsConfig Parse(string path)
        {
            IImmutableList<IniSection> tree = IniParser.Parse(path);
            return new WindowsConfig(tree.Map(ToWindowConfig));
        }

        private static WindowConfig ToWindowConfig(IniSection sec)
        {
            var left = Size.Parse(sec["left"]);
            var top = Size.Parse(sec["top"]);
            var width = Size.Parse(sec["width"]);
            var height = Size.Parse(sec["height"]);
            return new WindowConfig(sec.Name, left, top, width, height);
        }
    }
}

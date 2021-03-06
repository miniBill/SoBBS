using Sobbs.Config.Ini;
using Sobbs.Config.Sizes;
using System.IO;
using Sobbs.Functional.Data.List;

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

        public static void CreateDefaultConfig(string path)
        {
            using (var writer = new StreamWriter(path, false))
            {
                writer.WriteLine("[Zones]");
                writer.WriteLine("top   =  0");
                writer.WriteLine("left  =  0");
                writer.WriteLine("width = 30%");
                writer.WriteLine("height=  *");
                writer.WriteLine("[Threads]");
                writer.WriteLine("top   =  0");
                writer.WriteLine("left  = 30%");
                writer.WriteLine("width =  *");
                writer.WriteLine("height= 50%");
                writer.WriteLine("[Messages]");
                writer.WriteLine("top   = 50%");
                writer.WriteLine("left  = 30%");
                writer.WriteLine("width =  *");
                writer.WriteLine("height=  *");
            }
        }
    }
}

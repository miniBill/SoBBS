using System;
using System.Collections.Generic;
using System.Linq;

namespace Sobbs.Config.Ini
{
	public class IniTree
	{
		public IEnumerable<IniSection> Sections { 
			get;
			private set;
		}

		public IniTree (IEnumerable<IniSection> sections)
		{
			Sections = sections.ToList();
		}

	}
}

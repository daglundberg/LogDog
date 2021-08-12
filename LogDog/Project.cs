using DotLiquid;

namespace LogDog
{
	public class Project : Drop
	{
        public string Directory { get; set; }
		public ChangeLogItem[] ChangeLogItems { get; set; }
		public string Name { get; set; }
        public string Copyright { get; set; }

		public SemanticVersion LatestVersion { get; set; }
	}
}
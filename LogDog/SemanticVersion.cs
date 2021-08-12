using DotLiquid;
using System;
using System.Text.RegularExpressions;

namespace LogDog
{
	public struct SemanticVersion : ILiquidizable
	{
		public SemanticVersion(string version)
		{
            Regex regex = new Regex(
                @"^(0|[1-9]\d*)\.(0|[1-9]\d*)\.(0|[1-9]\d*)(?:-((?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*)(?:\.(?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*))*))?(?:\+([0-9a-zA-Z-]+(?:\.[0-9a-zA-Z-]+)*))?$");

            MatchCollection matches = regex.Matches(version);
            if (matches.Count > 0)
			{
                Major = int.Parse(matches[0].Groups[1].Value);
                Minor = int.Parse(matches[0].Groups[2].Value);
                Patch = int.Parse(matches[0].Groups[3].Value);
                Label = matches[0].Groups[4].Value;
            }
            else				
                throw new Exception("Not a Semantic Version!");				
		}

		public SemanticVersion(int major, int minor, int patch, string label = "")
		{
            Major = major;
            Minor = minor;
            Patch = patch;
            Label = label;
		}
        
        public int Major;
        public int Minor;
        public int Patch;
        public string Label;

		public override string ToString()
		{
            if (String.IsNullOrWhiteSpace(Label))
                return $"{Major}.{Minor}.{Patch}";

            return $"{Major}.{Minor}.{Patch}-{Label}";
		}

		public object ToLiquid()
		{
            return ToString();
		}
	}
}